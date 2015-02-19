using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using WebService.Conexion.PostGresSQL;
using WebService.Conexion.SQLServer;
using WebService.Estructuras;
using WebService.Metodos.PostSQL.PostDatabase;
using WebService.Metodos.SQLServer;

namespace WebService
{
    /// <summary>
    /// Descripción breve de WebService
    /// </summary>
    
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        //Inicio de conexiones a base de datos 
        AccesoDatosPost postgres = new AccesoDatosPost();
        AccesoDatosSQL sqlserver = new AccesoDatosSQL();
      
        #region Metodos Administradores

        #region testearConexion
        [WebMethod]
        public string testearConexion()
        {
            string result = "postgresql: " + postgres.accesodatos.Estado() + " sqlserver: " + sqlserver.Accesar.Estado();

            return result;
            
        }
        #endregion

        #region guardarAdmin
        [WebMethod]
        public string guardarAdmin(string username, string password)
        {
            AdministradoresPost post = new AdministradoresPost();
            string result = "";
            if (post.UsernameRepetido(username))
            {
                result = "Usuario repetido";
            }
            else
            {
                post.guardarAdministrativo(username, password);
                if (post.IsError)
                {
                    result = "A ocurrido un error " + post.ErrorDescripcion;
                }
                else
                {
                    result = "Guardardo con exito";
                }
            }
            return result;
        }
        #endregion

        #region eliminarAdmin
        [WebMethod]
        public string eliminarAdmin(int admin_id)
        {
            string result="";
            AdministradoresPost post = new AdministradoresPost();
            post.eliminarAdministrador(admin_id);
            if (post.IsError)
            {
                result = "A ocurrido un error " + post.ErrorDescripcion;
            }
            else
            {
                result = "Guardado con exito";
            }
            return result;
        }
        #endregion

        #region cargarAdmins
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void cargarAdmins()
        {
            AdministradoresPost post = new AdministradoresPost();
            List<Administrador> admins = new List<Administrador>();
            admins = post.cargarAdministrativos();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(js.Serialize(admins));
        }
        #endregion

        #region cargarAdmin
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void cargarAdmin(int admin_id)
        {
            AdministradoresPost post = new AdministradoresPost();
            List<Administrador> admin = new List<Administrador>();
            admin = post.cargarAdmin(admin_id);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(js.Serialize(admin));
        }
        #endregion 

        #region cambiarContraseña
        [WebMethod]
        public string cambiarContraseña(string username, string password)
        {
            string result="";
            AdministradoresPost post = new AdministradoresPost();
            post.cambiarContraseña(username, password);
            if (post.IsError)
            {
                result = "A ocurrido un error "+post.ErrorDescripcion;
            }
            else
            {
                result = "Contraseña cambiada";
            }
            return result;
        }
        #endregion

        #region LoginAdmin
        [WebMethod]
        public bool LoginAdmin(string admin_username, string admin_password)
        {
            bool result = false;
            AdministradoresPost post = new AdministradoresPost();
            if(post.LoginAdmin(admin_username,admin_password))
            {
                result = true;
            }
            return result;
        }
        #endregion
        #endregion

        #region Metodos Clientes


        #endregion

    }
}
