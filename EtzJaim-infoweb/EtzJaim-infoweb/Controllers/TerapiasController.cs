using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EtzJaim_infoweb.Models;

namespace EtzJaim_infoweb.Controllers
{
    public class TerapiasController : Controller
    {
        private TerapiasModeloDBContext db = new TerapiasModeloDBContext();

        // GET: /Terapias/
        public ActionResult Index()
        {
            return View(db.TerapiasModelo.ToList());
        }

        // GET: /Terapias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TerapiasModelo terapiasmodelo = db.TerapiasModelo.Find(id);
            if (terapiasmodelo == null)
            {
                return HttpNotFound();
            }
            return View(terapiasmodelo);
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
        public ActionResult Create([Bind(Include="ID,Name,Price,Description,Time")] TerapiasModelo terapiasmodelo)
        {
            if (ModelState.IsValid)
            {
                db.TerapiasModelo.Add(terapiasmodelo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(terapiasmodelo);
        }

        // GET: /Terapias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TerapiasModelo terapiasmodelo = db.TerapiasModelo.Find(id);
            if (terapiasmodelo == null)
            {
                return HttpNotFound();
            }
            return View(terapiasmodelo);
        }

        // POST: /Terapias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,Price,Description,Time")] TerapiasModelo terapiasmodelo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(terapiasmodelo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(terapiasmodelo);
        }

        // GET: /Terapias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TerapiasModelo terapiasmodelo = db.TerapiasModelo.Find(id);
            if (terapiasmodelo == null)
            {
                return HttpNotFound();
            }
            return View(terapiasmodelo);
        }

        // POST: /Terapias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TerapiasModelo terapiasmodelo = db.TerapiasModelo.Find(id);
            db.TerapiasModelo.Remove(terapiasmodelo);
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
