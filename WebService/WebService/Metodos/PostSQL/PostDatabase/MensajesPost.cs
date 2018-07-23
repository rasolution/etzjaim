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
    public class MensajesPost
    {
        public bool is_Error { set; get; }

        public string Error_Descripcion { get; set; }

        #region guardarMensaje
        public void guardarMensaje(int conv_id, string message, string username)
        {
            username = username.ToLower();
            DateTime mes_fecha = System.DateTime.Now;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into conv_message (conv_id,username,message,mes_fecha) values(@conv_id,@username,@message,'" + mes_fecha + "')");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
                  new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username
                },
                  new NpgsqlParameter{
                    ParameterName="message",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=message
                },
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.is_Error = AccesoDatosPost.Instance.accesodatos.IsError;
                this.Error_Descripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        #endregion


        #region cargarMensajes
        public List<Conv_Message> cargarMensajes(int conv_id)
        {
            List<Conv_Message> messages = new List<Conv_Message>();
            var sql = new StringBuilder();
            sql.AppendLine("select * from  conv_message where conv_id=@conv_id order by mes_fecha DESC");
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
                Conv_Message message = new Conv_Message();
                message.id = Convert.ToInt32(item["mes_id"]);
                message.message = item["message"].ToString();
                message.username = item["username"].ToString();
                message.mes_fecha = Convert.ToDateTime(item["mes_fecha"]);
                message.conv_id = Convert.ToInt32(item["conv_id"]);
                messages.Add(message);
            }
            return messages;
        }
    }
    #endregion
}