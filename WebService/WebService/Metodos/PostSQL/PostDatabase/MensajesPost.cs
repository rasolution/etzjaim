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
        public void guardarMensaje(int conv_id, string mes_message)
        {
            DateTime mes_salida = DateTime.Now;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into mensajes (conv_id,mes_salida,mes_message) values(@conv_id,@mes_salida,@mes_message)");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="conv_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=conv_id
                },
                new NpgsqlParameter{
                    ParameterName="mes_salida",
                    NpgsqlDbType=NpgsqlDbType.Timestamp,
                    NpgsqlValue=mes_salida
                },
                new NpgsqlParameter{
                    ParameterName="mes_message",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=mes_message
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