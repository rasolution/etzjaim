using EtzJaimWebPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EtzJaimWebPage.Controllers
{
    public class ProductosController : Controller
    {
        // GET: Productos

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
                    var datos = js.Deserialize<List<Producto>>(service.product_cargarProductos());

                    return View(datos);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }
        #endregion


        #region CreateView
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

        #region CreateFunciont
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pro_nombre,pro_precio,pro_estado")] Producto product)
        {
            if (ModelState.IsValid)
            {
                var service = new WebService.WebServiceSoapClient();
                JavaScriptSerializer js = new JavaScriptSerializer();
                var datos = js.Deserialize<Respuesta>(service.product_guardarProducto(product.pro_nombre, product.pro_precio, product.pro_estado, product.pro_foto));
                ViewBag.Message = datos.response;

            }
            else
            {
                ViewBag.Message = "Llena los datos correctamente";
            }
            return View();
        }
        #endregion

        #region BorrarView
        public ActionResult Borrar(int pro_id)
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
                    var datos = js.Deserialize<Producto>(service.product_cargarProducto(pro_id));
                    Producto product = new Producto();
                    product.pro_nombre = datos.pro_nombre;
                    product.pro_precio = datos.pro_precio;
                    product.pro_estado = product.pro_estado;
                    product.pro_id = datos.pro_id;
                    return View(product);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }
        #endregion


        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult BorradoConfirmed(int pro_id)
        {
            if (ModelState.IsValid)
            {
                var service = new WebService.WebServiceSoapClient();
                JavaScriptSerializer js = new JavaScriptSerializer();
                var datos = js.Deserialize<Respuesta>(service.product_eliminarCliente(pro_id));
                ViewBag.Message = datos.response;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Atras()
        {
            return RedirectToAction("Index");
        }


        public ActionResult Product(int pro_id)
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
                    var datos = js.Deserialize<Producto>(service.product_cargarProducto(pro_id));
                    Producto product = new Producto();
                    product.pro_nombre = datos.pro_nombre;
                    product.pro_precio = datos.pro_precio;
                    product.pro_estado = product.pro_estado;
                    product.pro_id = datos.pro_id;
                    return View(product);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }

        public ActionResult Edit(int pro_id)
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
                    var datos = js.Deserialize<Producto>(service.product_cargarProducto(pro_id));
                    Producto product = new Producto();
                    product.pro_nombre = datos.pro_nombre;
                    product.pro_precio = datos.pro_precio;
                    product.pro_estado = product.pro_estado;
                    product.pro_id = datos.pro_id;
                    return View(product);
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }
        [HttpPost]
        public JsonResult EditProduct(int pro_id,string pro_nombre,int pro_precio,int pro_estado)
        {
            
            var service = new WebService.WebServiceSoapClient();
            return Json(service.product_editarProducto(pro_id,pro_nombre,pro_precio,pro_estado,null));
        }

        
    }

}