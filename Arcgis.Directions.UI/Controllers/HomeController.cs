using Arcgis.Directions.BL.Services;
using Arcgis.Directions.VM;
using log4net;
using System;
using System.Collections.Generic;
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
            return View();
        }


        [HttpPost]
        public JsonResult GetPoiList(string keywords)
        {
            var vm = new GetPOIVM();
            _poiService = new PoiService();
            vm = _poiService.GetAvailablePoiByDescription(keywords);
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