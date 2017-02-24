using Arcgis.Directions.BL.Services;
using Arcgis.Directions.BL.SSOAuth;
using Arcgis.Directions.VM;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Arcgis.Directions.UI.Controllers
{
    public class HomeController : Controller
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
                return RedirectToAction(@"Index", @"Home");
            }
            else if (Session[nameof(UserData)] == null)
                return Redirect(ConfigurationManager.AppSettings[@"LoginRedirect"]);

            var vm = GetPois();
            return View(vm);
        }

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
                return RedirectToAction(@"Index", @"Home");
            }
            else if (!string.IsNullOrEmpty(authToken))
            {
                _poiService = new PoiService();
                var user = _poiService.ValidateUser(authToken);
                Session[nameof(UserData)] = user;
            }
            else
            {
                //Kada prodati SSO ovo odkomentirati potto će biti beskonačna petlja
                //return Redirect(ConfigurationManager.AppSettings[@"LoginRedirect"]);
            }

            var vm = GetPois();
            return View(vm);
        }

        GetPOIVM GetPois()
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
            var user = new UserData();
            user = Session[nameof(UserData)] as UserData;
            var userID = 0;
            int.TryParse(user.UserID, out userID);
            vm = _poiService.GetAvailablePoiByDescription(keywords, userID);
            return Json(vm?.CusPoiList ?? null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPoiByID(int id)
        {
            var vm = new GetPOIVM();
            _poiService = new PoiService();
            vm = _poiService.GetPoiByID(id);
            return Json(vm?.CusPoi ?? null, JsonRequestBehavior.AllowGet);
        }
    }
}