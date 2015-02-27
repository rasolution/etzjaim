using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EtzJaim_infoweb.Models;
using EtzJaim_infoweb.DAL;

namespace EtzJaim_infoweb.Controllers
{
    public class TerapiasController : Controller
    {
        private Context db = new Context();

        // GET: /Terapias/
        public ActionResult Index()
        {
            return View(db.Terapias.ToList());
        }

        // GET: /Terapias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terapias terapias = db.Terapias.Find(id);
            if (terapias == null)
            {
                return HttpNotFound();
            }
            return View(terapias);
        }

        // GET: /Terapias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Terapias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Price,Description,Time")] Terapias terapias)
        {
            if (ModelState.IsValid)
            {
                db.Terapias.Add(terapias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(terapias);
        }

        // GET: /Terapias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terapias terapias = db.Terapias.Find(id);
            if (terapias == null)
            {
                return HttpNotFound();
            }
            return View(terapias);
        }

        // POST: /Terapias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,Price,Description,Time")] Terapias terapias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(terapias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(terapias);
        }

        // GET: /Terapias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terapias terapias = db.Terapias.Find(id);
            if (terapias == null)
            {
                return HttpNotFound();
            }
            return View(terapias);
        }

        // POST: /Terapias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Terapias terapias = db.Terapias.Find(id);
            db.Terapias.Remove(terapias);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
