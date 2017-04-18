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

namespace Arcgis.Directions.UI.Controllers
{
    public class HomeController : BaseController
    {


        PoiService _poiService;
        public ActionResult Index()
        {
            var ssoOvreiden = false;
            bool.TryParse(ConfigurationManager.AppSettings[@"SSOveriden"], out ssoOvreiden);
            if (ssoOvreiden && Session[nameof(UserData)] == null)
            {
                var user = new UserData
                {
                    UserID = ConfigurationManager.AppSettings[@"User_id"],
                    Username = ConfigurationManager.AppSettings[@"Username"]
                };
                Session[nameof(UserData)] = user;
                Session["Username"] = user.Username;
                return RedirectToAction(nameof(Index), @"Home");
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

            var vm = GetPois();
            return View(vm);
        }

        //public static int CurrentUICulture
        //{
        //    get
        //    {
        //        if (Thread.CurrentThread.CurrentUICulture.Name == "ro-RO")
        //            return 1;
        //        else if (Thread.CurrentThread.CurrentUICulture.Name == "de-DE")
        //            return 2;
        //        else
        //            return 0;
        //    }
        //    set
        //    {
        //        //
        //        // Set the thread's CurrentUICulture.
        //        //
        //        if (value == 1)
        //            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ro-RO");
        //        else if (value == 2)
        //            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
        //        else
        //            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        //        //
        //        // Set the thread's CurrentCulture the same as CurrentUICulture.
        //        //
        //        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
        //    }
        //}

        public ActionResult Login()
        {
            var authToken = Request.QueryString["SSO_AUTH_TOKEN"];
            var ssoOvreiden = false;
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
            if (!string.IsNullOrEmpty(authToken))
            {
                _poiService = new PoiService();
                var user = _poiService.ValidateUser(authToken);
                Session[nameof(UserData)] = user;
            }

            var vm = GetPois();
            return View(vm);
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