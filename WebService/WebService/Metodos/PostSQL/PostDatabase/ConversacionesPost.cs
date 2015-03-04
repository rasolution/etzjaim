using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebService.Conexion.PostGresSQL;
using WebService.Estructuras.Conversaciones;

namespace WebService.Metodos.PostSQL.PostDatabase
{
    public class ConversacionesPost
    {
        public bool IsError { get; set; }

        public string ErrorDescripcion { get; set; }

        #region crearConversacion
        public void crearConversacion(string username1, string username2)
        {
            username2 = username2.ToLower();
            username1 = username1.ToLower();
            var sql =new StringBuilder();
            sql.AppendLine("insert into conversaciones(username1,username2,user1_spam,user2_spam,user1_estado,user2_estado) values (@username1,@username2,'0','0','0','0')");
            var parametros = new List<NpgsqlParameter>
            {
                 new NpgsqlParameter{
                    ParameterName="username1",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username1
                },
                 new NpgsqlParameter{
                    ParameterName="username2",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username2
                },     
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        #endregion

        #region cargarConversaciones
        public List<Conversaciones> cargarConversaciones(string username)
        {
            username = username.ToLower();
            List<Conversaciones> conversaciones = new List<Conversaciones>();
            var sql = new StringBuilder();
            sql.AppendLine("select * from conversaciones where username1=@username or username2=@username");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username
                },      
            };
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(),parametros);
          
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                Conversaciones conversacion = new Conversaciones();
                conversacion.conv_id = Convert.ToInt32(item["conv_id"]);
                conversacion.username1 = item["username1"].ToString();
                conversacion.username2 = item["username2"].ToString();
                conversacion.user1_spam = Convert.ToInt32(item["user1_spam"]);
                conversacion.user2_spam = Convert.ToInt32(item["user2_spam"]);
                conversacion.user1_estado = Convert.ToInt32(item["user1_estado"]);
                conversacion.user2_estado = Convert.ToInt32(item["user2_estado"]);
                conversaciones.Add(conversacion);
            }
            return conversaciones;
        }
        #endregion

        #region cargarConversacion
        public Conversaciones cargarConversacion(int conv_id)
        {
            Conversaciones conversacion = new Conversaciones();
            var sql = new StringBuilder();
            sql.AppendLine("select * from conversaciones where  conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },      
            };
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);

            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                
                conversacion.conv_id = Convert.ToInt32(item["conv_id"]);
                conversacion.username1 = item["username1"].ToString();
                conversacion.username2 = item["username2"].ToString();
                conversacion.user1_spam = Convert.ToInt32(item["user1_spam"]);
                conversacion.user2_spam = Convert.ToInt32(item["user2_spam"]);
                conversacion.user1_estado = Convert.ToInt32(item["user1_estado"]);
                conversacion.user2_estado = Convert.ToInt32(item["user2_estado"]);
            }
            return conversacion;
        }
        #endregion

        #region verificarConversacion
        public Conversaciones verificarConversacion(string username1, string username2)
        {
            username1 = username1.ToLower();
            username2 = username2.ToLower();
            Conversaciones conversacion = new Conversaciones();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select * from conversaciones where (username1=@username1 or username2=@username1) and (username1=@username2 or username2=@username2");
            var parametros = new List<NpgsqlParameter>{
                    new NpgsqlParameter{
                        ParameterName="username1",
                        NpgsqlDbType=NpgsqlDbType.Varchar,
                        NpgsqlValue=username1,
                    },
                    new NpgsqlParameter{
                        ParameterName="username2",
                        NpgsqlDbType=NpgsqlDbType.Varchar,
                        NpgsqlValue=username2,
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

        #region agregarSpam
        public void agregarSpam(string username, int conv_id)
        {
            username = username.ToLower();
            string usernametoedit= "username";
            string userspam = "";
            Conversaciones conversacion = cargarConversacion(conv_id);
            if (conversacion.username1.Equals(username))
            {
                usernametoedit = usernametoedit + "1";
               userspam = "user1_spam";
            }
            else
            {
                usernametoedit = usernametoedit + "2";
                userspam = "user2_spam";
            }
            var sql = new StringBuilder();
            sql.AppendLine("update conversaciones set " + userspam + "='1' where " + usernametoedit + "=@username and conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                 new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username
                }, 
                 new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
     
        #endregion

        #region quitarSpam
        public void quitarSpam(string username, int conv_id)
        {
            username = username.ToLower();
            string usernametoedit = "username";
            string userspam = "";
            Conversaciones conversacion = cargarConversacion(conv_id);
            if (conversacion.username1.Equals(username))
            {
                usernametoedit = usernametoedit + "1";
                userspam = "user1_spam";
            }
            else
            {
                usernametoedit = usernametoedit + "2";
                userspam = "user2_spam";
            }
            var sql = new StringBuilder();
            sql.AppendLine("update conversaciones set " + userspam + "='0' where " + usernametoedit + "=@username and conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                 new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username
                }, 
                 new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        #endregion

        #region estadoLeido
        public void estadoLeido(string username, int conv_id)
        {
            username = username.ToLower();
            string usernametoedit = "username";
            string userspam = "";
            Conversaciones conversacion = cargarConversacion(conv_id);
            if (conversacion.username1.Equals(username))
            {
                usernametoedit = usernametoedit + "1";
                userspam = "user1_estado";
            }
            else
            {
                usernametoedit = usernametoedit + "2";
                userspam = "user2_estado";
            }
            var sql = new StringBuilder();
            sql.AppendLine("update conversaciones set " + userspam + "='0' where " + usernametoedit + "=@username and conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                 new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username
                }, 
                 new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion

        #region estadoNoLeido
        public void estadoNoLeido(string username, int conv_id)
        {
            username = username.ToLower();
            string usernametoedit = "username";
            string userspam = "";
            Conversaciones conversacion = cargarConversacion(conv_id);
            if (conversacion.username1.Equals(username))
            {
                usernametoedit = usernametoedit + "1";
                userspam = "user1_estado";
            }
            else
            {
                usernametoedit = usernametoedit + "2";
                userspam = "user2_estado";
            }
            var sql = new StringBuilder();
            sql.AppendLine("update conversaciones set " + userspam + "='1' where " + usernametoedit + "=@username and conv_id=@conv_id");
            var parametros = new List<NpgsqlParameter>
            {
                 new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username
                }, 
                 new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion

        #region cargarConversacionesSpamUsuario
        public List<Conversaciones> cargarConversacionesSpamUsuario(string username)
        {
            username = username.ToLower();
            List<Conversaciones> conversaciones = cargarConversaciones(username);
            List<Conversaciones> conversacionesfinal=new List<Conversaciones>();
            for (int i = 0; i < conversaciones.Count; i++)
            {
                if(conversaciones[i].username1.Equals(username) && conversaciones[i].user1_spam==1){
                    conversacionesfinal.Add(conversaciones[i]);
                }
                if (conversaciones[i].username2.Equals(username) && conversaciones[i].user2_spam == 1)
                {
                    conversacionesfinal.Add(conversaciones[i]);
                }
            }
            return conversaciones;
        }
        #endregion

        #region cargarConversacionesUsuario
        public List<Conversaciones> cargarConversacionesUsuario(string username)
        {
            username = username.ToLower();
            List<Conversaciones> conversaciones = cargarConversaciones(username);
            List<Conversaciones> conversacionesfinal = new List<Conversaciones>();
            for (int i = 0; i < conversaciones.Count; i++)
            {
                if (conversaciones[i].username1.Equals(username) && conversaciones[i].user1_spam == 0)
                {
                    conversacionesfinal.Add(conversaciones[i]);
                }
                if (conversaciones[i].username2.Equals(username) && conversaciones[i].user2_spam == 0)
                {
                    conversacionesfinal.Add(conversaciones[i]);
                }
            }
            return conversaciones;
        }
        #endregion

        #region cargarConversacionesLeidas
        public List<Conversaciones> cargarConversacionesLeidas(string username)
        {
            username = username.ToLower();
            List<Conversaciones> conversaciones = cargarConversacionesUsuario(username);
            List<Conversaciones> conversacionesfinal = new List<Conversaciones>();
            for (int i = 0; i < conversaciones.Count; i++)
            {
                if (conversaciones[i].username1.Equals(username) && conversaciones[i].user1_estado== 0)
                {
                    conversacionesfinal.Add(conversaciones[i]);
                }
                if (conversaciones[i].username2.Equals(username) && conversaciones[i].user2_estado == 0)
                {
                    conversacionesfinal.Add(conversaciones[i]);
                }
            }
            return conversaciones;
        }
        #endregion

        #region cargarConversacionesNoLeidas
        public List<Conversaciones> cargarConversacionesNoLeidas(string username)
        {
            username = username.ToLower();
            List<Conversaciones> conversaciones = cargarConversacionesUsuario(username);
            List<Conversaciones> conversacionesfinal = new List<Conversaciones>();
            for (int i = 0; i < conversaciones.Count; i++)
            {
                if (conversaciones[i].username1.Equals(username) && conversaciones[i].user1_estado == 1)
                {
                    conversacionesfinal.Add(conversaciones[i]);
                }
                if (conversaciones[i].username2.Equals(username) && conversaciones[i].user2_estado == 1)
                {
                    conversacionesfinal.Add(conversaciones[i]);
                }
            }
            return conversaciones;

        }

        #endregion
    }

}