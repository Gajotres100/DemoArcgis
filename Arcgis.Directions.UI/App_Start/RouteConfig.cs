using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Arcgis.Directions.UI
{
#pragma warning disable RECS0014 // If all fields, properties and methods members are static, the class can be made static.
    public class RouteConfig
#pragma warning restore RECS0014 // If all fields, properties and methods members are static, the class can be made static.
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute(@"{resource}.axd/{*pathInfo}");

            #region HomeRoute

            routes.MapRoute(
                name: @"HomeRoute",
                url: @"{lang}/{controller}/{action}",
                defaults: new { lang = String.Empty, controller = @"Home", action = @"Index" },
                constraints: new { lang = @"EN|HR" }
            );

            #endregion

            #region DefaultRoute

            routes.MapRoute(
                name: @"Default",
                url: @"{controller}/{action}",
                defaults: new
                {
                    controller = @"Home",
                    action = @"Index"
                }
            );

            #endregion

            #region NoRoute

            routes.MapRoute(
                @"NotFound",
                @"{*url}",
                new { controller = @"Error", action = @"PageNotFound" }
            );

            #endregion

            routes.MapRoute(
                @"Error",
                @"{*catchall}",
                new { controller = @"Home", action = @"PageNotFound" }
            );

        }
    }
}
