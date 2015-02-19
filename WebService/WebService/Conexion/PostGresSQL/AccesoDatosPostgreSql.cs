using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace WebService.Conexion.PostGresSQL
{
    public class AccesoDatosPostgreSql : IAccesoDatos
    {
        // Constructor
        public AccesoDatosPostgreSql(String servidor, String puerto, String usuario, String contrasena, String baseDatos)
        {
            this.LimpiarEstado();
            ConexionPost = new NpgsqlConnection("Server=" + servidor + ";Port=" + puerto + ";Database=" + baseDatos + ";User Id=" + usuario + ";Password=" + contrasena);

            Instancias += 1;
            // no puede haber + de una instancia de la clase
            if (Instancias > 1)
                return;
            try
            {
                ConexionPost.Open();
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error de Conexión \n";
                ErrorDescripcion += error.Message;
                Instancias = 0;
            }
        }

        // destructor
        ~AccesoDatosPostgreSql()
        {
            try
            {
                ConexionPost.Close();
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error de Desconexión \n";
                ErrorDescripcion += error.Message;
            }
        }

        #region Métodos de conexion

        // Indica el estado de la persistencia
        public string Estado()
        {
            string mensaje = "";
            this.LimpiarEstado();
            // estado dela conexión
            switch (ConexionPost.State)
            {
                case System.Data.ConnectionState.Broken: mensaje = "Quebrada";
                    break;
                case System.Data.ConnectionState.Closed: mensaje = "Cerrada";
                    break;
                case System.Data.ConnectionState.Connecting: mensaje = "Conectandose";
                    break;
                case System.Data.ConnectionState.Executing: mensaje = "Ejecutando";
                    break;
                case System.Data.ConnectionState.Fetching: mensaje = "Extrayendo";
                    break;
                case System.Data.ConnectionState.Open: mensaje = "Abierta";
                    break;
            }

            // cargar la propiedad con el estado de la conexion
            
            return mensaje;
        }

        public DataSet EjecutarConsultaSQL(String sql)
        {
            this.LimpiarEstado();
            var oDataSet = new DataSet();
            try
            {
                var oDataAdapter = new NpgsqlDataAdapter(sql, ConexionPost);
                if (this.HayTransaccion)
                {
                    oDataAdapter.SelectCommand.Transaction = this.Transaccion;
                }
                oDataAdapter.Fill(oDataSet);
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error en ejecutarConsultaSQL \n";
                ErrorDescripcion += error.Message;
            }
            return oDataSet;
        }

        public DataSet EjecutarConsultaSQL(String sql, IEnumerable parametros)
        {
            this.LimpiarEstado();
            var oDataSet = new DataSet();
            try
            {
                var oDataAdapter = new NpgsqlDataAdapter(sql, ConexionPost);
                var cmd = new NpgsqlCommand(sql, ConexionPost);
                foreach (NpgsqlParameter parametro in parametros)
                {
                    cmd.Parameters.Add(parametro);
                }
                oDataAdapter.SelectCommand = cmd;

                if (this.HayTransaccion)
                {
                    oDataAdapter.SelectCommand.Transaction = this.Transaccion;
                }
                oDataAdapter.Fill(oDataSet);
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error en ejecutarConsultaSQL \n";
                ErrorDescripcion += error.Message;
            }
            return oDataSet;
        }

        public void EjecutarSQL(string sql, IEnumerable parametros)
        {
            try
            {
                this.LimpiarEstado();
                var cmd = new NpgsqlCommand(sql, ConexionPost);
                foreach (NpgsqlParameter parametro in parametros)
                {
                    cmd.Parameters.Add(parametro);
                }
                if (this.HayTransaccion)
                {
                    cmd.Transaction = this.Transaccion;
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error en ejecutarSQL \n";
                ErrorDescripcion += error.Message;
            }
        }

        // Método para manipular DQL (Select) Para busquedas escalares SUM(), Count(), Etc.
        public Int32 EjecutarScalarSQL(String pSql)
        {
            int resultado = 0;
            var cmd = new NpgsqlCommand(pSql, ConexionPost);
            this.IsError = false;

            // capturar la excepción
            try
            {
                resultado = (Int32)cmd.ExecuteScalar();
            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error en ejecutarConsultaSQL \n";
                ErrorDescripcion += error.Message;
            }
            return resultado;
        }

        // Método para manipular DQL (Select) Exclusivo para carga de listas y combos
        public DataSet EjecutarSQLListas(String sql, String tabla)
        {
            var vDatos = new NpgsqlDataAdapter(sql, ConexionPost);
            var dsTabla = new DataSet();
            this.IsError = false;

            // capturar la excepción
            try
            {
                vDatos.Fill(dsTabla, tabla);

            }
            catch (Exception error)
            {
                IsError = true;
                ErrorDescripcion = "Error en ejecutarConsultaSQL \n";
                ErrorDescripcion += error.Message;
            }
            return dsTabla;
        }

        #endregion

        #region Set & Gets

        public Boolean IsError { set; get; }

        public String ErrorDescripcion { set; get; }

        public static int Instancias { set; get; }

        public bool HayTransaccion { set; get; }       

        public NpgsqlTransaction Transaccion { set; get; }
      
        public NpgsqlConnection ConexionPost { set; get; }

        #endregion

        #region Métodos de la clase

        public void LimpiarEstado()
        {
            this.ErrorDescripcion = "";
            this.IsError = false;
        }

        public void IniciarTransaccion()
        {
            if (this.HayTransaccion == false)
            {
                this.Transaccion = this.ConexionPost.BeginTransaction();
                this.HayTransaccion = true;
            }
        }

        public void CommitTransaccion()
        {
            if (this.HayTransaccion)
            {
                this.Transaccion.Commit();
                this.HayTransaccion = false;
            }
        }

        public void RollbackTransaccion()
        {
            if (this.HayTransaccion)
            {
                this.Transaccion.Rollback();
                this.HayTransaccion = false;
            }
        }

        #endregion
    }
}