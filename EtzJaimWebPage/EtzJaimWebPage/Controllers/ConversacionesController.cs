using EtzJaimWebPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EtzJaimWebPage.Controllers
{
    public class ConversacionesController : Controller
    {
        // GET: Conversaciones
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0 || user.tipo == 1)
                {
                    var service = new WebService.WebServiceSoapClient();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var datos = js.Deserialize<List<Conversaciones>>(service.conv_CargarConversacionesUsuario(user.username));
                    List<Conversaciones> finalList = new List<Conversaciones>();
                    foreach (var item in datos)
                    {
                        Conversaciones conv = item;
                        if (item.username2 == user.username)
                        {
                            var aux = item.username1;
                            conv.username1 = conv.username2;
                            conv.username2 = aux;
                        }
                        finalList.Add(conv);
                    }
                    return View(finalList);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }


        public ActionResult Leidas()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0 || user.tipo == 1)
                {
                    var service = new WebService.WebServiceSoapClient();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var datos = js.Deserialize<List<Conversaciones>>(service.conv_cargarConversacionesLeidas(user.username));
                    List<Conversaciones> finalList = new List<Conversaciones>();
                    foreach (var item in datos)
                    {
                        Conversaciones conv = item;
                        if (item.username2 == user.username)
                        {
                            var aux = item.username1;
                            conv.username1 = conv.username2;
                            conv.username2 = aux;
                        }
                        finalList.Add(conv);
                    }
                    return View(finalList);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }

        public ActionResult NoLeidas()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0 || user.tipo == 1)
                {
                    var service = new WebService.WebServiceSoapClient();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var datos = js.Deserialize<List<Conversaciones>>(service.conv_cargarConversacionesNoLeidas(user.username));
                    List<Conversaciones> finalList = new List<Conversaciones>();
                    foreach (var item in datos)
                    {
                        Conversaciones conv = item;
                        if (item.username2 == user.username)
                        {
                            var aux = item.username1;
                            conv.username1 = conv.username2;
                            conv.username2 = aux;
                        }
                        finalList.Add(conv);
                    }
                    return View(finalList);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }


        public ActionResult Spam()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0 || user.tipo == 1)
                {
                    var service = new WebService.WebServiceSoapClient();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var datos = js.Deserialize<List<Conversaciones>>(service.conv_cargarConversacionesSpamUsuario(user.username));
                    List<Conversaciones> finalList = new List<Conversaciones>();
                    foreach (var item in datos)
                    {
                        Conversaciones conv = item;
                        if (item.username2 == user.username)
                        {
                            var aux = item.username1;
                            conv.username1 = conv.username2;
                            conv.username2 = aux;
                        }
                        finalList.Add(conv);
                    }
                    return View(finalList);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }


        public ActionResult Cargar(int conv_id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0 || user.tipo == 1)
                {
                    var service = new WebService.WebServiceSoapClient();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var datos = js.Deserialize<List<Conv_Message>>(service.mes_cargarMensajes(conv_id));
                    MensajeFinal data = new MensajeFinal();
                    data.messages = datos;
                    data.user = user;
                    return View(data);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Conversar(string search)
        {
            Usuario user = (Usuario)Session["user"];
            var service = new WebService.WebServiceSoapClient();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var conv = js.Deserialize<Conversaciones>(service.conv_verificarConversacion(user.username, search));
            if (conv.conv_id == 0)
            {
                service.conv_crearConversacion(user.username, search, "");
            }
            js.Deserialize<Conversaciones>(service.conv_verificarConversacion(user.username, search));
            return RedirectToAction("Cargar?conv_id=" + conv.conv_id, "Conversaciones");
        }

        [HttpPost]
        public ActionResult Pull(int conv_id)
        {
            var service = new WebService.WebServiceSoapClient();
            return Json(service.mes_cargarMensajes(conv_id));
        }

        [HttpPost]
        public ActionResult Send(int conv_id, string message, string username)
        {
            var service = new WebService.WebServiceSoapClient();
            return Json(service.mes_enviarmensaje(conv_id, message, username));
        }

        [HttpPost]
        public ActionResult moveSpam(string username, int conv_id)
        {
            var service = new WebService.WebServiceSoapClient();
            return Json(service.conv_agregarSpam(username, conv_id));
        }

        [HttpPost]
        public ActionResult NoSpam(string username, int conv_id)
        {
            var service = new WebService.WebServiceSoapClient();
            return Json(service.conv_quitarSpam(username, conv_id));
        }
    }
}