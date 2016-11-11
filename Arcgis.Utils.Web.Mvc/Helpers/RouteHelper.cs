using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Arcgis.Utils.Web.Mvc.Helpers
{
    public class RouteHelper
    {
        #region Classes

        public class RouteSegment
        {
            public static readonly string Instance = "instance";
            public static readonly string Portal = "portal";
            public static readonly string Language = "lang";
            public static readonly string Area = "area";
            public static readonly string Controller = "controller";
            public static readonly string Action = "action";
            public static readonly string Id = "id";
        }

        public class Routes
        {
            public static readonly string InstanceError = "InstanceError";
            public static readonly string JavascriptError = "JavascriptError";
            public static readonly string ForbiddenError = "ForbiddenError";
            public static readonly string UnauthorizedError = "UnauthorizedError";
            public static readonly string NotFoundError = "NotFoundError";
            public static readonly string NotSupportedError = "NotSupportedError";
            public static readonly string Redirect = "RedirectRoute";
            public static readonly string Catchall = "Catchall";
            public static readonly string HomeController = "HomeRoute";
            public static readonly string ServiceController = "ServiceRoute";
            public static readonly string ValidationController = "ValidationRoute";
            public static readonly string SamlController = "SamlRoute";

            /// <summary>
            /// External route
            /// </summary>
            public static readonly string WidgetController = "WidgetRoute";
        }

        public class SharedResources
        {
            public class Controllers
            {
                public static readonly string ErrorController = "Error";
                public static readonly string HomeController = "Home";
                public static readonly string SamlController = "Saml";
                public static readonly string ServiceController = "Service";
                public static readonly string ValidationController = "Validation";
            }

            public class Actions
            {
                public static readonly string ConsumerService = "ConsumerService";
                public static readonly string Error = "Error";
                public static readonly string JavascriptError = "JavascriptError";
                public static readonly string Forbidden = "Forbidden";
                public static readonly string Unauthorized = "Unauthorized";
                public static readonly string NotFound = "NotFound";
                public static readonly string NotSupported = "NotSupported";
            }
        }

        public class ExternalResources
        {
            public class Controllers
            {
                public static readonly string WidgetController = "Widget";
            }

            public class ControllerNamespaces
            {
                public static readonly string WidgetController = "ComProvis.Widgets.Web.Controllers";
            }
        }

        #endregion

        #region Constants

        public static readonly string RouteName = "RouteName";
        public static readonly string DefaultControllerName = "Home";
        public static readonly string DefaultActionName = "Index";

        #endregion

        #region RegisterRoutes

        public static void RegisterRoutes(RouteCollection routes, string controllersNamespace, string portalName, string defaultInstanceName)
        {
            #region IgnoreRoutes

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            #endregion

            #region ErrorRoutes

            routes.MapRoute(
                name: RouteHelper.Routes.InstanceError,
                url: portalName + "/" + RouteHelper.SharedResources.Actions.Error,
                defaults: new { controller = RouteHelper.SharedResources.Controllers.ErrorController, action = RouteHelper.SharedResources.Actions.Error },
                namespaces: new string[] { controllersNamespace }
            ).DataTokens.Add(RouteName, RouteHelper.Routes.InstanceError);

            routes.MapRoute(
                name: RouteHelper.Routes.JavascriptError,
                url: portalName + "/" + RouteHelper.SharedResources.Actions.JavascriptError,
                defaults: new { controller = RouteHelper.SharedResources.Controllers.ErrorController, action = RouteHelper.SharedResources.Actions.JavascriptError },
                namespaces: new string[] { controllersNamespace }
            ).DataTokens.Add(RouteName, RouteHelper.Routes.JavascriptError);

            routes.MapRoute(
                RouteHelper.Routes.ForbiddenError,
                "{instance}/" + portalName + "/{lang}/" + RouteHelper.SharedResources.Controllers.ErrorController + "/" + RouteHelper.SharedResources.Actions.Forbidden,
                new { controller = RouteHelper.SharedResources.Controllers.ErrorController, action = RouteHelper.SharedResources.Actions.Forbidden },
                new { instance = "[a-zA-Z]+", lang = "[a-zA-Z]{2}" },
                new string[] { controllersNamespace }
            ).DataTokens.Add(RouteName, RouteHelper.Routes.ForbiddenError);

            routes.MapRoute(
                RouteHelper.Routes.UnauthorizedError,
                "{instance}/" + portalName + "/{lang}/" + RouteHelper.SharedResources.Controllers.ErrorController + "/" + RouteHelper.SharedResources.Actions.Unauthorized,
                new { controller = RouteHelper.SharedResources.Controllers.ErrorController, action = RouteHelper.SharedResources.Actions.Unauthorized },
                new { instance = "[a-zA-Z]+", lang = "[a-zA-Z]{2}" },
                new string[] { controllersNamespace }
            ).DataTokens.Add(RouteName, RouteHelper.Routes.UnauthorizedError);

            routes.MapRoute(
                RouteHelper.Routes.NotFoundError,
                "{instance}/" + portalName + "/{lang}/" + RouteHelper.SharedResources.Controllers.ErrorController + "/" + RouteHelper.SharedResources.Actions.NotFound,
                new { controller = RouteHelper.SharedResources.Controllers.ErrorController, action = RouteHelper.SharedResources.Actions.NotFound },
                new { instance = "[a-zA-Z]+", lang = "[a-zA-Z]{2}" },
                new string[] { controllersNamespace }
            ).DataTokens.Add(RouteName, RouteHelper.Routes.NotFoundError);

            routes.MapRoute(
                RouteHelper.Routes.NotSupportedError,
                "{instance}/" + portalName + "/{lang}/" + RouteHelper.SharedResources.Controllers.ErrorController + "/" + RouteHelper.SharedResources.Actions.NotSupported,
                new { controller = RouteHelper.SharedResources.Controllers.ErrorController, action = RouteHelper.SharedResources.Actions.NotSupported },
                new { instance = "[a-zA-Z]+", lang = "[a-zA-Z]{2}" },
                new string[] { controllersNamespace }
            ).DataTokens.Add(RouteName, RouteHelper.Routes.NotSupportedError);

            #endregion

            #region IncompleteRoutes

            // This route catches incomplete URL-s and redirects them to the home page.
            // Request checks in global.asax are responsible for stopping requests without language segment
            routes.MapRoute(
                name: RouteHelper.Routes.Redirect,
                url: "{instance}/{portal}/{lang}/{controller}/{action}/{id}",
                defaults: new { instance = defaultInstanceName, portal = portalName, lang = String.Empty, controller = RouteHelper.DefaultControllerName, action = RouteHelper.DefaultActionName, id = UrlParameter.Optional },
                constraints: new { portal = portalName, controller = RouteHelper.DefaultControllerName },
                namespaces: new string[] { controllersNamespace }
            ).DataTokens.Add(RouteName, RouteHelper.Routes.Redirect);

            #endregion

            #region HomeRoute

            routes.MapRoute(
                name: RouteHelper.Routes.HomeController,
                url: "{instance}/{portal}/{lang}/{controller}/{action}",
                defaults: new { instance = defaultInstanceName, portal = portalName, lang = String.Empty, action = RouteHelper.DefaultActionName },
                constraints: new { portal = portalName, controller = RouteHelper.SharedResources.Controllers.HomeController }
            );

            #endregion

            #region ServiceRoute

            routes.MapRoute(
                name: RouteHelper.Routes.ServiceController,
                url: "{instance}/{portal}/{lang}/{controller}/{action}",
                defaults: new { instance = defaultInstanceName, portal = portalName, lang = String.Empty, action = RouteHelper.DefaultActionName },
                constraints: new { portal = portalName, controller = RouteHelper.SharedResources.Controllers.ServiceController }
            );

            #endregion

            #region ValidationRoute

            routes.MapRoute(
                name: RouteHelper.Routes.ValidationController,
                url: "{instance}/{portal}/{lang}/{controller}/{action}",
                defaults: new { instance = defaultInstanceName, portal = portalName, lang = String.Empty, action = RouteHelper.DefaultActionName },
                constraints: new { portal = portalName, controller = RouteHelper.SharedResources.Controllers.ValidationController }
            );

            #endregion

            #region SamlRoute

            routes.MapRoute(
                name: RouteHelper.Routes.SamlController,
                url: "{instance}/{portal}/{lang}/{controller}/{action}",
                defaults: new { instance = defaultInstanceName, portal = portalName, lang = String.Empty, action = RouteHelper.SharedResources.Actions.ConsumerService },
                constraints: new { portal = portalName, controller = RouteHelper.SharedResources.Controllers.SamlController }
            );

            #endregion

            #region WidgetsRoute

            routes.MapRoute(
                name: RouteHelper.Routes.WidgetController,
                url: "{instance}/{portal}/{lang}/{controller}/{action}",
                defaults: new { instance = defaultInstanceName, portal = portalName, lang = String.Empty, action = String.Empty },
                constraints: new { portal = portalName, controller = RouteHelper.ExternalResources.Controllers.WidgetController },
                namespaces: new string[] { RouteHelper.ExternalResources.ControllerNamespaces.WidgetController }
            );

            #endregion

            #region CatchAllRoute

            routes.MapRoute(
                name: RouteHelper.Routes.Catchall,
                url: "{instance}/{portal}/{lang}/{*suffix}",
                defaults: new { controller = "Error", action = "Error" } //Staviti not found action
            ).DataTokens.Add(RouteName, RouteHelper.Routes.Catchall);

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portalName"></param>
        /// <returns></returns>
        public static string GetInstanceSafeErrorUrl(string portalName)
        {
            return string.Format("~/{0}/Error", portalName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultFallbackLanguageName"></param>
        /// <param name="getDefaultLanguage"></param>
        /// <returns></returns>
        public static string GetLanguageSafeErrorUrl(string defaultFallbackLanguageName, Func<string> getDefaultLanguage)
        {
            string languageSegment = getDefaultLanguage() ?? defaultFallbackLanguageName;

            return string.Format("~/{0}/{1}/{2}/{3}/{4}/NotFound", RequestState.Request.Instance.Name, RequestState.PortalName, languageSegment, RequestState.Request.ControllerAction.AreaName, RouteHelper.SharedResources.Controllers.ErrorController);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultFallbackLanguageName"></param>
        /// <param name="getDefaultLanguage"></param>
        /// <returns></returns>
        public static string GetDefaultLanguageHomepageUrl(string defaultFallbackLanguageName, Func<string> getDefaultLanguage)
        {
            string languageSegment = getDefaultLanguage() ?? defaultFallbackLanguageName;

            return string.Format("~/{0}/{1}/{2}/{3}", RequestState.Request.Instance.Name, RequestState.PortalName, languageSegment, RequestState.Request.ControllerAction.AreaName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultFallbackLanguageName"></param>
        /// <param name="getDefaultLanguage"></param>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public static string GetDefaultAreaHomepageUrl(string defaultFallbackLanguageName, Func<string> getDefaultLanguage, string areaName)
        {
            string languageSegment = getDefaultLanguage() ?? defaultFallbackLanguageName;

            return string.Format("~/{0}/{1}/{2}/{3}", RequestState.Request.Instance.Name, RequestState.PortalName, languageSegment, areaName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultFallbackLanguageName"></param>
        /// <param name="languageSegment"></param>
        /// <returns></returns>
        public static string GetForbiddenErrorUrl(string defaultFallbackLanguageName, string languageSegment)
        {
            if (String.IsNullOrEmpty(languageSegment))
            {
                languageSegment = defaultFallbackLanguageName;
            }

            return string.Format("~/{0}/{1}/{2}/Error/Forbidden", RequestState.Request.Instance.Name, RequestState.PortalName, languageSegment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetAbsoluteRoutedUrl(string relativePath)
        {
            relativePath = relativePath.Substring(0, 1) == "/" ? relativePath.Remove(0, 1) : relativePath;
            string relativeUrl = string.Format("/{0}/{1}/{2}/{3}", RequestState.Request.Instance.Name, RequestState.PortalName, RequestState.Request.Language.LanguageCode, relativePath);
            return BaseUrlHelper.GetAbsoluteUrl(relativeUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetRoutedUrl(string area, string controller, string action)
        {
            string relativeUrl = string.Format("/{0}/{1}/{2}/{3}/{4}/{5}", RequestState.Request.Instance.Name, RequestState.PortalName, RequestState.Request.Language.LanguageCode, area, controller, action);
            return BaseUrlHelper.GetAbsoluteUrl(relativeUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetRoutedUrl(string controller, string action)
        {
            string relativeUrl = string.Format("/{0}/{1}/{2}/{3}/{4}", RequestState.Request.Instance.Name, RequestState.PortalName, RequestState.Request.Language.LanguageCode, controller, action);
            return BaseUrlHelper.GetAbsoluteUrl(relativeUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetValidationRoutedUrl(string action)
        {
            string relativeUrl = string.Format("/{0}/{1}/{2}/{3}/{4}", RequestState.Request.Instance.Name, RequestState.PortalName, RequestState.Request.Language.LanguageCode, RouteHelper.SharedResources.Controllers.ValidationController, action);
            return BaseUrlHelper.GetAbsoluteUrl(relativeUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static string GetRequestedPath(RouteData routeData)
        {
            string languageSegment = routeData.Values[RouteHelper.RouteSegment.Language] as string;
            string requestedAction = routeData.Values[RouteHelper.RouteSegment.Action] as string;
            string requestedController = routeData.Values[RouteHelper.RouteSegment.Controller] as string;
            string requestedArea = routeData.DataTokens[RouteHelper.RouteSegment.Area] as string;
            return string.Format("{0}/{1}/{2}", requestedArea, requestedController, requestedAction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string GetRequestedPath(RouteValueDictionary routeValues)
        {
            string languageSegment = routeValues[RouteHelper.RouteSegment.Language] as string;
            string requestedAction = routeValues[RouteHelper.RouteSegment.Action] as string;
            string requestedController = routeValues[RouteHelper.RouteSegment.Controller] as string;
            string requestedArea = routeValues[RouteHelper.RouteSegment.Area] as string;
            return string.Format("{0}/{1}/{2}", requestedArea, requestedController, requestedAction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeData"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static string GetRedirectActionSafePath(RouteData routeData, string actionName)
        {
            string instanceSegment = routeData.Values[RouteHelper.RouteSegment.Instance] as string;
            string languageSegment = routeData.Values[RouteHelper.RouteSegment.Language] as string;
            string requestedAction = routeData.Values[RouteHelper.RouteSegment.Action] as string;
            string requestedController = routeData.Values[RouteHelper.RouteSegment.Controller] as string;
            string requestedArea = routeData.DataTokens[RouteHelper.RouteSegment.Area] as string;

            if (routeData.Values.Count > 0)
            {
                return string.Format("~/{0}/{1}/{2}/Error/{3}", instanceSegment, RequestState.PortalName, languageSegment, actionName);
            }
            else
            {
                return string.Format("~/{0}/Error/{1}", RequestState.PortalName, actionName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static bool IsInstanceSafeRoute(RouteData routeData)
        {
            bool retVal = routeData != null &&
                routeData.DataTokens != null &&
                routeData.DataTokens.ContainsKey(RouteName) &&
                (
                    routeData.DataTokens[RouteHelper.RouteName].ToString() == RouteHelper.Routes.InstanceError ||
                    routeData.DataTokens[RouteHelper.RouteName].ToString() == RouteHelper.Routes.JavascriptError
                );

            return retVal;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static bool IsForbiddenErrorRoute(RouteData routeData)
        {
            bool retVal = routeData != null && routeData.DataTokens[RouteName] as string == RouteHelper.Routes.ForbiddenError;

            return retVal;
        }

        #endregion
    }
}
