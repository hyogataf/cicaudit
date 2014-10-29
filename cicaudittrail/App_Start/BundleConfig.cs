using System.Web;
using System.Web.Optimization;

namespace cicaudittrail
{
    public class BundleConfig
    {
        // Pour plus d’informations sur le Bundling, accédez à l’adresse http://go.microsoft.com/fwlink/?LinkId=254725 (en anglais)
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                         "~/Scripts/sb-admin-2.js",
                         "~/Scripts/metisMenu.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                        "~/Scripts/bootstrap.min.js")); 

            bundles.Add(new ScriptBundle("~/bundles/datatablejs").Include(
                        "~/Scripts/DataTables-1.10.2/jquery.dataTables.min.js"));

            bundles.Add(new StyleBundle("~/Content/datatablecss").Include(
                        "~/Content/DataTables-1.10.2/css/jquery.dataTables.min.css"/*,
                         "~/Content/DataTables-1.10.2/css/jquery.dataTables_themeroller.css"*/));

            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                        "~/Content/bootstrap.min.css",
                        "~/Content/main.css",
                        "~/Content/metisMenu.min.css",
                        "~/Content/sb-admin-2.css",
                        "~/Content/font-awesome.min.css",
                        "~/Content/bootstrap-responsive.min.css"));
             
            bundles.Add(new StyleBundle("~/Content/jqueryuicss").Include(
                        "~/Content/themes/base/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/wysiwygcss").Include(
                        "~/Content/summernote-bs3.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryuijs").Include(
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusive").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/wysiwygjs").Include(
                        "~/Scripts/summernote.min.js"));
        }
    }
}