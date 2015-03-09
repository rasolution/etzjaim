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

        #region borrarUsuario
        [WebMethod]
        public string user_delete(string username)
        {
            string result = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            UsuarioPost post = new UsuarioPost();
            post.eliminarUsuario(username);
            if (post.IsError)
            {
                result = errorMessage + post.ErrorDescripcion;
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
            UsuarioPost post = new UsuarioPost();
            List<Usuario> usuarios = post.user_Ajax(user);
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
                result = errorMessage + post.ErrorDescripcion;
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
                result = errorMessage + post.ErrorDescripcion;
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
                result = errorMessage + post.ErrorDescripcion;
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

        #region SolicitarCita
        [WebMethod]
        public string cita_solicitarCita(string username, string cita_fecha,string cita_hora)
        {
            string result = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            CitasPost post = new CitasPost();
            if (post.verficiarCita_Fecha(cita_fecha,cita_hora))
            {
                result = "Ya hay una cita aprobada para dicha fecha";
            }
            else
            {
                post.guardarCita(username, cita_fecha,cita_hora);
                if (post.Is_error)
                {
                    result = errorMessage + post.Error_Descripcion;
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
            JavaScriptSerializer js = new JavaScriptSerializer();
            CitasPost post = new CitasPost();
            if (post.verificarCita(cita_id))
            {
                result = "La cita ya ha sido aprobada";
            }
            else
            {
                Cita_Usuario cita = new Cita_Usuario();
                cita = post.cargarCita(cita_id);
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
                    Conversaciones conversacion = convpost.verificarConversacion("service", cita.username);
                    if (conversacion.conv_id == 0)
                    {
                        convpost.crearConversacion("service", cita.username);
                    }
                    conversacion = convpost.verificarConversacion("service", cita.username);
                    MensajesPost mespost = new MensajesPost();
                    mespost.guardarMensaje(conversacion.conv_id, message, "service");
                    convpost.estadoNoLeido(cita.username,conversacion.conv_id);
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
            JavaScriptSerializer js = new JavaScriptSerializer();
            CitasPost post = new CitasPost();
            Cita_Usuario cita = new Cita_Usuario();
            cita = post.cargarCita(cita_id);
            post.borrarCita(cita_id);
            if (post.Is_error)
            {
                result = "A ocurrido un error " + post.Error_Descripcion;
            }
            else
            {
                string message = "Querido cliente le informamos que su cita ha sido rechazada, si desea solicitar otra peude hacerlo";
                result = "Se ha borrado la cita se le informara al cliente";
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

            response.response = result;
            return js.Serialize(response);

        }
        
        #endregion

        #region cargarCitas
        [WebMethod]
        public string cita_cargarCitas()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            CitasPost post = new CitasPost();
            List<Cita_Usuario> citas = post.cargarCitas();
            return js.Serialize(citas);
        }
        #endregion

        #region cargarCita
        [WebMethod]
        public string cita_cargarCita(int cita_id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            CitasPost post = new CitasPost();
            Cita_Usuario cita = post.cargarCita(cita_id);
            return js.Serialize(cita);
        }
        #endregion

        #region cargarCitasAprobadas
        [WebMethod]
        public string cita_cargarCitasAprobadas()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            CitasPost post = new CitasPost();
            List<Cita_Usuario> citas = post.cargarCitasAprobadas();
            return js.Serialize(citas);
        }
        #endregion



        #region cargarCitasPendientes
        [WebMethod]
        public string cita_cargarCitasPendientes()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            CitasPost post = new CitasPost();
            List<Cita_Usuario> citas = post.cargarCitasPendientes();
            return js.Serialize(citas);
        }
        #endregion
        #endregion

        #region MetodosConversaciones

        #region CargarConversacionesUsuario
        [WebMethod]
        public string conv_CargarConversacionesUsuario(string username)
        {
            ConversacionesPost post = new ConversacionesPost();
            List<Conversaciones> conversaciones =post.cargarConversacionesUsuario(username);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(conversaciones);
        }
        #endregion

        #region crearConversacion
        [WebMethod]
        public string conv_crearConversacion(string username1, string username2,string message)
        {        
            ConversacionesPost post = new ConversacionesPost();
            MensajesPost messagepost = new MensajesPost();
            Conversaciones conversacion = post.verificarConversacion(username1, username2);
            if (conversacion.conv_id == 0)
            {
                post.crearConversacion(username1, username2);
            }
            conversacion = post.verificarConversacion(username1, username2);
            messagepost.guardarMensaje(conversacion.conv_id, message, username1);
            post.estadoNoLeido(username2, conversacion.conv_id);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(conversacion);
        }
        #endregion

        #region verificarConversacion
        [WebMethod]
        public string conv_verificarConversacion(string username1,string username2)
        {
            ConversacionesPost post = new ConversacionesPost();
            Conversaciones conversacion = post.verificarConversacion(username1, username2);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(conversacion);
        }
        #endregion

        #region CargarConversacion
        [WebMethod]
        public string conv_CargarConversacion(int conv_id)
        {
            ConversacionesPost post = new ConversacionesPost();
            Conversaciones conversacion = post.cargarConversacion(conv_id);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(conversacion);
        }
        #endregion

        #region agregarSpam
        [WebMethod]
        public string conv_agregarSpam(string username,int conv_id)
        {
            string result = "";
            ConversacionesPost post = new ConversacionesPost();
            post.agregarSpam(username, conv_id);
            if (post.IsError)
            {
                result = errorMessage + post.ErrorDescripcion;
            }
            else
            {
                result = "Se ha movido a spam con exito";
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            response.response = result;
            return js.Serialize(response);
        }
        #endregion

        #region quitarSpam
        [WebMethod]
        public string conv_quitarSpam(string username, int conv_id)
        {
            string result = "";
            ConversacionesPost post = new ConversacionesPost();
            post.quitarSpam(username, conv_id);
            if (post.IsError)
            {
                result = errorMessage + post.ErrorDescripcion;
            }
            else
            {
                result = "Se ha quitado de spam con exito";
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            response.response = result;
            return js.Serialize(response);
        }
        #endregion

        #region CargarConversacionesUsuario
        [WebMethod]
        public string conv_cargarConversacionesSpamUsuario(string username)
        {
            ConversacionesPost post = new ConversacionesPost();
            List<Conversaciones> conversaciones = post.cargarConversacionesSpamUsuario(username);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(conversaciones);
        }
        #endregion

        #region cargarConversacionesLeidas
        [WebMethod]
        public string conv_cargarConversacionesLeidas(string username)
        {
            ConversacionesPost post = new ConversacionesPost();
            List<Conversaciones> conversaciones = post.cargarConversacionesLeidas(username);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(conversaciones);
        }
        #endregion

        #region cargarConversacionesNoLeidas
        [WebMethod]
        public string conv_cargarConversacionesNoLeidas(string username)
        {
            ConversacionesPost post = new ConversacionesPost();
            List<Conversaciones> conversaciones = post.cargarConversacionesNoLeidas(username);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(conversaciones);
        }
        #endregion

        #endregion

        #region metodosMensajes
        #region cargarMensajes
        [WebMethod]
        public string mes_cargarMensajes(int conv_id)
        {          
            JavaScriptSerializer js = new JavaScriptSerializer();
            MensajesPost post = new MensajesPost();
            List<Conv_Message> messages = post.cargarMensajes(conv_id);
            return js.Serialize(messages);
        }
        #endregion

        #region enviarMensaje
        [WebMethod]
        public string mes_enviarmensaje(int conv_id,string message,string username)
        {
            MensajesPost post = new MensajesPost();
            
            post.guardarMensaje(conv_id, message, username);
            return "enviado";
        }

        #endregion
        #endregion

    }
}