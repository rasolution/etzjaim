using EtzJaimWebPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EtzJaimWebPage.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios

        #region index
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0)
                {
                    var service = new WebService.WebServiceSoapClient();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var datos = js.Deserialize<List<Usuario>>(service.user_CargarUsuariosTodos());
                    return View(datos);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }
        #endregion

        #region indexpull
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int tipo)
        {
            if (ModelState.IsValid)
            {
                if (tipo == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var service = new WebService.WebServiceSoapClient();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var datos = js.Deserialize<List<Usuario>>(service.user_CargarUsuarios(tipo));
                    return View(datos);
                }
            }
            return View();
        }
        #endregion

        #region createView
        public ActionResult Create()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        #endregion

        #region ChangepaswwordView

        public ActionResult Password(string username)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0)
                {
                    var service = new WebService.WebServiceSoapClient();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var datos = js.Deserialize<Usuario>(service.user_CargarUsuario(username));
                    Usuario usuario = new Usuario();
                    usuario.username = datos.username;
                    usuario.nombre = datos.nombre;
                    usuario.apellidos = datos.apellidos;
                    usuario.tipo = datos.tipo;
                    return View(user);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        #endregion

        #region CreateFunciont
        [HttpPost, ActionName("Password")]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordConfirmed(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var service = new WebService.WebServiceSoapClient();
                JavaScriptSerializer js = new JavaScriptSerializer();
                var datos = js.Deserialize<Respuesta>(service.user_CambiarContraseña(username, password));
                ViewBag.Message = datos.response;
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region CreateFunciont
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,password,nombre,apellidos,tipo")] Usuario user)
        {
            if (ModelState.IsValid)
            {
                var service = new WebService.WebServiceSoapClient();
                JavaScriptSerializer js = new JavaScriptSerializer();
                var datos = js.Deserialize<Respuesta>(service.user_Save(user.username, user.password, user.nombre, user.apellidos, user.tipo));
                ViewBag.Message = datos.response;

            }
            return View();
        }
        #endregion

        #region user
        public ActionResult User(string username)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0)
                {
                    var service = new WebService.WebServiceSoapClient();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var datos = js.Deserialize<Usuario>(service.user_CargarUsuario(username));
                    Usuario usuario = new Usuario();
                    usuario.username = datos.username;
                    usuario.nombre = datos.nombre;
                    usuario.apellidos = datos.apellidos;
                    usuario.tipo = datos.tipo;
                    return View(user);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        #endregion

        #region borrar
        public ActionResult Borrar(string username)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0)
                {
                    var service = new WebService.WebServiceSoapClient();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var datos = js.Deserialize<Usuario>(service.user_CargarUsuario(username));
                    Usuario usuario = new Usuario();
                    usuario.username = datos.username;
                    usuario.nombre = datos.nombre;
                    usuario.apellidos = datos.apellidos;
                    usuario.tipo = datos.tipo;
                    return View(user);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }


        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult BorradoConfirmed(string username)
        {
            if (ModelState.IsValid)
            {
                var service = new WebService.WebServiceSoapClient();
                JavaScriptSerializer js = new JavaScriptSerializer();
                var datos = js.Deserialize<Respuesta>(service.user_delete(username));
                ViewBag.Message = datos.response;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Atras()
        {
            return RedirectToAction("Index");
        }

        #endregion

        [HttpPost]
        public JsonResult Ajax(string user)
        {
            var service = new WebService.WebServiceSoapClient();
            return Json(service.user_Ajax(user));
        }
    }
}