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
    public class ClientesPost
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

        #region cargarClientes
        public List<Cliente> cargarClientes()
        {
            var sql = new StringBuilder();
            sql.AppendLine("select * from clientes");
            List<Cliente> clientes = new List<Cliente>();
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString());
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                clientes.Add(new Cliente()
                {
                    cl_id = Convert.ToInt32(item["cl_id"]),
                    cl_username = item["cl_username"].ToString(),
                    cl_password = item["cl_password"].ToString(),
                    cl_nombre = item["cl_nombre"].ToString(),
                    cl_apellidos = item["cl_apellidos"].ToString(),
                });
            }
            return clientes;
        }
        #endregion

        #region cargarCliente
        public List<Cliente> cargarCliente(int cl_id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("select * from clientes where cl_id=@cl_id");
            List<Cliente> clientes = new List<Cliente>();
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="cl_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=cl_id
                },      
            };
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                clientes.Add(new Cliente()
                {
                    cl_id = Convert.ToInt32(item["cl_id"]),
                    cl_username = item["cl_username"].ToString(),
                    cl_password = item["cl_password"].ToString(),
                    cl_nombre = item["cl_nombre"].ToString(),
                    cl_apellidos = item["cl_apellidos"].ToString(),
                });
            }
            return clientes;
        }
        #endregion

        #region guardarClinte
        public void guardarClinte(string cl_username, string cl_password,string cl_nombre,string cl_apellidos)
        {
            cl_username = cl_username.ToLower();
            cl_password = cl_password.ToLower();
            cl_password = encriptacion(cl_password);

            var sql = new StringBuilder();
            sql.AppendLine("insert into clientes(cl_username,cl_password,cl_nombre,cl_apellidos) values (@cl_username,@cl_password,@cl_nombre,@cl_apellidos)");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="cl_username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue =  cl_username
                },
                new NpgsqlParameter{
                    ParameterName ="cl_password",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=cl_password
                },
                 new NpgsqlParameter{
                    ParameterName ="cl_nombre",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=cl_nombre
                },
                 new NpgsqlParameter{
                    ParameterName ="cl_apellidos",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=cl_apellidos
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

        #region eliminarClientes
        public void eliminarCliente(int cl_id)
        {
            var sql = new StringBuilder();
            sql.Append("delete from clientes where cl_id=@cl_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="cl_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=cl_id
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
        public void cambiarContraseña(string cl_username, string cl_password)
        {
            cl_username = cl_username.ToLower();
            cl_password = cl_password.ToLower();
            cl_password = encriptacion(cl_password);
            var sql = new StringBuilder();
            sql.Append("update clientes set cl_password=@cl_password where cl_username=@cl_username");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="cl_username",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=cl_username
                },
                new NpgsqlParameter{
                    ParameterName="cl_password",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue=cl_password
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
        public bool UsernameRepetido(string cl_username)
        {
            cl_username = cl_username.ToLower();
            bool result = false;
            List<Cliente> Clientes = cargarClientes();
            for (int i = 0; i < Clientes.Count; i++)
            {
                if (Clientes[i].cl_username.Equals(cl_username))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        #endregion

        #region LoginCliente
        public bool LoginCliente(string cl_username, string cl_password)
        {
            bool loginOk = false;
            cl_username = cl_username.ToLower();
            cl_password = cl_password.ToLower();
            cl_password = encriptacion(cl_password);

            List<Cliente> clientes = new List<Cliente>();
            for (int i = 0; i < clientes.Count; i++)
            {
                if (clientes[i].cl_username.Equals(cl_username) &&
                   clientes[i].cl_password.Equals(cl_password))
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