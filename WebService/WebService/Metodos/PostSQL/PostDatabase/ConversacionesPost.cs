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
    public class ConversacionesPost
    {
        public bool IsErro { get; set; }

        public string ErrorDescripcion { get; set; }

        #region crearConversacion
        public void crearConversacion(int cl_id, int admin_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into conversaciones (cl_id,admin_id,conv_spam_admin,conv_spam_cliente,conv_estado_admin,conv_estado_cliente) values(@cl_id,@admin_id,'0','0','0','0')");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="cl_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=cl_id
                },
                new NpgsqlParameter{
                    ParameterName="admin_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=admin_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsErro = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        #endregion

        //Conversaciones Cliente
        #region cargarConversacionesCliente
        public List<Conversaciones_Cliente_Admin> cargarConversacionsCliente(int cl_id)
        {
            List<Conversaciones_Cliente_Admin> conversaciones = new List<Conversaciones_Cliente_Admin>();
            StringBuilder sql = new StringBuilder();
            string consulta="select a.conv_id,a.cl_id,a.admin_id,a.conv_spam_cliente,a.conv_estado_cliente,b.cl_username,c.admin_username"+
                " from conversaciones as a,clientes as b,administradores as c where a.cl_id=@cl_id and a.admin_id=c.admin_id and a.conv_spam_cliente='0' ";
            sql.AppendLine(consulta);
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="cl_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=cl_id
                },
               
            };
            var datos=AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            foreach (DataRow item in datos.Tables[0].Rows)
            {
                Conversaciones_Cliente_Admin conversacion = new Conversaciones_Cliente_Admin();
                conversacion.conv_id = Convert.ToInt32(item["conv_id"]);
                conversacion.cl_id = Convert.ToInt32(item["cl_id"]);
                conversacion.admin_id = Convert.ToInt32(item["admin_id"]);
                conversacion.conv_spam = Convert.ToInt32(item["conv_spam_cliente"]);
                conversacion.conv_estado = Convert.ToInt32(item["conv_estado_cliente"]);
                conversacion.cl_username = item["cl_username"].ToString();
                conversacion.admin_username = item["admin_username"].ToString();
                conversaciones.Add(conversacion);
            }
            return conversaciones;
        }
        #endregion

        #region moverConversacionASpamCliente
        public void moverConversacionASpamCliente(int conv_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update conversaciones set conv_spam_cliente='1' where conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsErro = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        #endregion

        #region quitarconversacionSpamCliente
        public void quitarconversacionSpamCliente(int conv_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update conversaciones set conv_spam_cliente='0' where conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsErro = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion

        #region cargarConversacionesClienteNoLeidas
        public List<Conversaciones_Cliente_Admin> cargarConversacionesClienteNoLeidas(int cl_id)
        {
            List<Conversaciones_Cliente_Admin> conversaciones = new List<Conversaciones_Cliente_Admin>();
            StringBuilder sql = new StringBuilder();
            string consulta = "select a.conv_id,a.cl_id,a.admin_id,a.conv_spam_cliente,a.conv_estado_admin,b.cl_username,c.admin_username" +
                " from conversaciones as a,clientes as b,administradores as c where a.cl_id=@cl_id and a.admin_id=c.admin_id and a.conv_spam_cliente='0' and conv_estado_cliente='1' ";
            sql.AppendLine(consulta);
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="cl_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=cl_id
                },
               
            };
            var datos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            foreach (DataRow item in datos.Tables[0].Rows)
            {
                Conversaciones_Cliente_Admin conversacion = new Conversaciones_Cliente_Admin();
                conversacion.conv_id = Convert.ToInt32(item["conv_id"]);
                conversacion.cl_id = Convert.ToInt32(item["cl_id"]);
                conversacion.admin_id = Convert.ToInt32(item["admin_id"]);
                conversacion.conv_spam = Convert.ToInt32(item["conv_spam_cliente"]);
                conversacion.conv_estado = Convert.ToInt32(item["conv_estado_cliente"]);
                conversacion.cl_username = item["cl_username"].ToString();
                conversacion.admin_username = item["admin_username"].ToString();
                conversaciones.Add(conversacion);
            }
            return conversaciones;
        }
        #endregion

        #region cargarConversacionSpamCliente
        public List<Conversaciones_Cliente_Admin> cargarConversacionSpamCliente(int cl_id)
        {
            List<Conversaciones_Cliente_Admin> conversaciones = new List<Conversaciones_Cliente_Admin>();
            StringBuilder sql = new StringBuilder();
            string consulta = "select a.conv_id,a.cl_id,a.admin_id,a.conv_spam_cliente,a.conv_estado_cliente,b.cl_username,c.admin_username" +
                " from conversaciones as a,clientes as b,administradores as c where a.cl_id=@cl_id and a.admin_id=c.admin_id and a.conv_spam_cliente='1' ";
            sql.AppendLine(consulta);
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="cl_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=cl_id
                },
               
            };
            var datos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            foreach (DataRow item in datos.Tables[0].Rows)
            {
                Conversaciones_Cliente_Admin conversacion = new Conversaciones_Cliente_Admin();
                conversacion.conv_id = Convert.ToInt32(item["conv_id"]);
                conversacion.cl_id = Convert.ToInt32(item["cl_id"]);
                conversacion.admin_id = Convert.ToInt32(item["admin_id"]);
                conversacion.conv_spam = Convert.ToInt32(item["conv_spam_cliente"]);
                conversacion.conv_estado = Convert.ToInt32(item["conv_estado_cliente"]);
                conversacion.cl_username = item["cl_username"].ToString();
                conversacion.admin_username = item["admin_username"].ToString();
                conversaciones.Add(conversacion);
            }
            return conversaciones;
        }
        #endregion

        #region cambiarEstadoConversacionNoLeidoCliente
        public void cambiarEstadoConversacionNoLeidoCliente(int conv_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update conversaciones set conv_estado_cliente='1' where conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsErro = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        #endregion

        #region cambiarEstadoConversacionLeidaCliente
        public void cambiarEstadoConversacionLeidaCliente(int conv_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update conversaciones set conv_estado_cliente='0' where conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsErro = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion

        //Conversaciones Administrador

        #region cargarConversacionesAdmin
        public List<Conversaciones_Cliente_Admin> cargarConversacionsAdmin(int admin_id)
        {
            List<Conversaciones_Cliente_Admin> conversaciones = new List<Conversaciones_Cliente_Admin>();
            StringBuilder sql = new StringBuilder();
            string consulta = "select a.conv_id,a.cl_id,a.admin_id,a.conv_spam_admin,a.conv_estado_admin,b.cl_username,c.admin_username" +
                " from conversaciones as a,clientes as b,administradores as c where a.cl_id=b.cl_id and a.admin_id=@admin_id a.conv_spam_admin='0'";
            sql.AppendLine(consulta);
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="admin_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=admin_id
                },
               
            };
            var datos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            foreach (DataRow item in datos.Tables[0].Rows)
            {
                Conversaciones_Cliente_Admin conversacion = new Conversaciones_Cliente_Admin();
                conversacion.conv_id = Convert.ToInt32(item["conv_id"]);
                conversacion.cl_id = Convert.ToInt32(item["cl_id"]);
                conversacion.admin_id = Convert.ToInt32(item["admin_id"]);
                conversacion.conv_spam = Convert.ToInt32(item["conv_spam_admin"]);
                conversacion.conv_estado = Convert.ToInt32(item["conv_estado_admin"]);
                conversacion.cl_username = item["cl_username"].ToString();
                conversacion.admin_username = item["admin_username"].ToString();
                conversaciones.Add(conversacion);
            }
            return conversaciones;
        }
        #endregion

        #region cargarConversacionSpamAdmin
        public List<Conversaciones_Cliente_Admin> cargarConversacionSpamAdmin(int admin_id)
        {
            List<Conversaciones_Cliente_Admin> conversaciones = new List<Conversaciones_Cliente_Admin>();
            StringBuilder sql = new StringBuilder();
            string consulta = "select a.conv_id,a.cl_id,a.admin_id,a.conv_spam_admin,a.conv_estado_admin,b.cl_username,c.admin_username" +
                " from conversaciones as a,clientes as b,administradores as c where a.cl_id=b.cl_id and a.admin_id=@admin_id a.conv_spam_admin='1'";
            sql.AppendLine(consulta);
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="admin_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=admin_id
                },
               
            };
            var datos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            foreach (DataRow item in datos.Tables[0].Rows)
            {
                Conversaciones_Cliente_Admin conversacion = new Conversaciones_Cliente_Admin();
                conversacion.conv_id = Convert.ToInt32(item["conv_id"]);
                conversacion.cl_id = Convert.ToInt32(item["cl_id"]);
                conversacion.admin_id = Convert.ToInt32(item["admin_id"]);
                conversacion.conv_spam = Convert.ToInt32(item["conv_estado_admin"]);
                conversacion.conv_estado = Convert.ToInt32(item["conv_estado_cliente"]);
                conversacion.cl_username = item["cl_username"].ToString();
                conversacion.admin_username = item["admin_username"].ToString();
                conversaciones.Add(conversacion);
            }
            return conversaciones;
        }
        #endregion

        #region moverConversacionASpamAdmin
        public void moverConversacionASpamAdmin(int conv_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update conversaciones set conv_spam_admin='1' where conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsErro = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion

        #region quitarconversacionSpamAdmin
        public void quitarconversacionSpamAdmin(int conv_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update conversaciones set conv_spam_admin='0' where conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsErro = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion

        #region cargarConversacionesAdminNoLeidas
        public List<Conversaciones_Cliente_Admin> cargarConversacionesAdminNoLeidas(int admin_id)
        {
            List<Conversaciones_Cliente_Admin> conversaciones = new List<Conversaciones_Cliente_Admin>();
            StringBuilder sql = new StringBuilder();
            string consulta = "select a.conv_id,a.cl_id,a.admin_id,a.conv_spam_admin,a.conv_estado_admin,b.cl_username,c.admin_username" +
                " from conversaciones as a,clientes as b,administradores as c where a.cl_id=b.cl_id and a.admin_id=@admin_id a.conv_spam_admin='0' a.conv_estado_admin='0'";
            sql.AppendLine(consulta);
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="admin_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=admin_id
                },
               
            };
            var datos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            foreach (DataRow item in datos.Tables[0].Rows)
            {
                Conversaciones_Cliente_Admin conversacion = new Conversaciones_Cliente_Admin();
                conversacion.conv_id = Convert.ToInt32(item["conv_id"]);
                conversacion.cl_id = Convert.ToInt32(item["cl_id"]);
                conversacion.admin_id = Convert.ToInt32(item["admin_id"]);
                conversacion.conv_spam = Convert.ToInt32(item["conv_spam_admin"]);
                conversacion.conv_estado = Convert.ToInt32(item["conv_estado_admin"]);
                conversacion.cl_username = item["cl_username"].ToString();
                conversacion.admin_username = item["admin_username"].ToString();
                conversaciones.Add(conversacion);
            }
            return conversaciones;
        }
        #endregion

        #region cambiarEstadoConversacionNoLeidoAdmin
        public void cambiarEstadoConversacionNoLeidoAdmin(int conv_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update conversaciones set conv_estado_admin='1' where conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsErro = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion

        #region cambiarEstadoConversacionLeidoAdmin
        public void cambiarEstadoConversacionLeidoAdmin(int conv_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update conversaciones set conv_estado_admin='0' where conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsErro = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion



        #region cargarConversacion
        public Conversacion cargarConversacion(int conv_id)
        {
            Conversacion conversacion = new Conversacion();
            StringBuilder sql = new StringBuilder();
            string consulta = "select * from conversaciones where conv_id=@conv_id ";
            sql.AppendLine(consulta);
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
               
            };
            var datos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            foreach (DataRow item in datos.Tables[0].Rows)
            {
                
                conversacion.conv_id = Convert.ToInt32(item["conv_id"]);
                conversacion.cl_id = Convert.ToInt32(item["cl_id"]);
                conversacion.admin_id = Convert.ToInt32(item["admin_id"]);
            }
            return conversacion;
        }
        #endregion

        #region verificarConversacion
        public Conversacion verificarConversacion(int admin_id, int cl_id)
        {
            Conversacion conversacion = new Conversacion();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select * from conversaciones where admin_id=@admin_id and cl_id=@cl_id");
            var parametros = new List<NpgsqlParameter>{
                    new NpgsqlParameter{
                        ParameterName="admin_id",
                        NpgsqlDbType=NpgsqlDbType.Integer,
                        NpgsqlValue=admin_id,
                    },
                    new NpgsqlParameter{
                        ParameterName="cl_id",
                        NpgsqlDbType=NpgsqlDbType.Integer,
                        NpgsqlValue=cl_id,
                    },
                };

            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (odatos == null)
            {
                conversacion = null;
            }
            else
            {
                foreach (DataRow item in odatos.Tables[0].Rows)
                {
                    conversacion.conv_id = Convert.ToInt32(item["conv_id"]);
                }
            }
            return conversacion;
        }
        #endregion
    }
}