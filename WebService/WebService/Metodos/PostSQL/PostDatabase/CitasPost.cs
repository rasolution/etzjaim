using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebService.Conexion.PostGresSQL;
using WebService.Estructuras;

namespace WebService.Metodos.PostSQL.PostDatabase
{
    public class CitasPost
    {
        public bool Is_error { get; set; }

        public string Error_Descripcion { get; set; }

        #region guardarCita
        public void guardarCita(int cl_id, DateTime cita_fecha)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into citas(cl_id,cita_fecha,cita_estado) values(@cl_id,@cita_fecha,'0')");
            var parametros = new List<NpgsqlParameter>
            {
                 new NpgsqlParameter{
                        ParameterName="cl_id",
                        NpgsqlDbType=NpgsqlDbType.Integer,
                        NpgsqlValue=cl_id,
                    },
                 new NpgsqlParameter{
                        ParameterName="cita_fecha",
                        NpgsqlDbType=NpgsqlDbType.Timestamp,
                        NpgsqlValue=cita_fecha,
                 },
            };

            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.Is_error = AccesoDatosPost.Instance.accesodatos.IsError;
                this.Error_Descripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion

        #region borrarCita(intcita_id)
        public void borrarCita(int cita_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("delete from citas where cita_id=@cita_id");
            var parametros = new List<NpgsqlParameter>
            {
                 new NpgsqlParameter{
                        ParameterName="cita_id",
                        NpgsqlDbType=NpgsqlDbType.Integer,
                        NpgsqlValue=cita_id,
                    },
            };

            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.Is_error = AccesoDatosPost.Instance.accesodatos.IsError;
                this.Error_Descripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        #endregion

        #region aprobarCita
        public void aprobarCita(int cita_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update citas set cita_estado='1' where cita_id=@cita_id");
            var parametros = new List<NpgsqlParameter>
            {
                 new NpgsqlParameter{
                        ParameterName="cita_id",
                        NpgsqlDbType=NpgsqlDbType.Integer,
                        NpgsqlValue=cita_id,
                    },
            };

            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.Is_error = AccesoDatosPost.Instance.accesodatos.IsError;
                this.Error_Descripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        #endregion

        #region CargarCitas
        public List<Cita_cliente> cargarCitas()
        {
            List<Cita_cliente> citas = new List<Cita_cliente>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.cita_id,a.cl_id,b.cl_nombre,b.cl_apellidos,b.cl_username,a.cita_fecha,a.cita_estado from citas as a, clientes as b where a.cl_id=b.cl_id");
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString());
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                Cita_cliente cita = new Cita_cliente();
                cita.cita_id = Convert.ToInt32(item["cita_id"]);
                cita.cl_id=Convert.ToInt32(item["cl_id"]);
                cita.cl_Nombre_Apellidos = item["cl_nombre"].ToString() + " " + item["cl_apellidos"].ToString();
                cita.cl_username = item["cl_username"].ToString();
                cita.cita_fecha = Convert.ToDateTime(item["cita_fecha"]);
                cita.cita_estado = Convert.ToInt32(item["cita_estado"]);
                citas.Add(cita);
            }
            return citas;
        }
        #endregion

        #region cargarCita
        public Cita_cliente cargarCita(int cita_id)
        {
           Cita_cliente cita= new Cita_cliente();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.cita_id,a.cl_id,b.cl_nombre,b.cl_apellidos,b.cl_username,a.cita_fecha,a.cita_estado from citas as a, clientes as b where a.cl_id=b.cl_id and a.cita_id=@cita_id");
            var parametros = new List<NpgsqlParameter>
            {
                 new NpgsqlParameter{
                        ParameterName="cita_id",
                        NpgsqlDbType=NpgsqlDbType.Integer,
                        NpgsqlValue=cita_id,
                    },
            };
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(),parametros);
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                cita.cita_id = Convert.ToInt32(item["cita_id"]);
                cita.cl_id = Convert.ToInt32(item["cl_id"]);
                cita.cl_Nombre_Apellidos = item["cl_nombre"].ToString() + " " + item["cl_apellidos"].ToString();
                cita.cl_username = item["cl_username"].ToString();
                cita.cita_fecha = Convert.ToDateTime(item["cita_fecha"]);
                cita.cita_estado = Convert.ToInt32(item["cita_estado"]);
                
            }
            return cita;
        }

        #endregion

        #region cargarCitasAprobadas
        public List<Cita_cliente> cargarCitasAprobadas()
        {
            List<Cita_cliente> citas = new List<Cita_cliente>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.cita_id,a.cl_id,b.cl_nombre,b.cl_apellidos,b.cl_username,a.cita_fecha,a.cita_estado from citas as a, clientes as b where a.cl_id=b.cl_id and cita_estado='1'");
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString());
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                Cita_cliente cita = new Cita_cliente();
                cita.cita_id = Convert.ToInt32(item["cita_id"]);
                cita.cl_id = Convert.ToInt32(item["cl_id"]);
                cita.cl_Nombre_Apellidos = item["cl_nombre"].ToString() + " " + item["cl_apellidos"].ToString();
                cita.cl_username = item["cl_username"].ToString();
                cita.cita_fecha = Convert.ToDateTime(item["cita_fecha"]);
                cita.cita_estado = Convert.ToInt32(item["cita_estado"]);
                citas.Add(cita);
            }
            return citas;
        }
        #endregion

        #region cargarCitasNoAprobadas
        public List<Cita_cliente> cargarCitasNoAprobadas()
        {
            List<Cita_cliente> citas = new List<Cita_cliente>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.cita_id,a.cl_id,b.cl_nombre,b.cl_apellidos,b.cl_username,a.cita_fecha,a.cita_estado from citas as a, clientes as b where a.cl_id=b.cl_id and cita_estado='0'");
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString());
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                Cita_cliente cita = new Cita_cliente();
                cita.cita_id = Convert.ToInt32(item["cita_id"]);
                cita.cl_id = Convert.ToInt32(item["cl_id"]);
                cita.cl_Nombre_Apellidos = item["cl_nombre"].ToString() + " " + item["cl_apellidos"].ToString();
                cita.cl_username = item["cl_username"].ToString();
                cita.cita_fecha = Convert.ToDateTime(item["cita_fecha"]);
                cita.cita_estado = Convert.ToInt32(item["cita_estado"]);
                citas.Add(cita);
            }
            return citas;
        }
        #endregion

        #region verificarCita_Fecha
        public bool verficiarCita_Fecha(DateTime cita_fecha)
        {
            bool result = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select * from citas where cita_fecha=@cita_fecha");
            var parametros = new List<NpgsqlParameter>{
                    new NpgsqlParameter{
                        ParameterName="cita_fecha",
                        NpgsqlDbType=NpgsqlDbType.Timestamp,
                        NpgsqlValue=cita_fecha,
                    },
                };

            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            Cita cita = new Cita();
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                cita.cl_id = Convert.ToInt32(item["cl_id"]);
                cita.cita_id = Convert.ToInt32(item["cita_id"]);
                cita.cita_estado = Convert.ToInt32(item["cita_estado"]);
                cita.cita_fecha = Convert.ToDateTime(item["cita_fecha"]);
            }
            if (cita.cita_estado == 1)
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region verificarCita
        public bool verificarCita(int cita_id)
        {
            bool result = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select * from citas where cita_id=@cita_id");
            var parametros = new List<NpgsqlParameter>{
                    new NpgsqlParameter{
                        ParameterName="cita_id",
                        NpgsqlDbType=NpgsqlDbType.Integer,
                        NpgsqlValue=cita_id,
                    },
                };

            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            Cita cita = new Cita();
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                cita.cl_id = Convert.ToInt32(item["cl_id"]);
                cita.cita_id = Convert.ToInt32(item["cita_id"]);
                cita.cita_estado = Convert.ToInt32(item["cita_estado"]);
                cita.cita_fecha = Convert.ToDateTime(item["cita_fecha"]);
            }
            if (cita.cita_estado == 1)
            {
                result = true;
            }
            return result;
        }
        #endregion
    }
}