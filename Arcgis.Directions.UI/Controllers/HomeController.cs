using Arcgis.Directions.BL.Services;
using Arcgis.Directions.BL.SSOAuth;
using Arcgis.Directions.VM;
using DemoArcgis.Controllers;
using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Dynamic;
using System.Threading;
using System.Globalization;
using log4net;
using System.Reflection;

namespace Arcgis.Directions.UI.Controllers
{
    public class HomeController : BaseController
    {

        static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        PoiService _poiService;
        public ActionResult Index()
        {
            var ssoOvreiden = false;
            bool.TryParse(ConfigurationManager.AppSettings[@"SSOveriden"], out ssoOvreiden);
            if (ssoOvreiden && Session[nameof(UserData)] == null)
            {
                var user1 = new UserData
                {
                    UserID = ConfigurationManager.AppSettings[@"User_id"],
                    Username = ConfigurationManager.AppSettings[@"Username"]
                };
                Session[nameof(UserData)] = user1;
                Session["Username"] = user1.Username;
                return RedirectToAction(nameof(Index), @"Home");
            }


            var user = new UserData();
            user = Session[nameof(UserData)] as UserData;
            if (!ssoOvreiden && user == null)
            {
                
                var authToken = Request.QueryString["SSO_AUTH_TOKEN"];
                logger.Info("VALIDATE USER0-->" + authToken );
                if (!string.IsNullOrEmpty(authToken))
                {
                    logger.Info("VALIDATE USER1");
                    _poiService = new PoiService();
                    user = _poiService.ValidateUser(authToken);
                    logger.Info("VALIDATE USER DONE1-->" + user.UserID);
                    if (user == null || user.UserID == null)
                        return Redirect(ConfigurationManager.AppSettings[@"LoginRedirect"]);
                    Session[nameof(UserData)] = user;
                    var vm1 = GetPois();
                    return View(vm1);
                    // return RedirectToAction(nameof(Index), @"Home");
                }
                else
                {
                    return Redirect(ConfigurationManager.AppSettings[@"LoginRedirect"]);
                }

            }

            var vm = GetPois();
            return View(vm);
        }

        public ActionResult Login()
        {
            var authToken = Request.QueryString["SSO_AUTH_TOKEN"];
            var ssoOvreiden = false;

            logger.Info("AUTH TOKEN--> " + authToken);
            bool.TryParse(ConfigurationManager.AppSettings[@"SSOveriden"], out ssoOvreiden);
            if (ssoOvreiden)
            {
                var user = new UserData
                {
                    UserID = ConfigurationManager.AppSettings[@"User_id"],
                    Username = ConfigurationManager.AppSettings[@"Username"]
                };
                Session[nameof(UserData)] = user;
                return RedirectToAction(nameof(Index), @"Home");
            }
            logger.Info("SSO OVERRIDE OFF");
            if (!string.IsNullOrEmpty(authToken))
            {
                logger.Info("VALIDATE USER");
                _poiService = new PoiService();
                var user = _poiService.ValidateUser(authToken);
                logger.Info("VALIDATE USER DONE-->" + user.UserID);
                Session[nameof(UserData)] = user;
                var vm = GetPois();
                return RedirectToAction(nameof(Index), @"Home");
            }
            else
            {
                return Redirect(ConfigurationManager.AppSettings[@"LoginRedirect"]);
            }







        }

        public ActionResult Logout()
        {
            if (Session[nameof(UserData)] != null) Session.Remove(nameof(UserData));
            return Redirect(ConfigurationManager.AppSettings[@"LoginRedirect"]);
        }

        public ActionResult ChangeLanguage(string lang)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            return Redirect($"~/{lang}");
        }


        GetPOIVM GetPois()
        {
            var lang = (string)ControllerContext.RouteData.Values[@"lang"];
            var vm = new GetPOIVM();

            _poiService = new PoiService();

            var user = new UserData();
            user = Session[nameof(UserData)] as UserData;
            var userID = 0;
            int.TryParse(user.UserID, out userID);

            vm = _poiService.GetStartupData(userID);
            var defaultLang = vm.LanguageList.FirstOrDefault(l => l.Name.Equals(lang));
            if (defaultLang == null) defaultLang = vm.LanguageList.FirstOrDefault();
            vm.Langugae = defaultLang;


            Thread.CurrentThread.CurrentUICulture = new CultureInfo(defaultLang.Code);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

            return vm;
        }

        public ActionResult Error() => View();

        [HttpPost]
        public JsonResult GetPoiList(string keywords)
        {
            var vm = new GetPOIVM();
            _poiService = new PoiService();
            var userID = GetUserIDFromSession();
            vm = _poiService.GetAvailablePoiByDescription(keywords, userID);
            return Json(vm?.CusPoiList ?? null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPoiByID(int id)
        {
            _poiService = new PoiService();
            var vm = _poiService.GetPoiByID(id);
            return Json(vm?.CusPoi ?? null, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetRouteData(int lang)
        {
            _poiService = new PoiService();
            var vm = _poiService.GetRoutesByRouteId(lang);
            return Json(vm?.RouteData ?? null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveRoute(dynamic routeData, string name, bool optimalRoute, bool returnToStart)
        {
            string jsonRouteData = Newtonsoft.Json.JsonConvert.SerializeObject(routeData);
            _poiService = new PoiService();
            var userID = GetUserIDFromSession();
            int routeID = _poiService.SaveRoute(userID, jsonRouteData, name, optimalRoute, returnToStart);
            return Json(routeID, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteRoute(int routeID)
        {
            _poiService = new PoiService();
            _poiService.DeleteRoute(routeID);
            return Json(routeID, JsonRequestBehavior.AllowGet);
        }

        private int GetUserIDFromSession()
        {
            var user = new UserData();
            user = Session[nameof(UserData)] as UserData;
            var userID = 0;
            int.TryParse(user.UserID, out userID);
            return userID;
        }

    }
}