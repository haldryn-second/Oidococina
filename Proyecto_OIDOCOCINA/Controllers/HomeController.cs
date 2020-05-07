using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Proyecto_OIDOCOCINA.Models;

namespace Proyecto_OIDOCOCINA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BD_OIDOCOCINAEntities db = new BD_OIDOCOCINAEntities();
            // Si existe el empleado correspondiente al usuario actual
            // se va a View, en caso contrario se va a crear el empleado.
            string usuario = User.Identity.GetUserName();
            var empleado = db.USUARIOS.Where(u => u.Correo == usuario).FirstOrDefault();
            var local = db.LOCALES.Where(l => l.Correo == usuario).FirstOrDefault();
            if (User.Identity.IsAuthenticated &&
            User.IsInRole("Usuario") &&
            empleado == null)
            {
                return RedirectToAction("Create", "Usuarios");
            }

            if (User.Identity.IsAuthenticated &&
            User.IsInRole("Local") &&
            local == null)
            {
                return RedirectToAction("Create", "Locales");
            }

            return View(db.LOCALES.ToList());
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}