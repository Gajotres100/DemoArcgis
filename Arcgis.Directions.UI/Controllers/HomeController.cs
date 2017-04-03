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

        public ActionResult ChangeLanguage(string lang) => Redirect($"~/{lang}");


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
            _poiService = new PoiService();
            var vm = _poiService.GetPoiByID(id);
            return Json(vm?.CusPoi ?? null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveRoute(string routeData)
        {
            
            var compresedString = CompressString(routeData);

            string path = @"d:\MyTest.txt";
                        
            System.IO.File.WriteAllText(path, compresedString);

            return Json(routeData, JsonRequestBehavior.AllowGet);
        }

        
        [ValidateInput(false)]
        public ActionResult GetRoute(string routeData)
        {
            var path = @"d:\MyTest.txt";
            var readText = System.IO.File.ReadAllText(path);

            JavaScriptSerializer j = new JavaScriptSerializer();
            j.MaxJsonLength = int.MaxValue;

            var jsonResult = Json(j.Deserialize(DecompressString(readText), typeof(object)), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}