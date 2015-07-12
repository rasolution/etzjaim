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
        Respuesta response = new Respuesta();
        JavaScriptSerializer js = new JavaScriptSerializer();
        UsuarioPost usuariopostgress = new UsuarioPost();
        ProductosPost Productopost = new ProductosPost();
        CitasPost Citaspost = new CitasPost();
        ConversacionesPost Conversacionespost = new ConversacionesPost();
        MensajesPost Messagepost = new MensajesPost();
        string errorMessage = "A ocurrido un error ";

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

            if (usuariopostgress.Login(username, password) == true)
            {
                userresponse.response = "yes";
                userresponse.user = usuariopostgress.cargarUsuario(username);
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
            if (usuariopostgress.UsernameRepetido(username))
            {
                result = "El nombre de usuario ya esta en uso";
            }
            else
            {
                usuariopostgress.guardarUsuario(username, password, nombre, apellidos, tipo);
                if (usuariopostgress.IsError)
                {
                    result = errorMessage + usuariopostgress.ErrorDescripcion;
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
            usuariopostgress.cambiarContraseña(username, password);
            if (usuariopostgress.IsError)
            {
                result = errorMessage + usuariopostgress.ErrorDescripcion;
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
            List<Usuario> usuarios = usuariopostgress.cargarAdmins_Clientes(tipo);
            return js.Serialize(usuarios);
        }
        #endregion

        #region cargarUsuario
        [WebMethod]
        public string user_CargarUsuario(string username)
        {
            Usuario usuario = usuariopostgress.cargarUsuario(username);
            return js.Serialize(usuario);
        }
        #endregion

        #region cargarUsuarios
        [WebMethod]
        public string user_CargarUsuariosTodos()
        {
            List<Usuario> usuarios = usuariopostgress.cargarUsuarios();
            return js.Serialize(usuarios);
        }
        #endregion

        #region borrarUsuario
        [WebMethod]
        public string user_delete(string username)
        {
            string result = "";
            usuariopostgress.eliminarUsuario(username);
            if (usuariopostgress.IsError)
            {
                result = errorMessage + usuariopostgress.ErrorDescripcion;
            }
            else
            {
                result = "Eliminado con exito";
            }
            response.response = result;
            return js.Serialize(response);
        }
        #endregion

        #region user_Ajax
        [WebMethod]
        public string user_Ajax(string user)
        {
            List<Usuario> usuarios = usuariopostgress.user_Ajax(user);
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

            Productopost.guardarProducto(pro_nombre, pro_precio, pro_estado, pro_foto);
            if (Productopost.IsError)
            {
                result = errorMessage + Productopost.ErrorDescripcion;
            }
            else
            {
                result = "Se ha guardado con exito";
            }
            response.response = result;
            return js.Serialize(response);
        }
        #endregion

        #region eliminarProducto
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string product_eliminarCliente(int pro_id)
        {
            string result;
            Productopost.eliminarProducto(pro_id);
            if (Productopost.IsError)
            {
                result = errorMessage + Productopost.ErrorDescripcion;
            }
            else
            {
                result = "Se ha borrado con exito";
            }
            response.response = result;
            return js.Serialize(response);
        }
        #endregion

        #region editarProducto
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string product_editarProducto(int pro_id, string pro_nombre, int pro_precio, int pro_estado, byte[] pro_foto)
        {
            string result = "";
            Productopost.editarProducto(pro_id, pro_nombre, pro_precio, pro_estado, pro_foto);
            if (Productopost.IsError)
            {
                result = errorMessage + Productopost.ErrorDescripcion;
            }
            else
            {
                result = "Edición correcta";
            }
            response.response = result;
            return js.Serialize(response);
        }
        #endregion

        #region cargarProductos
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string product_cargarProductos()
        {
            List<Producto> Productos = Productopost.cargarProductos();
            return js.Serialize(Productos);
        }
        #endregion

        #region cargarProducto
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string product_cargarProducto(int pro_id)
        { 
            Producto Producto = Productopost.cargarProducto(pro_id);
            return js.Serialize(Producto);
        }
        #endregion

        #endregion

        #region MetodosCitas

        #region SolicitarCita
        [WebMethod]
        public string cita_solicitarCita(string username, string cita_fecha, string cita_hora)
        {
            string result = "";

            if (Citaspost.verficiarCita_Fecha(cita_fecha, cita_hora))
            {
                result = "Ya hay una cita aprobada para dicha fecha";
            }
            else
            {
                Citaspost.guardarCita(username, cita_fecha, cita_hora);
                if (Citaspost.Is_error)
                {
                    result = errorMessage + Citaspost.Error_Descripcion;
                }
                else
                {
                    result = "La cita se ha solicitado con exito se le informara si su cita ha sido aprobada o rechazada";
                }
            }
            response.response = result;
            return js.Serialize(response);
        }
        #endregion

        #region aprobarCita
        [WebMethod]
        public string cita_aprobarCita(int cita_id)
        {
            string result = "";
            if (Citaspost.verificarCita(cita_id))
            {
                result = "La cita ya ha sido aprobada";
            }
            else
            {
                Cita_Usuario cita = new Cita_Usuario();
                cita = Citaspost.cargarCita(cita_id);
                Citaspost.aprobarCita(cita_id);
                if (Citaspost.Is_error)
                {
                    result = "A ocurrido un error " + Citaspost.Error_Descripcion;
                }
                else
                {
                    string message = "Querido cliente le informamos que su cita ha sido aprobada";
                    result = "Se ha aprovado la cita se le informara al cliente";
                    ConversacionesPost convpost = new ConversacionesPost();
                    Conversaciones conversacion = convpost.verificarConversacion("service", cita.username);
                    if (conversacion.conv_id == 0)
                    {
                        convpost.crearConversacion("service", cita.username);
                    }
                    conversacion = convpost.verificarConversacion("service", cita.username);
                    MensajesPost mespost = new MensajesPost();
                    mespost.guardarMensaje(conversacion.conv_id, message, "service");
                    convpost.estadoNoLeido(cita.username, conversacion.conv_id);
                }
            }
            response.response = result;
            return js.Serialize(response);
        }

        #endregion

        #region rechazarCita
        [WebMethod]
        public string cita_rechazarCita(int cita_id)
        {
            string result = "";
            Cita_Usuario cita = new Cita_Usuario();
            cita = Citaspost.cargarCita(cita_id);
            Citaspost.borrarCita(cita_id);
            if (Citaspost.Is_error)
            {
                result = "A ocurrido un error " + Citaspost.Error_Descripcion;
            }
            else
            {
                string message = "Querido cliente le informamos que su cita ha sido rechazada, si desea solicitar otra peude hacerlo";
                result = "Se ha borrado la cita se le informara al cliente";
                Conversaciones conversacion = Conversacionespost.verificarConversacion("service", cita.username);
                if (conversacion.conv_id == 0)
                {
                    Conversacionespost.crearConversacion("service", cita.username);
                }
                conversacion = Conversacionespost.verificarConversacion("service", cita.username);
                Messagepost.guardarMensaje(conversacion.conv_id, message, "service");
                Conversacionespost.estadoNoLeido(cita.username, conversacion.conv_id);
            }
            response.response = result;
            return js.Serialize(response);
        }

        #endregion

        #region cargarCitas
        [WebMethod]
        public string cita_cargarCitas()
        {
            List<Cita_Usuario> citas = Citaspost.cargarCitas();
            return js.Serialize(citas);
        }
        #endregion

        #region cargarCita
        [WebMethod]
        public string cita_cargarCita(int cita_id)
        {
            Cita_Usuario cita = Citaspost.cargarCita(cita_id);
            return js.Serialize(cita);
        }
        #endregion

        #region cargarCitasAprobadas
        [WebMethod]
        public string cita_cargarCitasAprobadas()
        {
            List<Cita_Usuario> citas = Citaspost.cargarCitasAprobadas();
            return js.Serialize(citas);
        }
        #endregion

        #region cargarCitasPendientes
        [WebMethod]
        public string cita_cargarCitasPendientes()
        {
            List<Cita_Usuario> citas = Citaspost.cargarCitasPendientes();
            return js.Serialize(citas);
        }
        #endregion
        #endregion

        #region MetodosConversaciones

        #region CargarConversacionesUsuario
        [WebMethod]
        public string conv_CargarConversacionesUsuario(string username)
        {   
            List<Conversaciones> conversaciones = Conversacionespost.cargarConversacionesUsuario(username);         
            return js.Serialize(conversaciones);
        }
        #endregion

        #region crearConversacion
        [WebMethod]
        public string conv_crearConversacion(string username1, string username2, string message)
        {      
            Conversaciones conversacion = Conversacionespost.verificarConversacion(username1, username2);
            if (conversacion.conv_id == 0)
            {
                Conversacionespost.crearConversacion(username1, username2);
            }
            conversacion = Conversacionespost.verificarConversacion(username1, username2);
            Messagepost.guardarMensaje(conversacion.conv_id, message, username1);
            Conversacionespost.estadoNoLeido(username2, conversacion.conv_id);          
            return js.Serialize(conversacion);
        }
        #endregion

        #region verificarConversacion
        [WebMethod]
        public string conv_verificarConversacion(string username1, string username2)
        {          
            Conversaciones conversacion = Conversacionespost.verificarConversacion(username1, username2);
            return js.Serialize(conversacion);
        }
        #endregion

        #region CargarConversacion
        [WebMethod]
        public string conv_CargarConversacion(int conv_id)
        {         
            Conversaciones conversacion = Conversacionespost.cargarConversacion(conv_id);
            return js.Serialize(conversacion);
        }
        #endregion

        #region agregarSpam
        [WebMethod]
        public string conv_agregarSpam(string username, int conv_id)
        {
            string result = "";
            Conversacionespost.agregarSpam(username, conv_id);
            if (Conversacionespost.IsError)
            {
                result = errorMessage + Conversacionespost.ErrorDescripcion;
            }
            else
            {
                result = "Se ha movido a spam con exito";
            }
            response.response = result;
            return js.Serialize(response);
        }
        #endregion

        #region quitarSpam
        [WebMethod]
        public string conv_quitarSpam(string username, int conv_id)
        {
            string result = "";
            Conversacionespost.quitarSpam(username, conv_id);
            if (Conversacionespost.IsError)
            {
                result = errorMessage + Conversacionespost.ErrorDescripcion;
            }
            else
            {
                result = "Se ha quitado de spam con exito";
            }
            response.response = result;
            return js.Serialize(response);
        }
        #endregion

        #region CargarConversacionesUsuario
        [WebMethod]
        public string conv_cargarConversacionesSpamUsuario(string username)
        {
            List<Conversaciones> conversaciones = Conversacionespost.cargarConversacionesSpamUsuario(username);
            return js.Serialize(conversaciones);
        }
        #endregion

        #region cargarConversacionesLeidas
        [WebMethod]
        public string conv_cargarConversacionesLeidas(string username)
        {
            List<Conversaciones> conversaciones = Conversacionespost.cargarConversacionesLeidas(username);
            return js.Serialize(conversaciones);
        }
        #endregion

        #region cargarConversacionesNoLeidas
        [WebMethod]
        public string conv_cargarConversacionesNoLeidas(string username)
        {
            List<Conversaciones> conversaciones = Conversacionespost.cargarConversacionesNoLeidas(username);
            return js.Serialize(conversaciones);
        }
        #endregion

        #endregion

        #region metodosMensajes
        #region cargarMensajes
        [WebMethod]
        public string mes_cargarMensajes(int conv_id)
        { 
            List<Conv_Message> messages = Messagepost.cargarMensajes(conv_id);
            return js.Serialize(messages);
        }
        #endregion

        #region enviarMensaje
        [WebMethod]
        public string mes_enviarmensaje(int conv_id, string message, string username)
        {
            Messagepost.guardarMensaje(conv_id, message, username);
            return "enviado";
        }

        #endregion
        #endregion

    }
}