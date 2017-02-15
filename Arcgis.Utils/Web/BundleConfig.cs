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
                                        "~/assets/plugins/bootstrap/css/bootstrap.min.css",
                                        "~/assets/plugins/bootstrap/css/bootstrap-responsive.min.css",
                                        "~/assets/plugins/font-awesome/css/font-awesome.min.css",
                                        "~/assets/css/style-metro.css",
                                        "~/assets/css/style.css",
                                        "~/assets/css/style-responsive.css",
                                        "~/assets/css/themes/default.css",
                                        "~/assets/plugins/uniform/css/uniform.default.css",
                                        "~/assets/plugins/bootstrap-tag/css/bootstrap-tag.css",
                                        "~/assets/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.css",
                                        "~/assets/plugins/fancybox/source/jquery.fancybox.css",
                                        "~/assets/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.css",
                                        "~/assets/plugins/jquery-file-upload/css/jquery.fileupload-ui.css",
                                        "~/assets/css/pages/inbox.css"
                                        ));

            bundles.Add(new ScriptBundle("~/assets/scripts/").Include(
                            "~/assets/plugins/breakpoints/breakpoints.js",
                            "~/assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                            "~/assets/plugins/jquery.blockui.js",
                            "~/assets/plugins/jquery.cookie.js",
                            "~/assets/plugins/uniform/jquery.uniform.min.js",
                            "~/assets/plugins/bootstrap-tag/js/bootstrap-tag.js",
                            "~/assets/plugins/fancybox/source/jquery.fancybox.pack.js",
                            "~/assets/plugins/bootstrap-wysihtml5/wysihtml5-0.3.0.js",
                            "~/assets/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.js",
                            "~/assets/scripts/app.js"
                            ));

            #endregion

            #region Template 2

            bundles.Add(new StyleBundle("~/Content/2/css/").Include(
                            "~/Content/2/css/esri.css",
                            "~/Content/2/css/style.css"
                            ));

            #endregion

            BundleTable.EnableOptimizations = false;

        }
    }
}
