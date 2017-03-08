using System.Web.Optimization;

namespace Arcgis.Utils.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Template 1

            bundles.Add(new StyleBundle("~/Content/1/css/").Include(
                                        "~/Content/1/css/esri.css",
                                        "~/Content/1/css/style.css",
                                        "~/Content/1/css/style-metro.css",
                                        "~/Content/1/css/style-responsive.css",
                                        "~/Content/1/css/themes/default.css",
                                        "~/Content/1/css/pages/inbox.css",
                                         "~/Content/1/css/autocomplete-ui-input.css"
                                        ));
            bundles.Add(new StyleBundle("~/Content/1/plugins/").Include(
                                        "~/Content/1/plugins/bootstrap/css/bootstrap.min.css",
                                        "~/Content/1/plugins/bootstrap/css/bootstrap-responsive.min.css",
                                        "~/Content/1/plugins/font-awesome/css/font-awesome.min.css",
                                        "~/Content/1/plugins/uniform/css/uniform.default.css",
                                        "~/Content/1/plugins/bootstrap-tag/css/bootstrap-tag.css",
                                        "~/Content/1/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.css",
                                        "~/Content/1/plugins/fancybox/source/jquery.fancybox.css",
                                        "~/Content/1/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.css",
                                        "~/Content/1/plugins/jquery-file-upload/css/jquery.fileupload-ui.css"

                                        ));
            bundles.Add(new ScriptBundle("~/Content/1/scripts/").Include(
                             "~/Content/1/scripts/app.js"
                             
                            ));
            bundles.Add(new ScriptBundle("~/Content/1/plugins/").Include(
                            "~/Content/1/plugins/breakpoints/breakpoints.js",
                            "~/Content/1/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                            "~/Content/1/plugins/jquery.blockui.js",
                            "~/Content/1/plugins/jquery.cookie.js",
                            "~/Content/1/plugins/uniform/jquery.uniform.min.js",
                            "~/Content/1/plugins/bootstrap-tag/js/bootstrap-tag.js",
                            "~/Content/1/plugins/fancybox/source/jquery.fancybox.pack.js",
                            "~/Content/1/plugins/bootstrap-wysihtml5/wysihtml5-0.3.0.js",
                            "~/Content/1/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.js",
                            "~/Content/1/plugins/bootstrap/css/bootstrap.min.css"

                            ));

            #endregion

            #region Template 2

            bundles.Add(new StyleBundle("~/Content/2/css/").Include(
                            "~/Content/2/css/esri.css",
                            "~/Content/2/css/style.css",
                            "~/Content/2/css/style-metro.css",
                            "~/Content/2/css/style-responsive.css",
                            "~/Content/2/css/pages/inbox.css",
                            "~/Content/2/css/autocomplete-ui-input.css",
                            "~/Content/2/css/flags.css",
                            "~/Content/2/css/flags.min.css"

                            ));

            bundles.Add(new StyleBundle("~/Content/2/plugins/").Include(
                                    "~/Content/2/plugins/bootstrap/css/bootstrap.min.css",
                                    "~/Content/2/plugins/bootstrap/css/bootstrap-responsive.min.css",
                                    "~/Content/2/plugins/font-awesome/css/font-awesome.min.css",
                                    "~/Content/2/plugins/uniform/css/uniform.default.css",
                                    "~/Content/2/plugins/bootstrap-tag/css/bootstrap-tag.css",
                                    "~/Content/2/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.css",
                                    "~/Content/2/plugins/fancybox/source/jquery.fancybox.css",
                                    "~/Content/2/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.css",
                                    "~/Content/2/plugins/jquery-file-upload/css/jquery.fileupload-ui.css"

                                    ));
            bundles.Add(new ScriptBundle("~/Content/2/plugins/").Include(
                           "~/Content/2/plugins/breakpoints/breakpoints.js",
                           "~/Content/2/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                           "~/Content/2/plugins/jquery.blockui.js",
                           "~/Content/2/plugins/jquery.cookie.js",
                           "~/Content/2/plugins/uniform/jquery.uniform.min.js",
                           "~/Content/2/plugins/bootstrap-tag/js/bootstrap-tag.js",
                           "~/Content/2/plugins/fancybox/source/jquery.fancybox.pack.js",
                           "~/Content/2/plugins/bootstrap-wysihtml5/wysihtml5-0.3.0.js",
                           "~/Content/2/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.js"
                         

                           ));


            bundles.Add(new ScriptBundle("~/Content/2/scripts/").Include(
                           "~/Content/2/scripts/app.js",
                           "~/Content/2/scripts/menu-action.js",
                            "~/Content/2/scripts/jquery.slimscroll.js",
                            "~/Content/2/scripts/jquery.slimscroll.min.js"
                           ));








            #endregion

            BundleTable.EnableOptimizations = false;

        }
    }
}
