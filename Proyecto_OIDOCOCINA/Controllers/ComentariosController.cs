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
    public class ComentariosController : Controller
    {
        private BD_OIDOCOCINAEntities db = new BD_OIDOCOCINAEntities();

        // GET: Comentarios
        public ActionResult Index()
        {
            var cOMENTARIOS = db.COMENTARIOS.Include(c => c.LOCALES).Include(c => c.USUARIOS);
            return View(cOMENTARIOS.ToList());
        }

        // GET: Comentarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMENTARIOS cOMENTARIOS = db.COMENTARIOS.Find(id);
            if (cOMENTARIOS == null)
            {
                return HttpNotFound();
            }
            return View(cOMENTARIOS);
        }

        // GET: Comentarios/Create
        [Authorize(Roles = "Usuario")]
        public ActionResult Create(int? id)
        {


            var correoUsuario = User.Identity.GetUserName();

            ViewBag.Id_Usuario = new SelectList(db.USUARIOS.Where(a => a.Correo == correoUsuario), "Id", "Id");
            ViewBag.Id_Local = new SelectList(db.LOCALES.Where(a => a.Id == id), "Id", "Id");

            ViewBag.exists = db.RESERVAS.Any(a => a.USUARIOS.Correo == correoUsuario && a.MESAS.Id_Local == id && a.Dia < DateTime.Now);

            if (!ViewBag.exists)
            {
                return RedirectToAction("Detalles", "Locales", new { id = id });
            }

            return View();
        }

        // POST: Comentarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Local,Id_Usuario,Comentario,Puntuacion,Fecha")] COMENTARIOS cOMENTARIOS)
        {
            if (ModelState.IsValid)
            {
                cOMENTARIOS.Fecha = DateTime.Now;

                var elim_coment = db.COMENTARIOS.Where(a => a.Id_Usuario == cOMENTARIOS.Id_Usuario && a.Id_Local == cOMENTARIOS.Id_Local).ToList();
                foreach (var registro in elim_coment)
                {
                    db.COMENTARIOS.Remove(registro);
                }

                db.COMENTARIOS.Add(cOMENTARIOS);
                db.SaveChanges();
                return RedirectToAction("Detalles", "Locales", new { id = cOMENTARIOS.Id_Local });
            }

            ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", cOMENTARIOS.Id_Local);
            ViewBag.Id_Usuario = new SelectList(db.USUARIOS, "Id", "Nombre", cOMENTARIOS.Id_Usuario);
            return View(cOMENTARIOS);
        }

        // GET: Comentarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMENTARIOS cOMENTARIOS = db.COMENTARIOS.Find(id);
            if (cOMENTARIOS == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", cOMENTARIOS.Id_Local);
            ViewBag.Id_Usuario = new SelectList(db.USUARIOS, "Id", "Nombre", cOMENTARIOS.Id_Usuario);
            return View(cOMENTARIOS);
        }

        // POST: Comentarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Local,Id_Usuario,Comentario,Puntuacion,Fecha")] COMENTARIOS cOMENTARIOS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOMENTARIOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Local = new SelectList(db.LOCALES, "Id", "Nombre", cOMENTARIOS.Id_Local);
            ViewBag.Id_Usuario = new SelectList(db.USUARIOS, "Id", "Nombre", cOMENTARIOS.Id_Usuario);
            return View(cOMENTARIOS);
        }

        // GET: Comentarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMENTARIOS cOMENTARIOS = db.COMENTARIOS.Find(id);
            if (cOMENTARIOS == null)
            {
                return HttpNotFound();
            }
            return View(cOMENTARIOS);
        }

        // POST: Comentarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COMENTARIOS cOMENTARIOS = db.COMENTARIOS.Find(id);
            db.COMENTARIOS.Remove(cOMENTARIOS);
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
