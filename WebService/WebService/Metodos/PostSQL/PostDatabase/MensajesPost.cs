using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WebService.Conexion.PostGresSQL;

namespace WebService.Metodos.PostSQL.PostDatabase
{
    public class MensajesPost
    {
        public bool is_Error { set; get; }

        public string Error_Descripcion { get; set; }

        #region guardarMensaje
        public void guardarMensaje(int conv_id, string message,string username)
        {
            username = username.ToLower();
            DateTime mes_fecha = DateTime.Now;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into conv_message (conv_id,username,message,mes_fecha) values(@conv_id,@username,@message,@mes_fecha)");
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
                new NpgsqlParameter{
                    ParameterName="mes_fecha",
                    NpgsqlDbType=NpgsqlDbType.TimestampTZ,
                    NpgsqlValue=mes_fecha
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
    }
}