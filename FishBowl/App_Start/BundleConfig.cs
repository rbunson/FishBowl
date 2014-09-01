using System.Web;
using System.Web.Optimization;

namespace FishBowl
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                        "~/Scripts/modernizr-*", 
                        "~/Scripts/jquery-{version}.js", 
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-notify.js",
                        "~/Scripts/angular.js", 
                        "~/Scripts/json2.js", 
                        "~/Scripts/jquery.signalR-1.1.3.js", 
                        "~/Scripts/underscore.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-notify.css",
                "~/Content/bootstrap-custom.css"));

        }
    }
}