using Arcgis.Directions.BL.Services;
using Arcgis.Directions.VM;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Arcgis.Directions.UI.Controllers
{
    public class HomeController : Controller
    {
        
        PoiService _poiService;
        public HomeController()
        {
            
        }     


        public ActionResult Index()
        {
            if (Session["UserData"] == null)
                return Redirect(ConfigurationManager.AppSettings["LoginRedirect"]); 

            string lang = (string)this.ControllerContext.RouteData.Values["lang"];           
            var vm = new GetPOIVM();
            _poiService = new PoiService();
            vm = _poiService.GetLanguages();
            var defaultLang = vm.LanguageList.Where(l => l.Name.Equals(lang)).FirstOrDefault();
            if(defaultLang == null) defaultLang = vm.LanguageList.FirstOrDefault();
            vm.Langugae = defaultLang;
            return View(vm);
        }

        public ActionResult Login()
        {
            if (Request.QueryString["user_id"] != null && Request.QueryString["username"] != null)
            {
                int userID = 0;
                int.TryParse(Request.QueryString["user_id"], out userID);
                User user = new User
                {
                    UserID = userID,
                    Username = Request.QueryString["username"]
                };
                Session["UserData"] = user;
                return RedirectToAction("Index", "Home");
            }

            string lang = (string)this.ControllerContext.RouteData.Values["lang"];
            var vm = new GetPOIVM();
            _poiService = new PoiService();
            vm = _poiService.GetLanguages();
            var defaultLang = vm.LanguageList.Where(l => l.Name.Equals(lang)).FirstOrDefault();
            if (defaultLang == null) defaultLang = vm.LanguageList.FirstOrDefault();
            vm.Langugae = defaultLang;
            return View(vm);
        }


        public ActionResult Error()
        {            
            return View();
        }
        


        [HttpPost]
        public JsonResult GetPoiList(string keywords)
        {
            var vm = new GetPOIVM();
            _poiService = new PoiService();
            User user = new User();
            user = Session["UserData"] as User;
            vm = _poiService.GetAvailablePoiByDescription(keywords, user.UserID);
            return Json(vm.CusPoiList != null ? vm.CusPoiList : null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPoiByID(int id)
        {
            var vm = new GetPOIVM();
            _poiService = new PoiService();
            vm = _poiService.GetPoiByID(id);
            return Json(vm.CusPoi != null ? vm.CusPoi : null, JsonRequestBehavior.AllowGet);
        }
    }
}