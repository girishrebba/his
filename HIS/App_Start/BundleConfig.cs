using System.Web;
using System.Web.Optimization;

namespace HIS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                       "~/light/js/plugins/datepicker/bootstrap-datepicker.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/light/css/bootstrap.min.css",
                      "~/light/css/entypo.css",
                      "~/light/css/font-awesome.min.css",     
                      "~/light/css/mouldifi-core.css",
                      "~/light/css/mouldifi-forms.css",
                      "~/Content/jquery-ui.min.css",
                      "~/light/css/plugins/datepicker/bootstrap-datepicker.css"));
        }
    }
}