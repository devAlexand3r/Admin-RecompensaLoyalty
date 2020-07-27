using System.Web;
using System.Web.Optimization;

namespace JulioLoyalty.UI
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            #region --> Scripts for default
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/popper.min.js",
                "~/Scripts/bootstrap-4.0.0.js",
                "~/Scripts/Utils.js",
                "~/Scripts/sweetalert2.all.min.js",
                //"~/Scripts/sweetalert.min.js",
                "~/Scripts/global/swalAlert.js"
                //"~/Scripts/global/direccion.js"
                ));
            #endregion

            #region --> Styles for default
            bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap-4.0.0.css",
                   "~/Content/estilosHome.css",
                   "~/Content/Layaut.min.css",
                   "~/Content/dataTables/dataTables.bootstrap.min.css",
                    //"~/Content/bootstrapSelect\bootstrap-select.min.css",
                    "~/Content/sweetalert2.css",
                    "~/Content/bootstrapDatepicker\bootstrap-datepicker.min.css"
               ));
            #endregion

            // Bootstrap select js
            bundles.Add(new ScriptBundle("~/bundles/bootstrapSelect").Include(
                 "~/Scripts/bootstrapSelect/bootstrap-select.min.js",
                 "~/Scripts/bootstrapSelect/bootstrap-select.es.min.js"
            ));

            // Bootstrap datepicker js
            bundles.Add(new ScriptBundle("~/bundles/bootstrapDatepicker").Include(
                "~/Scripts/bootstrapDatepicker/bootstrap-datepicker.min.js",
                "~/Scripts/bootstrapDatepicker/bootstrap-datepicker.es.min.js"
            ));

            // Data tables js
            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                "~/Scripts/dataTables/jquery.dataTables.min.js",
                "~/Scripts/dataTables/dataTables.bootstrap.min.js"
            ));

            #region --> Resource for Kendo.UI
            bundles.Add(new ScriptBundle("~/bundles/kendo/scripts").Include(
              "~/Scripts/jquery-1.10.2.min.js",
              "~/Scripts/kendo/kendo.all.min.js",
              "~/Scripts/kendo/kendo.aspnetmvc.min.js",
              "~/Scripts/kendo/kendo.culture.es-MX.min.js"));

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
                "~/Content/kendo/kendo.common-bootstrap.min.css",
                "~/Content/kendo/kendo.bootstrap.min.css"
            ));
            #endregion
        }
    }
}