using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebService.Conexion.PostGresSQL;
using WebService.Estructuras;

namespace WebService.Metodos.PostSQL.PostDatabase
{
    public class UsuarioPost
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

        #region cargarUsuarios
        public List<Usuario> cargarUsuarios()
        {
            var sql = new StringBuilder();
            sql.AppendLine("select * from usuarios");
            List<Usuario> usuarios = new List<Usuario>();
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString());
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                usuarios.Add(new Usuario()
                {
                    username = item["username"].ToString(),
                    password = item["password"].ToString(),
                    nombre = item["user_nombre"].ToString(),
                    apellidos = item["user_apellidos"].ToString(),
                    tipo = Convert.ToInt32(item["user_tipo"]),
                });
            }
            return usuarios;
        }
        #endregion

        #region cargarAdmins
        public List<Usuario> cargarAdmins_Clientes(int tipo)
        {

            var sql = new StringBuilder();
            switch (tipo)
            {
                case 0:
                    sql.AppendLine("select * from usuarios where user_tipo=0");
                    break;
                case 1:
                    sql.AppendLine("select * from usuarios where user_tipo=1");
                    break;
            }
            List<Usuario> usuarios = new List<Usuario>();
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString());
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                usuarios.Add(new Usuario()
                {
                    username = item["username"].ToString(),
                    password = item["password"].ToString(),
                    nombre = item["user_nombre"].ToString(),
                    apellidos = item["user_apellidos"].ToString(),
                    tipo = Convert.ToInt32(item["user_tipo"]),
                });
            }
            return usuarios;
        }
        #endregion

        #region cargarUsuario
        public Usuario cargarUsuario(string username)
        {
            username = username.ToLower();
            var sql = new StringBuilder();
            sql.AppendLine("select * from usuarios where username=@username");
            Usuario usuario = new Usuario();
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username
                },      
            };
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                usuario.username = item["username"].ToString();
                usuario.password = item["password"].ToString();
                usuario.nombre = item["user_nombre"].ToString();
                usuario.apellidos = item["user_apellidos"].ToString();
                usuario.tipo = Convert.ToInt32(item["user_tipo"]);
            }
            return usuario;
        }
        #endregion

        #region guardarUsuario
        public void guardarUsuario(string username, string password, string user_nombre, string user_apellidos, int user_tipo)
        {
            username = username.ToLower();
            password = password.ToLower();
            password = encriptacion(password);

            var sql = new StringBuilder();
            sql.AppendLine("insert into usuarios(username,password,user_nombre,user_apellidos,user_tipo) values (@username,@password,@user_nombre,@user_apellidos,@user_tipo)");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue =  username
                },
                new NpgsqlParameter{
                    ParameterName ="password",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=password
                },
                 new NpgsqlParameter{
                    ParameterName ="user_nombre",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=user_nombre
                },
                 new NpgsqlParameter{
                    ParameterName ="user_apellidos",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=user_apellidos
                },
                 new NpgsqlParameter{
                    ParameterName ="user_tipo",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=user_tipo
                }
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion

        #region eliminarUsuario
        public void eliminarUsuario(string username)
        {
            username = username.ToLower();
            var sql = new StringBuilder();
            sql.Append("update usuarios set user_tipo='2' where username=@username");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username
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

        #region habilitarUsuario
        public void habilitarUsuario(string username,int tipo)
        {
            username = username.ToLower();
            var sql = new StringBuilder();
            sql.Append("update usuarios set user_tipo='"+tipo+"' where username=@username");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username
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

        #region cambiarContraseña
        public void cambiarContraseña(string username, string password)
        {
            username = username.ToLower();
            password = password.ToLower();
            password = encriptacion(password);
            var sql = new StringBuilder();
            sql.Append("update usuarios set password=@password where username=@username");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=username
                },
                new NpgsqlParameter{
                    ParameterName="password",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=password
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
        public bool UsernameRepetido(string username)
        {
            username = username.ToLower();
            bool result = false;
            List<Usuario> Usuarios = cargarUsuarios();
            for (int i = 0; i < Usuarios.Count; i++)
            {
                if (Usuarios[i].username.Equals(username))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        #endregion

        #region Login
        public bool Login(string username, string password)
        {
            bool loginOk = false;
            username = username.ToLower();
            password = password.ToLower();
            password = encriptacion(password);

            List<Usuario> Usuarios = cargarUsuarios();
            
            for (int i = 0; i < Usuarios.Count; i++)
            {
                if (Usuarios[i].username.Equals(username) &&
                   Usuarios[i].password.Equals(password))
                {
                    loginOk = true;
                    break;
                }
            }
            return loginOk;
        }
        #endregion

        public string encriptacion(string password)
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
            return password;
        }
    }
}