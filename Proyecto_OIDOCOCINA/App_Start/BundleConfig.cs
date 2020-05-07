﻿using System.Web;
using System.Web.Optimization;

namespace Proyecto_OIDOCOCINA
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lightbox").Include(
                    "~/Scripts/lightbox-2.6.js"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            "~/Content/lightbox.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //// Añadido para Datapicker de JQueryUIHelpers.Mvc5
            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //"~/Scripts/jquery-ui-{version}.js",
            //"~/Scripts/jquery-ui.unobtrusive-{version}.js"));

            //// Añadido para Datapicker de JQueryUIHelpers.Mvc5
            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //"~/Content/themes/base/core.css",
            //"~/Content/themes/base/datepicker.css",
            //"~/Content/themes/base/theme.css"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
