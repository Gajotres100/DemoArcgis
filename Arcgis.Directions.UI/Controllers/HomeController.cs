using Arcgis.Directions.BL.Services;
using Arcgis.Directions.VM;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

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
            if (Session[@"UserData"] == null)
                return Redirect(ConfigurationManager.AppSettings[@"LoginRedirect"]);

            var vm = GetPois();
            return View(vm);
        }

        public ActionResult Login()
        {
            if (Request.QueryString[@"user_id"] != null && Request.QueryString[@"username"] != null)
            {
                var userID = 0;
                int.TryParse(Request.QueryString[@"user_id"], out userID);
                var user = new User
                {
                    UserID = userID,
                    Username = Request.QueryString[@"username"]
                };
                Session[@"UserData"] = user;
                return RedirectToAction(@"Index", @"Home");
            }

            var vm = GetPois();
            return View(vm);
        }

        private GetPOIVM GetPois()
        {
            var lang = (string)ControllerContext.RouteData.Values[@"lang"];
            var vm = new GetPOIVM();
            _poiService = new PoiService();
            vm = _poiService.GetLanguages();
            var defaultLang = vm.LanguageList.FirstOrDefault(l => l.Name.Equals(lang));
            if (defaultLang == null) defaultLang = vm.LanguageList.FirstOrDefault();
            vm.Langugae = defaultLang;
            return vm;
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
            var user = new User();
            user = Session[@"UserData"] as User;
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