using AdminWeb.Models;
using AdminWeb.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AdminWeb.Estructuras;
namespace AdminWeb.Controllers
{
    public class AdminsController : Controller
    {
        // GET: Admins
        static Response response = new Response();
        public ActionResult Index()
        {
            var service = new WebServiceSoapClient();
            string json = service.admin_CargarTodos();
            JavaScriptSerializer Json = new JavaScriptSerializer();
            var datos=Json.Deserialize<List<AdminModel>>(json);
            ViewBag.Message = response.response;
            return View(datos);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Admin_username,Admin_password")] AdminModel admin)
        {
            if (ModelState.IsValid)
            {
                var service = new WebServiceSoapClient();
                string json = service.admin_Guardar(admin.Admin_username, admin.Admin_password);
                JavaScriptSerializer Json = new JavaScriptSerializer();
                var responseservice = Json.Deserialize<Response>(json);
                response = responseservice;
                return RedirectToAction("Index");
            }
            return View(admin);
        }

    }
}