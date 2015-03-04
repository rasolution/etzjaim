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
using WebService.Estructuras.Conversaciones;
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
        Respuesta response = new Respuesta();
        string errorMessage = "A ocurrido un error";
        #region testearConexion
        [WebMethod]
        public bool testearConexion()
        {
            bool result = false;
            if (postgres.accesodatos.Estado())
            {
                result = true;
            }
            return result;

        }
        #endregion

        #region MetodosUsuarios

        #region Login
        [WebMethod]
        public string user_Login(string username, string password)
        {
            Respuesta_Usuario userresponse = new Respuesta_Usuario();
            JavaScriptSerializer js = new JavaScriptSerializer();
            UsuarioPost post = new UsuarioPost();
            if (post.Login(username, password) == true)
            {
                userresponse.response = "yes";
                userresponse.user = post.cargarUsuario(username);
            }
            else
            {
                userresponse.response = "no";
            }

            return js.Serialize(userresponse);
        }
        #endregion

        #region guardarUsuario
        [WebMethod]
        public string user_Save(string username, string password, string nombre, string apellidos, int tipo)
        {
            string result = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            UsuarioPost post = new UsuarioPost();
            if (post.UsernameRepetido(username))
            {
                result = "El nombre de usuario ya esta en uso";
            }
            else
            {
                post.guardarUsuario(username, password, nombre, apellidos, tipo);
                if (post.IsError)
                {
                    result = errorMessage+ post.ErrorDescripcion;
                }
                else
                {
                    result = "Guardado con exito";
                }
            }        
            response.response = result;
            return js.Serialize(response);
        }
        #endregion

        #region cambiarContraseña
        [WebMethod]
        public string user_CambiarContraseña(string username, string password)
        {
            string result = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            UsuarioPost post = new UsuarioPost();
            post.cambiarContraseña(username, password);
            if (post.IsError)
            {
                result = errorMessage + post.ErrorDescripcion;
            }
            else
            {
                result = "Contraseña cambiada con exito";
            }
            return result;
        }
        #endregion

        #region cargarUsuarios
        [WebMethod]
        public string user_CargarUsuarios(int tipo)
        {
            UsuarioPost post=new UsuarioPost();
            List<Usuario> usuarios = post.cargarAdmins_Clientes(tipo);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(usuarios);
        }
        #endregion

        #region cargarUsuario
        [WebMethod]
        public string user_CargarUsuario(string username)
        {
            UsuarioPost post = new UsuarioPost();
            Usuario usuario= post.cargarUsuario(username);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(usuario);
        }
        #endregion

        #region cargarUsuarios
        [WebMethod]
        public string user_CargarUsuariosTodos()
        {
            UsuarioPost post = new UsuarioPost();
            List<Usuario> usuarios = post.cargarUsuarios();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(usuarios);
        }
        #endregion

        #endregion

        #region MetodosProductos

        #region guardarProducto
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string product_guardarProducto(string pro_nombre, int pro_precio, int pro_estado, byte[] pro_foto)
        {
            string result = "";
            ProductosPost post = new ProductosPost();
            post.guardarProducto(pro_nombre, pro_precio, pro_estado, pro_foto);
            if (post.IsError)
            {
                result = "A ocurrido un error " + post.ErrorDescripcion;
            }
            else
            {
                result = "Se ha guardado con exito";
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region eliminarProducto
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string product_eliminarCliente(int pro_id)
        {
            string result;
            ProductosPost post = new ProductosPost();
            post.eliminarProducto(pro_id);
            if (post.IsError)
            {
                result = "A ocurrido un error " + post.ErrorDescripcion;
            }
            else
            {
                result = "Se ha borrado con exito";
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region editarProducto
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string product_editarProducto(int pro_id, string pro_nombre, int pro_precio, int pro_estado, byte[] pro_foto)
        {
            string result = "";
            ProductosPost post = new ProductosPost();
            post.editarProducto(pro_id, pro_nombre, pro_precio, pro_estado, pro_foto);
            if (post.IsError)
            {
                result = "A ocurrido un error " + post.ErrorDescripcion;
            }
            else
            {
                result = "Edición correcta";
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region cargarProductos
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string product_cargarProductos()
        {
            ProductosPost post = new ProductosPost();
            List<Producto> Productos = post.cargarProductos();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(Productos);
        }
        #endregion

        #region cargarProducto
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string product_cargarProducto(int pro_id)
        {
            ProductosPost post = new ProductosPost();
            Producto Producto = post.cargarProducto(pro_id);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(Producto);
        }
        #endregion

        #endregion

        [WebMethod]
        public string CargarConversacionesUsuario(string username)
        {
            ConversacionesPost post = new ConversacionesPost();
            List<Conversaciones> conversaciones =post.cargarConversacionesUsuario(username);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(conversaciones);
        }
       


    }
}