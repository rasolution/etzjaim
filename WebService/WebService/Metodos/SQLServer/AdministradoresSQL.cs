using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebService.Conexion.SQLServer;
namespace WebService.Metodos.SQLServer
{
    public class AdministradoresSQL
    {
        public bool IsError
        {
            get;
            set;
        }

        public string ErrorDescripcion
        {
            get;
            set;
        }

        public void GuardarAdministrativo(String username, String password)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = provider.ComputeHash(data);
            string md5 = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                md5 += data[i].ToString("x2").ToLower();
            }
            password = md5;
            var sql = new StringBuilder();
            sql.AppendLine("insert into administradores (admin_username,admin_password) values (@username,@password)");
            var parametros = new List<SqlParameter>
                {
                    new SqlParameter
                        {
                            ParameterName = "username",
                            SqlDbType =SqlDbType.VarChar,
                            SqlValue =  username
                        },
                        new SqlParameter
                            {
                            ParameterName = "password",
                            SqlDbType = SqlDbType.VarChar,
                            SqlValue = password
                        },
                };
            AccesoDatosSQL.Instance.Accesar.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosSQL.Instance.Accesar.HayError)
            {
                this.IsError = AccesoDatosSQL.Instance.Accesar.HayError;
                this.ErrorDescripcion = AccesoDatosSQL.Instance.Accesar.ErrorDescripcion;
            }
        }
    }
}