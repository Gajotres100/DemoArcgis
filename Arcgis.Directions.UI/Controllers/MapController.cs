using Arcgis.Directions.BL.Services;
using Arcgis.Directions.BL.SSOAuth;
using Arcgis.Directions.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoArcgis.Controllers
{
    public class MapController : BaseController
    {
        PoiService _poiService;
        // GET: Map
        public ActionResult GroupPoi()
        {
            var user = new UserData();
            user = Session[nameof(UserData)] as UserData;
            var userID = 0;
            int.TryParse(user.UserID, out userID);

            var vm = new GetGroupPOIVM();
            _poiService = new PoiService();
            vm = _poiService.GetPoiGroups(userID);            
            return View(vm);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetPoiGroupList(string lang)
        {
            if (string.IsNullOrEmpty(lang)) return null;
            var vm = new GetGroupPOIVM();
            _poiService = new PoiService();
            vm = _poiService.GetAllPois(lang.Split(',').ToList());
            return Json(vm.ClusterPoiList, JsonRequestBehavior.AllowGet);
        }
    }
}