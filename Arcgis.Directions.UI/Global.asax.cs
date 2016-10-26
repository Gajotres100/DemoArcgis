using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Arcgis.Directions.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Container _container;

        public static Container InjectorContainer
        {
            get { return _container; }
            private set { _container = value; }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
