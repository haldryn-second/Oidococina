using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Proyecto_OIDOCOCINA.Models;

namespace Proyecto_OIDOCOCINA.Controllers
{
    public class NoticiasController : Controller
    {
        private BD_OIDOCOCINAEntities db = new BD_OIDOCOCINAEntities();

        // GET: Noticias
        public ActionResult Index()
        {
            string correoLocal = User.Identity.GetUserName();
            var local = db.LOCALES.Where(e => e.Correo == correoLocal).FirstOrDefault();
            if (local == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var nOTICIAS = db.NOTICIAS.Where(a => a.Id_Local == local.Id).Include(n => n.LOCALES);
            return View(nOTICIAS.ToList());
        }

        //// GET: Noticias/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    NOTICIAS nOTICIAS = db.NOTICIAS.Find(id);
        //    if (nOTICIAS == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(nOTICIAS);
        //}

        // GET: Noticias/Create
        [Authorize(Roles = "Local")]
        public ActionResult Create()
        {
            string correoLocal = User.Identity.GetUserName();
            ViewBag.Id_Local = new SelectList(db.LOCALES.Where(a => a.Correo == correoLocal), "Id", "Nombre");
            return View();
        }

        // POST: Noticias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Local,Titulo,Descripcion,Destacado,Fecha")] NOTICIAS nOTICIAS)
        {
            if (ModelState.IsValid)
            {
                nOTICIAS.Fecha = DateTime.Now;
                db.NOTICIAS.Add(nOTICIAS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", nOTICIAS.Id_Local);
            return View(nOTICIAS);
        }

        // GET: Noticias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTICIAS nOTICIAS = db.NOTICIAS.Find(id);
            if (nOTICIAS == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", nOTICIAS.Id_Local);
            return View(nOTICIAS);
        }

        // POST: Noticias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Local,Titulo,Descripcion,Destacado,Fecha")] NOTICIAS nOTICIAS)
        {
            if (ModelState.IsValid)
            {
                nOTICIAS.Fecha = DateTime.Now;
                db.Entry(nOTICIAS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", nOTICIAS.Id_Local);
            return View(nOTICIAS);
        }

        // GET: Noticias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTICIAS nOTICIAS = db.NOTICIAS.Find(id);
            if (nOTICIAS == null)
            {
                return HttpNotFound();
            }
            return View(nOTICIAS);
        }

        // POST: Noticias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NOTICIAS nOTICIAS = db.NOTICIAS.Find(id);
            db.NOTICIAS.Remove(nOTICIAS);
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
