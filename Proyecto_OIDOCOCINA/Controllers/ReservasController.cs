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
    public class ReservasController : Controller
    {
        private BD_OIDOCOCINAEntities db = new BD_OIDOCOCINAEntities();

        public static class CustomRoles
        {
            public const string Administrator = "Administrador";
            public const string User = "Usuario";
        }

        // GET: Reservas
        [Authorize(Roles = "Usuario")]
        public ActionResult Index()
        {
            string correoUsu = User.Identity.GetUserName();
            var usuario_actual = db.USUARIOS.Where(e => e.Correo == correoUsu).FirstOrDefault();

            var rESERVAS = db.RESERVAS.Include(r => r.MESAS).Include(r => r.USUARIOS).Where(r=>r.Id_Usuario== usuario_actual.Id);
            rESERVAS = rESERVAS.OrderByDescending(r => r.Dia).ThenBy(r => r.Hora);

            ViewBag.dia_ac = DateTime.Now;
            return View(rESERVAS.ToList());
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Admin(string busquedaLocal)
        {
            var fecha_actual = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fecha_ac = Convert.ToDateTime(fecha_actual);
            ViewBag.fecha_actual = fecha_ac;

            var rESERVAS = db.RESERVAS.Include(r => r.MESAS).Include(r => r.USUARIOS);
            rESERVAS = rESERVAS.OrderByDescending(r => r.Dia).ThenBy(r => r.Hora);

            if (!String.IsNullOrEmpty(busquedaLocal))
            {
                rESERVAS = rESERVAS.Where(r => r.MESAS.LOCALES.Nombre.Contains(busquedaLocal));
            }

            return View(rESERVAS.ToList());
        }
        [Authorize(Roles = "Local")]
        public ActionResult ReservasPorLocal(string dia)
        {

            var fecha_actual = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fecha_ac = Convert.ToDateTime(fecha_actual);
            ViewBag.fecha_actual = fecha_ac;

            string correolocal = User.Identity.GetUserName();
            var local_actual = db.LOCALES.Where(e => e.Correo == correolocal).FirstOrDefault();

            var rESERVAS = db.RESERVAS.Include(r => r.MESAS).Include(r => r.USUARIOS).Where(r => r.MESAS.Id_Local == local_actual.Id);
            rESERVAS = rESERVAS.OrderByDescending(r => r.Dia).ThenBy(r=>r.Hora);

            if (!string.IsNullOrEmpty(dia))
            {
                DateTime dia_buscar = Convert.ToDateTime(dia);
                rESERVAS = rESERVAS.Where(r=>r.Dia == dia_buscar);
            }

                return View(rESERVAS.ToList());
        }

        //public ActionResult Locales()
        //{
        //    return View(db.LOCALES.ToList());
        //}

        // GET: Reservas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESERVAS rESERVAS = db.RESERVAS.Find(id);
            if (rESERVAS == null)
            {
                return HttpNotFound();
            }
            return View(rESERVAS);
        }
        [Authorize(Roles = CustomRoles.Administrator + "," + CustomRoles.User)]
        // GET: Reservas/Create
        public ActionResult Create(int? id, string hora, string dia)
        {
            var correoUsuario = User.Identity.GetUserName();
            ViewBag.idlocal = id;
            ViewBag.idlocal = id;
            ViewBag.Id_Mesa = new SelectList(db.MESAS.Where(c => c.Id_Local==id), "Id", "Descripcion");
            ViewBag.nummesas = new SelectList(db.MESAS.Where(c => c.Id_Local == id), "Id", "Descripcion").Count();
            ViewBag.Capacidad_Mesa = new SelectList(db.MESAS.Where(c => c.Id_Local == id), "Id", "Capacidad");
            ViewBag.Id_Usuario = new SelectList(db.USUARIOS.Where(a => a.Correo== correoUsuario), "Id", "Id");
            
            LOCALES local = db.LOCALES.Find(id);
            ViewBag.hora_cierre = local.Hora_cierre.ToString("hh\\:mm");
            ViewBag.hora_apert = local.Hora_apertura.ToString("hh\\:mm");
            ViewBag.nombre_local = local.Nombre;

            ViewBag.mesilla = new SelectList(db.MESAS.Where(c => c.Id_Local == id), "Id", "Descripcion");

            if (!string.IsNullOrEmpty(hora) && !string.IsNullOrEmpty(dia))
            {
                ViewBag.returned = 1;
                DateTime dia_reserv = Convert.ToDateTime(dia);
                TimeSpan hora_reserv = TimeSpan.Parse(hora);
                TimeSpan hora_diferencia = TimeSpan.FromHours(1);
                TimeSpan hora_extra = hora_reserv.Add(hora_diferencia);

                ViewBag.Dia = dia_reserv;
                ViewBag.Hora = hora;
                ViewBag.Id_Mesa = (from m in db.MESAS
                                   join l in db.LOCALES on m.Id_Local equals l.Id
                                   where l.Id==id && !(
                                   from r in db.RESERVAS
                                   join me in db.MESAS on r.Id_Mesa equals me.Id
                                   where (r.Dia == dia_reserv && r.Hora == hora_reserv)
                                   select r.Id_Mesa).Contains(m.Id)
                                   select new SelectListItem
                                   {
                                       Value = m.Id.ToString(),
                                       Text = m.Descripcion
                                   }
                                  ).Distinct();

                ViewBag.nummesas = (from m in db.MESAS
                                    join l in db.LOCALES on m.Id_Local equals l.Id
                                    where l.Id == id && !(
                                    from r in db.RESERVAS
                                    join me in db.MESAS on r.Id_Mesa equals me.Id
                                    where (r.Dia == dia_reserv && r.Hora == hora_reserv)
                                    select r.Id_Mesa).Contains(m.Id)
                                    select new SelectListItem
                                    {
                                        Value = m.Id.ToString(),
                                        Text = m.Descripcion
                                    }
                                  ).Count();


            }


            return View();
        }


        //from m in db.MESAS
        //                           join l in db.LOCALES
        //                           on m.Id_Local equals l.Id
        //                           join r in db.RESERVAS
        //                           on m.Id equals r.Id_Mesa into mesa_reserva
        //                           from mr in mesa_reserva.DefaultIfEmpty()
        //                           where l.Id == id && !(mr.Dia == dia_reserv && mr.Hora == hora_reserv)


        // POST: Reservas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]


        public ActionResult Create([Bind(Include = "Id_Usuario,Id_Mesa,Dia,Hora,Num_personas")] RESERVAS rESERVAS)
        {
            if (ModelState.IsValid)
            {
                db.RESERVAS.Add(rESERVAS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Mesa = new SelectList(db.MESAS, "Id", "Descripcion", rESERVAS.Id_Mesa);
            ViewBag.Id_Usuario = new SelectList(db.USUARIOS, "Id", "Nombre", rESERVAS.Id_Usuario);
            return View(rESERVAS);
        }

        //GET: Reservas/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RESERVAS rESERVAS = db.RESERVAS.Find(id);
        //    if (rESERVAS == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Id_Mesa = new SelectList(db.MESAS, "Id", "Descripcion", rESERVAS.Id_Mesa);
        //    ViewBag.Id_Usuario = new SelectList(db.USUARIOS, "Id", "Nombre", rESERVAS.Id_Usuario);
        //    return View(rESERVAS);
        //}

        // POST: Reservas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id_Usuario,Id_Mesa,Dia,Hora,Num_personas")] RESERVAS rESERVAS)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(rESERVAS).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Id_Mesa = new SelectList(db.MESAS, "Id", "Descripcion", rESERVAS.Id_Mesa);
        //    ViewBag.Id_Usuario = new SelectList(db.USUARIOS, "Id", "Nombre", rESERVAS.Id_Usuario);
        //    return View(rESERVAS);
        //}

        // GET: Reservas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESERVAS rESERVAS = db.RESERVAS.Find(id);
            if (rESERVAS == null)
            {
                return HttpNotFound();
            }
            return View(rESERVAS);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RESERVAS rESERVAS = db.RESERVAS.Find(id);
            db.RESERVAS.Remove(rESERVAS);
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
