

using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using WebService.Conexion.PostGresSQL;
using WebService.Estructuras;
namespace WebService.Metodos.PostSQL.PostDatabase
{
    public class AdministradoresPost
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

        #region guardarAdministrativo
        public void guardarAdministrativo(String username, String password)
        {
            username = username.ToLower();
            password = password.ToLower();
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
            var parametros = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter
                        {
                            ParameterName = "username",
                            NpgsqlDbType = NpgsqlDbType.Varchar,
                            NpgsqlValue =  username
                        },
                        new NpgsqlParameter
                            {
                            ParameterName = "password",
                            NpgsqlDbType = NpgsqlDbType.Varchar,
                            NpgsqlValue = password
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

        #region eliminarAdministrador
        public void eliminarAdministrador(int admin_id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("delete from administradores where admin_id=@admin_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="admin_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=admin_id
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

        #region cargarAdministrativos
        public List<Administrador> cargarAdministrativos()
        {
            var sql = new StringBuilder();
            sql.AppendLine("select * from administradores");
            List<Administrador> Administradores = new List<Administrador>();
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString());
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                Administradores.Add(new Administrador()
                {
                    admin_id=Convert.ToInt32(item["admin_id"]),
                    admin_username=item["admin_username"].ToString(),
                    admin_password=item["admin_password"].ToString(),
                });
            }
            return Administradores;
        }
        #endregion

        #region cargarAdmin
        public Administrador cargarAdmin(int admin_id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("select * from administradores where admin_id=@admin_id");
            Administrador Administrador = new Administrador();
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="admin_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=admin_id
                },      
            };
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(),parametros);
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                Administrador.admin_id = Convert.ToInt32(item["admin_id"]);
                Administrador.admin_username = item["admin_username"].ToString();
                Administrador.admin_password = item["admin_password"].ToString();
            }
            return Administrador;
        }
        #endregion

        #region cambiarContraseña
        public void cambiarContraseña(string admin_username, string admin_password)
        {
            admin_username = admin_username.ToLower();
            admin_password = admin_password.ToLower();
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(admin_password);
            data = provider.ComputeHash(data);
            string md5 = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                md5 += data[i].ToString("x2").ToLower();
            }
            admin_password = md5;
            var sql = new StringBuilder();
            sql.AppendLine("update administradores set admin_password=@admin_password where admin_username=@admin_username");
            var parametros = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter
                        {
                            ParameterName = "admin_username",
                            NpgsqlDbType = NpgsqlDbType.Varchar,
                            NpgsqlValue =  admin_username
                        },
                        new NpgsqlParameter
                            {
                            ParameterName = "admin_password",
                            NpgsqlDbType = NpgsqlDbType.Varchar,
                            NpgsqlValue = admin_password
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

        #region UsernameRepetido
        public bool UsernameRepetido(string admin_username)
        {
            admin_username = admin_username.ToLower();
            bool result = false;
            List<Administrador> Administradores = cargarAdministrativos();
            for (int i = 0; i < Administradores.Count; i++)
            {
                if (Administradores[i].admin_username.Equals(admin_username))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        #endregion

        #region LoginAdmin
        public bool LoginAdmin(string admin_username, string admin_password)
        {
            admin_username = admin_username.ToLower();
            admin_password = admin_password.ToLower();
            bool loginOk = false;
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(admin_password);
            data = provider.ComputeHash(data);
            string md5 = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                md5 += data[i].ToString("x2").ToLower();
            }
            admin_password = md5;
            List<Administrador> Administradores = cargarAdministrativos();
            for (int i = 0; i < Administradores.Count; i++)
            {
                if (Administradores[i].admin_username.Equals(admin_username) &&
                   Administradores[i].admin_password.Equals(admin_password))
                {
                    loginOk = true;
                    break;
                }
            }
            return loginOk;
        }
        #endregion
    }
}