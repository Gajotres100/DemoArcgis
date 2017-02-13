using System.Web.Optimization;

namespace Arcgis.Utils.Web
{
    public class BundleConfig
    {
        /// <summary>
        /// Registers bundles with the application.
        /// </summary>
        /// <param name="bundles">The bundles to register.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/*.css"));
        }
    }
}
