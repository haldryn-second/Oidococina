using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Proyecto_OIDOCOCINA.Models;

namespace Proyecto_OIDOCOCINA.Controllers
{
    public class GaleriaController : Controller
    {
        private BD_OIDOCOCINAEntities db = new BD_OIDOCOCINAEntities();

        // GET: Galeria
        public ActionResult Index()
        {

            string correoLocal = User.Identity.GetUserName();
            var local = db.LOCALES.Where(e => e.Correo == correoLocal).FirstOrDefault();
            if (local == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var gALERIA = db.GALERIA.Include(g => g.LOCALES);
            gALERIA = gALERIA.Where(a => a.Id_Local == local.Id).OrderByDescending(a =>
            a.Id);

            return View(gALERIA.ToList());
        }

        // GET: Galeria/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    GALERIA gALERIA = db.GALERIA.Find(id);
        //    if (gALERIA == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(gALERIA);
        //}

        // GET: Galeria/Create
        public ActionResult Create()
        {
            string correoLocal = User.Identity.GetUserName();
            ViewBag.Id_Local = new SelectList(db.LOCALES.Where(a => a.Correo == correoLocal), "Id", "Nombre");
            return View();
        }

        // POST: Galeria/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Local,Titulo,Descripcion,Portada,Ruta")] GALERIA gALERIA, HttpPostedFileBase Ruta)
        {
            if (ModelState.IsValid)
            {
                db.GALERIA.Add(gALERIA);

                if (gALERIA.Portada == true)
                {
                    gALERIA.Ruta = "portada" + Path.GetExtension(Ruta.FileName);

                    var elim_portada = db.GALERIA.Where(a => a.Portada == true && a.Id_Local == gALERIA.Id_Local).ToList();
                    foreach (var registro in elim_portada)
                    
                    db.GALERIA.Remove(registro);
                    db.SaveChanges();
                    
                        
                }
                else
                {
                    db.GALERIA.Add(gALERIA);
                    db.SaveChanges();
                    gALERIA.Ruta = gALERIA.Id + Path.GetExtension(Ruta.FileName);
                    db.SaveChanges();
                }
                db.Entry(gALERIA).State = EntityState.Modified;
                
                Ruta.SaveAs(Server.MapPath("~/Content/Images/"+gALERIA.Id_Local+"/" + gALERIA.Ruta));
                return RedirectToAction("Index");
            }

            ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", gALERIA.Id_Local);
            return View(gALERIA);
        }

        // GET: Galeria/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    GALERIA gALERIA = db.GALERIA.Find(id);
        //    if (gALERIA == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", gALERIA.Id_Local);
        //    return View(gALERIA);
        //}

        // POST: Galeria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Id_Local,Titulo,Descripcion,Portada,Ruta")] GALERIA gALERIA)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(gALERIA).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", gALERIA.Id_Local);
        //    return View(gALERIA);
        //}

        // GET: Galeria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GALERIA gALERIA = db.GALERIA.Find(id);
            if (gALERIA == null)
            {
                return HttpNotFound();
            }
            return View(gALERIA);
        }

        // POST: Galeria/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GALERIA gALERIA = db.GALERIA.Find(id);
            db.GALERIA.Remove(gALERIA);
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
