using System.Web;
using System.Web.Optimization;

namespace PatternCreator
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));



            //Layout
            bundles.Add(new StyleBundle("~/bundles/Layout-css").Include(
                "~/Content/bootstrap.css",
                "~/Content/LayoutStyles.css",
                "~/Content/stylePreload.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/Layout-js").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-1.12.1.min.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/umd/popper.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/preloader.js"
                
            ));

            //EditorPattern
            bundles.Add(new StyleBundle("~/bundles/EditorPattern-css").Include(
                "~/Content/EditorPatternStyles.css"
               
            ));
            bundles.Add(new ScriptBundle("~/bundles/EditorPattern-js").Include(
                
                "~/Scripts/wheelzoom.js",
                "~/Scripts/EditorPatternScripts.js"
               
            ));

        }
    }
}
