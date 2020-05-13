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
    public class LocalesController : Controller
    {
        private BD_OIDOCOCINAEntities db = new BD_OIDOCOCINAEntities();

        // GET: Locales
        //public ActionResult Index()
        //{
        //    return View(db.LOCALES.ToList());
        //}

        [Authorize(Roles = "Administrador")]
        public ActionResult Admin()
        {
            return View(db.LOCALES.ToList());
        }

        //GET: Locales/Detalles/1
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOCALES lOCALES = db.LOCALES.Find(id);
            if (lOCALES == null)
            {
                return HttpNotFound();
            }
            var correoUsuario = User.Identity.GetUserName();
            ViewBag.exists = db.RESERVAS.Any(a => a.USUARIOS.Correo == correoUsuario && a.MESAS.Id_Local == id && a.Dia < DateTime.Now);
            return View(lOCALES );
        }

        public ActionResult Lista(string busquedaLocal)
        {
            if (!String.IsNullOrEmpty(busquedaLocal))
            {
                var lOCALES = db.LOCALES.Where(e => e.Nombre.Contains(busquedaLocal));
                ViewBag.busqueda = busquedaLocal;
                return View(lOCALES);
            }

            return View(db.LOCALES.ToList());
        }


        [Authorize(Roles = "Administrador")]
        // GET: Locales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOCALES lOCALES = db.LOCALES.Find(id);
            if (lOCALES == null)
            {
                return HttpNotFound();
            }
            return View(lOCALES);
        }

        // GET: Locales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Direccion,Correo,Telefono,Hora_apertura,Hora_cierre,Lunes,Martes,Miercoles,Jueves,Viernes,Sabado,Domingo,Carta")] LOCALES lOCALES)
        {

            lOCALES.Correo = User.Identity.GetUserName();

            if (ModelState.IsValid)
            {
                db.LOCALES.Add(lOCALES);
                db.SaveChanges();
                var path = Server.MapPath("~/Content/Images/" + lOCALES.Id);
                Directory.CreateDirectory(path);
                return RedirectToAction("Index", "Home");
            }

            return View(lOCALES);
        }


        [Authorize(Roles = "Administrador")]

        // GET: Locales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOCALES lOCALES = db.LOCALES.Find(id);
            if (lOCALES == null)
            {
                return HttpNotFound();
            }
            return View(lOCALES);
        }

        // GET: Locales/Edit --> Redireciona la vista de edición con el id del local
        public ActionResult Edit_Local()
        {
            // Se seleccionan los datos del local correspondiente al usuario actual
            string correoLocal = User.Identity.GetUserName();
            var lOCALES = db.LOCALES.Where(e => e.Correo == correoLocal).FirstOrDefault();
            if (lOCALES == null)
            {
                // Si no existe el empleado, redirige a la acción Index del controlador Home
                return RedirectToAction("Index", "Home");
            }
            // Si existe el empleado correspondiente se va a View
            return View(lOCALES);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Local([Bind(Include = "Id,Nombre,Direccion,Correo,Telefono,Hora_apertura,Hora_cierre,Lunes,Martes,Miercoles,Jueves,Viernes,Sabado,Domingo,Carta")] LOCALES lOCALES)
        {
            lOCALES.Correo = User.Identity.GetUserName();
            if (ModelState.IsValid)
            {
                db.Entry(lOCALES).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(lOCALES);
        }

        [Authorize(Roles = "Administrador")]
        // POST: Locales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Direccion,Correo,Telefono,Hora_apertura,Hora_cierre,Lunes,Martes,Miercoles,Jueves,Viernes,Sabado,Domingo,Carta")] LOCALES lOCALES)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(lOCALES).State = EntityState.Modified;
                db.SaveChanges();
                
                if (User.IsInRole("Local")) return RedirectToAction("Index");
                else if (User.IsInRole("Administrador")) return RedirectToAction("Admin");

            }
            return View(lOCALES);
        }



        [Authorize(Roles = "Administrador")]
        // GET: Locales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOCALES lOCALES = db.LOCALES.Find(id);
            if (lOCALES == null)
            {
                return HttpNotFound();
            }
            return View(lOCALES);
        }
        // POST: Locales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LOCALES lOCALES = db.LOCALES.Find(id);
            db.LOCALES.Remove(lOCALES);
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
