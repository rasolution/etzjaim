using EtzJaimWebPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EtzJaimWebPage.Controllers
{
    public class CitasController : Controller
    {
        // GET: Citas
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
                    var datos = js.Deserialize<List<Cita_Usuario>>(service.cita_cargarCitas());

                    return View(datos);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }
        #endregion

        #region index
        public ActionResult Aprobadas()
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
                    var datos = js.Deserialize<List<Cita_Usuario>>(service.cita_cargarCitasAprobadas());

                    return View(datos);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }
        #endregion


        #region index
        public ActionResult Pendientes()
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
                    var datos = js.Deserialize<List<Cita_Usuario>>(service.cita_cargarCitasPendientes());

                    return View(datos);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }
        #endregion

        #region index
        public ActionResult Cargar(int cita_id)
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
                    var datos = js.Deserialize<Cita_Usuario>(service.cita_cargarCita(cita_id));
                    Cita_Usuario cita = new Cita_Usuario();
                    cita.username = datos.username;
                    cita.user_info = datos.user_info;
                    cita.cita_id = datos.cita_id;
                    cita.cita_estado = datos.cita_id;
                    cita.cita_fecha = datos.cita_fecha;
                    return View(cita);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }
        #endregion

        [HttpPost]
        public JsonResult Aprobar(int cita_id)
        {
            var service = new WebService.WebServiceSoapClient();
            return Json(service.cita_aprobarCita(cita_id));
        }

        [HttpPost]
        public JsonResult Rechazar(int cita_id)
        {
            var service = new WebService.WebServiceSoapClient();
            return Json(service.cita_rechazarCita(cita_id));
        }

        public ActionResult Solicitar()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
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
                    return RedirectToAction("Index", "Home");

                }
            }
        }
        [HttpPost, ActionName("Solicitar")]
        [ValidateAntiForgeryToken]
        public ActionResult SolicitarConfirmada(string fecha, string hora)
        {
            if (ModelState.IsValid)
            {
                var service = new WebService.WebServiceSoapClient();
                JavaScriptSerializer js = new JavaScriptSerializer();

                Usuario user = (Usuario)Session["user"];
                var data = js.Deserialize<Respuesta>(service.cita_solicitarCita(user.username, fecha, hora));
                ViewBag.Message = data.response;
            }
            else
            {
                ViewBag.Message = "Llena los datos correctamente";
            }
            return View();
        }

    }
}