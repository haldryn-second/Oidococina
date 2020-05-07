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
    public class MesasController : Controller
    {
        private BD_OIDOCOCINAEntities db = new BD_OIDOCOCINAEntities();

        [Authorize(Roles = "Administrador")]
        // GET: Mesas
        public ActionResult Index()
        {
            var mESAS = db.MESAS.Include(m => m.LOCALES);
            return View(mESAS.ToList());
        }

        [Authorize(Roles = "Local")]
        public ActionResult MesasPorLocal()
        {

            //string correoLocal = User.Identity.GetUserName();
            //LOCALES codlocal = db.LOCALES.Find(correoLocal);

            
            //var lOCALES = db.LOCALES.Where(e => e.Correo == correoLocal);
            //var mESAS = db.MESAS.Include(m => m.LOCALES);

            //return View(codlocal);

            // Se seleccionan los datos del empleado correspondiente al local
            string correoLocal = User.Identity.GetUserName();
            var local = db.LOCALES.Where(e => e.Correo == correoLocal).FirstOrDefault();
            if (local == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var mesas = db.MESAS.Include(a => a.LOCALES);

            // Se seleccionan los avisos del Empleado correspondiente al usuario actual
            mesas = mesas.Where(a => a.Id_Local == local.Id).OrderByDescending(a =>
            a.Id);

            return View(mesas.ToList());
        }

        //// GET: Mesas/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    MESAS mESAS = db.MESAS.Find(id);
        //    if (mESAS == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mESAS);
        //}

        // GET: Mesas/Create
        public ActionResult Create()
        {
            ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre");
            return View();
        }

        // POST: Mesas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Local,Descripcion,Capacidad")] MESAS mESAS)
        {
            string correoLocal = User.Identity.GetUserName();
            var local = db.LOCALES.Where(e => e.Correo == correoLocal).FirstOrDefault();
            mESAS.Id_Local = local.Id;

            if (ModelState.IsValid)
            {
                db.MESAS.Add(mESAS);
                db.SaveChanges();
                return RedirectToAction("MesasPorLocal");
            }

            ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", mESAS.Id_Local);
            return View(mESAS);
        }

        // GET: Mesas/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    MESAS mESAS = db.MESAS.Find(id);
        //    if (mESAS == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", mESAS.Id_Local);
        //    return View(mESAS);
        //}

        // POST: Mesas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Id_Local,Descripcion,Capacidad")] MESAS mESAS)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(mESAS).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("MesasPorLocal");
        //    }
        //    ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", mESAS.Id_Local);
        //    return View(mESAS);
        //}

        // GET: Mesas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MESAS mESAS = db.MESAS.Find(id);
            if (mESAS == null)
            {
                return HttpNotFound();
            }
            return View(mESAS);
        }

        // POST: Mesas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MESAS mESAS = db.MESAS.Find(id);
            db.MESAS.Remove(mESAS);
            db.SaveChanges();
            return RedirectToAction("MesasPorLocal");
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
