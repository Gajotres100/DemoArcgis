using Arcgis.Directions.BL.SSOAuth;
using Arcgis.Directions.UI.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DemoArcgis.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           // var controllerName = filterContext.Controller.GetType().Name;
           // var actionName = filterContext.ActionDescriptor.ActionName;

           // //SessionManager.Instance().GetUser()

           // if (Session[nameof(UserData)] == null && !actionName.Equals("Login"))
           // {
           //    // if ((!controllerName.Equals(typeof(HomeController).Name, StringComparison.InvariantCultureIgnoreCase)
           //   //  || !actionName.Equals("LogOn", StringComparison.InvariantCultureIgnoreCase)))
           //         filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
           //     { "Controller", "Home" },
           //     { "Action", "Index" }
           //});
           // }

            base.OnActionExecuting(filterContext);
        }
    }
}