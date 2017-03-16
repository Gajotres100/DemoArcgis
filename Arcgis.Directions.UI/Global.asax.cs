﻿using Arcgis.Utils.Web;
using log4net;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Arcgis.Directions.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static Container InjectorContainer { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            logger.Error(exception);
            //Server.ClearError();
            //Response.Redirect(@"/Home/Error");
        }
    }
}
