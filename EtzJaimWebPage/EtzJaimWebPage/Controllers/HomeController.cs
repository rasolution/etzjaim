using EtzJaimWebPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EtzJaimWebPage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 0)
                {
                    return RedirectToAction("Menu");
                }
                else
                {
                    return RedirectToAction("Cliente");
                }
            }
            else
            {
                return View();
            }
        }
        public ActionResult Menu()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index");
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
                    return RedirectToAction("Index");
                }
            }
            
        }

        public ActionResult Cliente()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 1)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult Close()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Usuario user = (Usuario)Session["user"];
                if (user.tipo == 1 || user.tipo == 0)
                {
                    Session["user"] = null;
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "username,password")] Usuario usuario)
        {
            var service = new WebService.WebServiceSoapClient();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var response = js.Deserialize<Respuesta_Usuario>(service.user_Login(usuario.username, usuario.password));
            if (response.response.Equals("yes"))
            {
                Session["user"] = response.user;
                if (response.user.tipo == 0)
                {
                    return RedirectToAction("Menu");
                }
                else
                {
                    return RedirectToAction("Cliente");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}