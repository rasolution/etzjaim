using System;
using System.Collections;
using System.Data;
using Npgsql;

namespace WebService.Conexion.PostGresSQL
{
    public class AccesoDatosPostgreSql : IAccesoDatos
    {
        public AccesoDatosPostgreSql(string servidor, string puerto, string usuario, string contrasena, string baseDatos)
        {
            this.LimpiarEstado();
            string connStr = string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4}",
                servidor ?? "localhost",
                puerto ?? "5432",
                baseDatos ?? "etzjaim",
                usuario ?? "etzjaim",
                contrasena ?? string.Empty);

            ConexionPost = new NpgsqlConnection(connStr);
            Instancias += 1;

            try
            {
                ConexionPost.Open();
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error de Conexión \n" + error.Message;
            }
        }

        ~AccesoDatosPostgreSql()
        {
            try
            {
                if (ConexionPost != null && ConexionPost.State != ConnectionState.Closed)
                {
                    ConexionPost.Close();
                }
            }
            catch
            {
                // Ignore finalizer disposal issues.
            }
        }

        private bool EnsureOpen()
        {
            if (ConexionPost == null)
            {
                IsError = true;
                ErrorDescripcion = "Conexión PostgreSQL no inicializada.";
                return false;
            }

            if (ConexionPost.State == ConnectionState.Open)
            {
                return true;
            }

            try
            {
                if (ConexionPost.State == ConnectionState.Broken)
                {
                    ConexionPost.Close();
                }
                ConexionPost.Open();
                return true;
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error de Conexión \n" + error.Message;
                return false;
            }
        }

        public bool Estado()
        {
            this.LimpiarEstado();
            if (ConexionPost == null)
            {
                return false;
            }

            return ConexionPost.State == ConnectionState.Open
                || ConexionPost.State == ConnectionState.Executing
                || ConexionPost.State == ConnectionState.Fetching;
        }

        public DataSet EjecutarConsultaSQL(string sql)
        {
            this.LimpiarEstado();
            var oDataSet = new DataSet();
            if (!EnsureOpen())
            {
                return oDataSet;
            }

            try
            {
                using (var cmd = new NpgsqlCommand(sql, ConexionPost))
                using (var oDataAdapter = new NpgsqlDataAdapter(cmd))
                {
                    if (this.HayTransaccion)
                    {
                        cmd.Transaction = this.Transaccion;
                    }
                    oDataAdapter.Fill(oDataSet);
                }
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error en ejecutarConsultaSQL \n" + error.Message;
            }
            return oDataSet;
        }

        public DataSet EjecutarConsultaSQL(string sql, IEnumerable parametros)
        {
            this.LimpiarEstado();
            var oDataSet = new DataSet();
            if (!EnsureOpen())
            {
                return oDataSet;
            }

            try
            {
                using (var cmd = new NpgsqlCommand(sql, ConexionPost))
                using (var oDataAdapter = new NpgsqlDataAdapter(cmd))
                {
                    if (parametros != null)
                    {
                        foreach (NpgsqlParameter parametro in parametros)
                        {
                            cmd.Parameters.Add(parametro);
                        }
                    }

                    if (this.HayTransaccion)
                    {
                        cmd.Transaction = this.Transaccion;
                    }
                    oDataAdapter.Fill(oDataSet);
                }
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error en ejecutarConsultaSQL \n" + error.Message;
            }
            return oDataSet;
        }

        public void EjecutarSQL(string sql, IEnumerable parametros)
        {
            this.LimpiarEstado();
            if (!EnsureOpen())
            {
                return;
            }

            try
            {
                using (var cmd = new NpgsqlCommand(sql, ConexionPost))
                {
                    if (parametros != null)
                    {
                        foreach (NpgsqlParameter parametro in parametros)
                        {
                            cmd.Parameters.Add(parametro);
                        }
                    }

                    if (this.HayTransaccion)
                    {
                        cmd.Transaction = this.Transaccion;
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error en ejecutarSQL \n" + error.Message;
            }
        }

        public int EjecutarScalarSQL(string pSql)
        {
            this.LimpiarEstado();
            if (!EnsureOpen())
            {
                return 0;
            }

            try
            {
                using (var cmd = new NpgsqlCommand(pSql, ConexionPost))
                {
                    object val = cmd.ExecuteScalar();
                    return val == null || val == DBNull.Value ? 0 : Convert.ToInt32(val);
                }
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error en ejecutarConsultaSQL \n" + error.Message;
                return 0;
            }
        }

        public DataSet EjecutarSQLListas(string sql, string tabla)
        {
            this.LimpiarEstado();
            var dsTabla = new DataSet();
            if (!EnsureOpen())
            {
                return dsTabla;
            }

            try
            {
                using (var vDatos = new NpgsqlDataAdapter(sql, ConexionPost))
                {
                    vDatos.Fill(dsTabla, tabla);
                }
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error en ejecutarConsultaSQL \n" + error.Message;
            }
            return dsTabla;
        }

        public bool IsError { set; get; }
        public string ErrorDescripcion { set; get; }
        public static int Instancias { set; get; }
        public bool HayTransaccion { set; get; }
        public NpgsqlTransaction Transaccion { set; get; }
        public NpgsqlConnection ConexionPost { set; get; }

        public void LimpiarEstado()
        {
            this.ErrorDescripcion = string.Empty;
            this.IsError = false;
        }

        public void IniciarTransaccion()
        {
            if (!this.HayTransaccion && EnsureOpen())
            {
                this.Transaccion = this.ConexionPost.BeginTransaction();
                this.HayTransaccion = true;
            }
        }

        public void CommitTransaccion()
        {
            if (this.HayTransaccion && this.Transaccion != null)
            {
                this.Transaccion.Commit();
                this.HayTransaccion = false;
            }
        }

        public void RollbackTransaccion()
        {
            if (this.HayTransaccion && this.Transaccion != null)
            {
                this.Transaccion.Rollback();
                this.HayTransaccion = false;
            }
        }
    }
}
