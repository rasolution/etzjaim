using EtzJaimWebPage.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EtzJaimWebPage.Controllers
{
    public class ProductosController : Controller
    {
        private bool HasAdminAccess()
        {
            return service.IsAdminSession(Session);
        }

        private ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }

        private Producto FetchProductSafe(int pro_id)
        {
            string raw = service.CallSoap(client => client.product_cargarProducto(pro_id), null);
            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Producto datos = js.Deserialize<Producto>(raw);
            if (datos == null)
            {
                return null;
            }

            return new Producto
            {
                pro_id = datos.pro_id,
                pro_nombre = datos.pro_nombre,
                pro_precio = datos.pro_precio,
                pro_estado = datos.pro_estado,
                pro_foto = datos.pro_foto
            };
        }

        public ActionResult Index()
        {
            if (!HasAdminAccess())
            {
                return RedirectToHome();
            }

            string raw = service.CallSoap(client => client.product_cargarProductos(), null);
            JavaScriptSerializer js = new JavaScriptSerializer();
            var datos = string.IsNullOrWhiteSpace(raw)
                ? new List<Producto>()
                : (js.Deserialize<List<Producto>>(raw) ?? new List<Producto>());

            return View(datos);
        }

        public ActionResult Create()
        {
            if (!HasAdminAccess())
            {
                return RedirectToHome();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pro_nombre,pro_precio,pro_estado")] Producto product)
        {
            if (!HasAdminAccess())
            {
                return RedirectToHome();
            }

            if (ModelState.IsValid && product != null && !string.IsNullOrWhiteSpace(product.pro_nombre))
            {
                string raw = service.CallSoap(
                    client => client.product_guardarProducto(product.pro_nombre.Trim(), product.pro_precio, product.pro_estado, product.pro_foto),
                    null);
                if (!string.IsNullOrWhiteSpace(raw))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Respuesta datos = js.Deserialize<Respuesta>(raw);
                    ViewBag.Message = datos != null ? datos.response : "Producto procesado.";
                }
                else
                {
                    ViewBag.Message = "No se pudo conectar con el servicio de productos.";
                }
            }
            else
            {
                ViewBag.Message = "Llena los datos correctamente";
            }
            return View(product);
        }

        public ActionResult Borrar(int pro_id)
        {
            if (!HasAdminAccess())
            {
                return RedirectToHome();
            }

            Producto product = FetchProductSafe(pro_id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult BorradoConfirmed(int pro_id)
        {
            if (!HasAdminAccess())
            {
                return RedirectToHome();
            }

            if (ModelState.IsValid)
            {
                string raw = service.CallSoap(client => client.product_eliminarCliente(pro_id), null);
                if (!string.IsNullOrWhiteSpace(raw))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Respuesta datos = js.Deserialize<Respuesta>(raw);
                    ViewBag.Message = datos != null ? datos.response : string.Empty;
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Atras()
        {
            return RedirectToAction("Index");
        }

        public ActionResult Product(int pro_id)
        {
            if (!HasAdminAccess())
            {
                return RedirectToHome();
            }

            Producto product = FetchProductSafe(pro_id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Edit(int pro_id)
        {
            if (!HasAdminAccess())
            {
                return RedirectToHome();
            }

            Producto product = FetchProductSafe(pro_id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpPost]
        public JsonResult EditProduct(int pro_id, string pro_nombre, int pro_precio, int pro_estado)
        {
            if (!HasAdminAccess())
            {
                return Json(new { response = "unauthorized" });
            }

            if (string.IsNullOrWhiteSpace(pro_nombre) || pro_precio < 0)
            {
                return Json(new { response = "invalid" });
            }

            string result = service.CallSoap(
                client => client.product_editarProducto(pro_id, pro_nombre.Trim(), pro_precio, pro_estado, null),
                "{\"response\":\"error\"}");
            return Json(result);
        }
    }
}
