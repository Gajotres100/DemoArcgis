using Arcgis.Directions.BL.Services;
using Arcgis.Directions.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arcgis.Directions.UI.Controllers
{
    public class HomeController : Controller
    {
        
        PoiService _poiService;

        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult GetPoiList(string keywords)
        {
            var vm = new GetPOIVM();
            vm = _poiService.GetAvailablePoi(keywords);
            return Json(vm, JsonRequestBehavior.AllowGet);
        }
    }
}