using System.Web;
using System.Web.Optimization;

namespace MockupApp
{
    public class BundleConfig
    {
       
            // Recibe una colección de bundles donde existen tres tipos: ScriptBundle para los scripts, StyleBundle para las hojas de estilos y
            //Bundle para aquellos casos que queramos personalizar la unificación, interpretación y reducción de los archivos elegidos para ese tipo.
            public static void RegisterBundles(BundleCollection bundles)
            {
                //Quitamos ScriptBundle y dejamos solo Bundle Ya que a partir de la version >5 de Bootstrap no responde a ScriptBundle
                //Bundle de tipo Script donde podemos agregar archivos .js
                bundles.Add(new Bundle("~/bundles/jquery").Include(
                            "~/Scripts/jquery-3.7.0.js"));

                //Agregamos los js para los iconos y el script de funcionamiento del dashboard
                bundles.Add(new Bundle("~/bundles/complementos").Include(
                           "~/Scripts/fontawesome/all.min.js",
                           //"~/Scripts/DataTables/jquery.dataTables.js",
                           //"~/Scripts/DataTables/dataTables.responsive.js",
                           "~/Scripts/script.js"));

                bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                            "~/Scripts/jquery.validate*"));
                // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios.  De esta manera estará
                // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
                bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

                // Quitamos ScriptBundle y dejamos solo Bundle
                //Ya que a partir de la version >5 de Bootstrap no responde a ScriptBundle
                //Cambiar por bootstrap.bundle.js
                bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                          "~/Scripts/bootstrap.bundle.js"));

                bundles.Add(new StyleBundle("~/Content/css").Include(
                          "~/Content/bootstrap.css",
                          //"~/Content/DataTables/css/jquery.dataTables.css",
                          //"~/Content/DataTables/css/responsive.dataTables.css",
                          "~/Content/Site.css"));
            }
    }
}
