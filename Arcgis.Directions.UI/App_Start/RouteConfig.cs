using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Arcgis.Directions.UI
{
    public class RouteConfig
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
