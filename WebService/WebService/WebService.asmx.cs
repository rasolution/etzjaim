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
        Respuesta response = new Respuesta();

        #region testearConexion
        [WebMethod]
        public string testearConexion()
        {
            string result = "postgresql: " + postgres.accesodatos.Estado() + " sqlserver: " + sqlserver.Accesar.Estado();

            return result;

        }
        #endregion

        #region Metodos Administradores
        #region guardarAdmin
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string admin_Guardar(string username, string password)
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
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region eliminarAdmin
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string admin_Eliminar(int admin_id)
        {
            string result = "";
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
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region cargarAdmins
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string admin_CargarTodos()
        {
            AdministradoresPost post = new AdministradoresPost();
            List<Administrador> admins = new List<Administrador>();
            admins = post.cargarAdministrativos();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(admins);
        }
        #endregion

        #region cargarAdmin
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string admin_Cargar(int admin_id)
        {
            AdministradoresPost post = new AdministradoresPost();
            Administrador admin = new Administrador();
            admin = post.cargarAdmin(admin_id);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(admin);
        }
        #endregion

        #region cambiarContraseña
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string admin_cambiarContraseña(string username, string password)
        {
            string result = "";
            AdministradoresPost post = new AdministradoresPost();
            post.cambiarContraseña(username, password);
            if (post.IsError)
            {
                result = "A ocurrido un error " + post.ErrorDescripcion;
            }
            else
            {
                result = "Contraseña cambiada";
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region LoginAdmin
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string admin_Login(string admin_username, string admin_password)
        {
            string result = "no";
            AdministradoresPost post = new AdministradoresPost();
            if (post.LoginAdmin(admin_username, admin_password))
            {
                result = "yes";
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion
        #endregion

        #region Metodos Clientes

        #region guardarCliente
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cliente_Guardar(string cl_username, string cl_password, string cl_nombre, string cl_apellidos)
        {
            ClientesPost post = new ClientesPost();
            string result;
            if (post.UsernameRepetido(cl_username))
            {
                result = "Ese nombre de usuario ya esta reservado para algun cliente";
            }
            else
            {
                post.guardarClinte(cl_username, cl_password, cl_nombre, cl_apellidos);
                if (post.IsError)
                {
                    result = "A ocurrido un error " + post.ErrorDescripcion;
                }
                else
                {
                    result = "Se ha guardado con exito";
                }
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region eliminarCliente
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cliente_Elminar(int cl_id)
        {
            ClientesPost post = new ClientesPost();
            string result = "";
            post.eliminarCliente(cl_id);
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

        #region cargarClientes
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cliente_CargarTodos()
        {
            ClientesPost post = new ClientesPost();
            List<Cliente> clientes = post.cargarClientes();
            JavaScriptSerializer js = new JavaScriptSerializer();

            return js.Serialize(clientes);
        }
        #endregion

        #region cargarCliente
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cliente_Cargar(int cl_id)
        {
            ClientesPost post = new ClientesPost();
            Cliente cliente = post.cargarCliente(cl_id);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(cliente);
        }

        #endregion

        #region cambiarContraseñaCliente
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cliente_cambiarContraseña(string cl_username, string cl_password)
        {
            ClientesPost post = new ClientesPost();
            string result = "";
            post.cambiarContraseña(cl_username, cl_password);
            if (post.IsError)
            {
                result = "A ocurrido un error " + post.ErrorDescripcion;
            }
            else
            {
                result = "Contraseña cambiada con exito";
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region LoginCliente
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cliente_Login(string cl_username, string cl_password)
        {
            ClientesPost post = new ClientesPost();
            string result = "no";
            if (post.LoginCliente(cl_username, cl_password))
            {
                result = "yes";
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);

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

        #region MetodosCitas

        #region solicitarCita
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cita_solicitarCita(int cl_id, DateTime cita_fecha)
        {
            string result = "";
            CitasPost post = new CitasPost();
            if (post.verficiarCita_Fecha(cita_fecha))
            {
                result = "Ya hay una fecha aprovada para esa fecha y hora";
            }
            else
            {
                post.guardarCita(cl_id, cita_fecha);
                if (post.Is_error)
                {
                    result = "A ocurrido un error " + post.Error_Descripcion;
                }
                else
                {
                    result = "La cita ha sido pedida con exito, se le verificara si se aprobara";
                }
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region aprobarCita
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cita_aprobarCita(int cita_id, int cl_id)
        {

            string result = "";
            CitasPost post = new CitasPost();
            if (post.verificarCita(cita_id))
            {
                result = "Ya se ha aprobado una cita para esa fecha y hora";
            }
            else
            {
                post.aprobarCita(cita_id);
                if (post.Is_error)
                {
                    result = "A ocurrido un error " + post.Error_Descripcion;
                }
                else
                {
                    string message = "Querido cliente le informamos que su cita ha sido aprobada";
                    result = "Se ha aprovado la cita se le informara al cliente";
                    ConversacionesPost convpost = new ConversacionesPost();
                    Conversacion conversacion = convpost.verificarConversacion(1, cl_id);
                    if (conversacion.conv_id == 0)
                    {
                        convpost.crearConversacion(cl_id, 1);
                    }
                    conversacion = convpost.verificarConversacion(1, cl_id);
                    MensajesPost mespost = new MensajesPost();
                    mespost.guardarMensaje(conversacion.conv_id, message);
                }
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region rechazarCita
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cita_recharCita(int cita_id, int cl_id)
        {
            string result = "";
            CitasPost post = new CitasPost();
            post.borrarCita(cita_id);
            if (post.Is_error)
            {
                result = "A ocurrido un error " + post.Error_Descripcion;
            }
            else
            {
                string message = "Querido cliente le informamos que su cita ha sido rechadaza, si desea pida otra cita";
                result = "Se ha borrado la cita se le informara al cliente";
                ConversacionesPost convpost = new ConversacionesPost();
                Conversacion conversacion = convpost.verificarConversacion(1, cl_id);
                if (conversacion.conv_id == 0)
                {
                    convpost.crearConversacion(cl_id, 1);
                }
                conversacion = convpost.verificarConversacion(1, cl_id);
                MensajesPost mespost = new MensajesPost();
                mespost.guardarMensaje(conversacion.conv_id, message);
            }
            response.response = result;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(response);
        }
        #endregion

        #region cargarCita
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cita_cargarCita(int cita_id)
        {
            CitasPost post = new CitasPost();
            Cita_cliente cita = new Cita_cliente();
            cita = post.cargarCita(cita_id);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(cita);
        }
        #endregion

        #region cargarCitas
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cita_cargarCitas()
        {
            CitasPost post = new CitasPost();
            List<Cita_cliente> cita = new List<Cita_cliente>();
            cita = post.cargarCitas();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(cita);
        }
        #endregion

        #region cargarCitasAprobadas
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cita_cargarCitasAprobadas()
        {
            CitasPost post = new CitasPost();
            List<Cita_cliente> cita = new List<Cita_cliente>();
            cita = post.cargarCitasAprobadas();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(cita);
        }
        #endregion

        #region cargarCitasNoAprobadas
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string cita_cargarCitasNoAprobadas()
        {
            CitasPost post = new CitasPost();
            List<Cita_cliente> cita = new List<Cita_cliente>();
            cita = post.cargarCitasNoAprobadas();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(cita);
        }
        #endregion

        #endregion
    }
}