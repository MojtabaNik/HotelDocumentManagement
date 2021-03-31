using System.Web;
using System.Web.Optimization;

namespace Archive
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                     "~/vendors/bower_components/jquery/dist/jquery.min.js",
                     "~/vendors/bower_components/bootstrap/dist/js/bootstrap.min.js",

                      "~/vendors/bower_components/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js",
                              "~/vendors/bower_components/Waves/dist/waves.min.js",
                               "~/vendors/bootstrap-growl/bootstrap-growl.min.js",
                               "~/vendors/bower_components/bootstrap-sweetalert/lib/sweet-alert.min.js",


                     "~/vendors/bower_components/moment/min/moment.min.js",
                     "~/vendors/bower_components/bootstrap-select/dist/js/bootstrap-select.js",
                     "~/vendors/bower_components/nouislider/distribute/jquery.nouislider.all.min.js",
                     //"~/vendors/bower_components/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
                     "~/js/bootstrap-persian-datetimepicker.min.js",
                     "~/vendors/bower_components/typeahead.js/dist/typeahead.bundle.min.js",
                     "~/vendors/summernote/dist/summernote-updated.min.js"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/ourJS").Include(
                     "~/js/functions.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/vendors/bower_components").Include("~/css/fontiran.css",
                      "~/vendors/bower_components/animate.css/animate.min.css",
                      "~/vendors/bower_components/bootstrap-sweetalert/lib/sweet-alert.css",
                      "~/vendors/bower_components/material-design-iconic-font/dist/css/material-design-iconic-font.min.css",
                      "~/vendors/bower_components/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/css/app.min.1.css",
                      "~/css/app.min.2.css"));

        }
    }
}
