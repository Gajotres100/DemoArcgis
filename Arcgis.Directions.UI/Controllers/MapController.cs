using Arcgis.Directions.BL.Services;
using Arcgis.Directions.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoArcgis.Controllers
{
    public class MapController : Controller
    {
        PoiService _poiService;
        // GET: Map
        public ActionResult GroupPoi()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetPoiGroupList()
        {
            var vm = new GetGroupPOIVM();
            _poiService = new PoiService();
            vm = _poiService.GetAllPois();
            return Json(vm.GroupPoiList, JsonRequestBehavior.AllowGet);
        }
    }



    //var testlist = new List<Test>();
    //var a = new Test
    //{
    //    caption = "another crappy day at work...",
    //    full_name = "gino beltran",
    //    image = "https://distilleryimage11.instagram.com/231895caaf2211e19dc71231380fe523_6.jpg",
    //    lat = "33.552143096",
    //    lng = "-117.776512145",
    //    link = "https://instagr.am/p/Lfz0O-Io5_/"
    //};
    //testlist.Add(a);
    //a = new Test
    //{
    //    caption = "another crappy day at work...",
    //    full_name = "gino beltran",
    //    image = "https://distilleryimage4.instagram.com/3c82fe5caf2111e19dc71231380fe523_6.jpg",
    //    lat = "33.552143096",
    //    lng = "-117.776512145",
    //    link = "https://instagr.am/p/Lfz0O-Io5_/"
    //};
    //testlist.Add(a);

}