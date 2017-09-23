using System.Web;
using System.Web.Optimization;

namespace SnapShop.Design
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Referances/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Referances/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/Referances/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Referances/bootstrap.min.js",
                      "~/Scripts/Referances/Referancesbootstrap.js",
                      "~/Scripts/Referances/Referancesrespond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                         "~/Scripts/Referances/angular.js",
                         "~/Scripts/Referances/angular-route.js",
                         "~/Scripts/Referances/angular-cookies.js",
                         "~/Scripts/Referances/angular-sanitize.js",
                         "~/Scripts/Referances/angular-local-storage.min.js",
                         "~/Scripts/Referances/angular.touch.min.js",
                         "~/Scripts/Referances/angular-animate.min.js",
                         "~/Scripts/Referances/angular-file-upload.min.js",
                         "~/Scripts/Referances/loading-bar.js",
                          "~/Scripts/Referances/angular-strap.min.js",
                          "~/Scripts/Referances/angular-strap.tpl.min.js" 
                         ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                           "~/Scripts/App/app.js",
                           "~/Scripts/App/route.js",
                           "~/Scripts/App/Controllers/topDealsCtrl.js",
                           "~/Scripts/App/Controllers/aboutCtrl.js"
               ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/card.css",
                       "~/Content/font-awesome.css",
                      "~/Content/site.css",
                      "~/Content/movingBackground.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            //BundleTable.EnableOptimizations = true;
        }
    }
}
