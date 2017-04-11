using Arcgis.Directions.BL.SSOAuth;
using Arcgis.Directions.Data.Repository.Poi;
using Arcgis.Directions.VM;
using Arcgis.Directions.Data.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Arcgis.Directions.BL.Services
{
    public class PoiService
    {
        readonly
        #region Variables

        PoiRepository _poiRepository;
        static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors
        public PoiService()
        {
            _poiRepository = new PoiRepository();
        }
        #endregion

        #region Methods

        public GetPOIVM GetAvailablePoiByDescription(string keyword, int userID)
        {
            try
            {
                var item = new GetPOIVM();
                item.CusPoiList = _poiRepository.GetAvailablePoiByDescriptionOracle(keyword, userID);
                return item;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return null;
            }

        }

        public GetPOIVM GetPoiByID(int Id)
        {
            try
            {
                var item = new GetPOIVM();
                item.CusPoi = _poiRepository.GetPoiByIDOracle(Id);

                return item;
            }
            catch (Exception e)
            {
                logger.Error("error = " + e);
                return null;
            }
        }

        public GetPOIVM GetStartupData(int userID)
        {
            try
            {                
                
                var ssoOvreiden = false;
                bool.TryParse(ConfigurationManager.AppSettings[@"SSOveriden"], out ssoOvreiden);

                var item = new GetPOIVM();
                item.LanguageList = _poiRepository.GetLanguagesOracle();
                if (!ssoOvreiden)
                {
                    var applications = GetApplicationList();
                    var app = new List<Application>();
                    app = applications.Select(x => new Application
                    {
                        Name = x.Name,
                        Url = x.URL
                    }).ToList();
                    item.Applications = app;
                }
                item.GroupPoiList = _poiRepository.GetPoiGroups(userID);
                item.RouteDataList =  _poiRepository.GetRoutesForUser(userID);
                return item;
            }
            catch (Exception e)
            {
                
                logger.Error("error = " + e);
                return null;
            }
        }

        public UserData ValidateUser(string authToken)
        {
            try
            {
                string headerHTML = "";
                string footerHTML = "";
                string sessionCulture;
                string newSSOAuthToken;
                ArrayOfString allowedApplicationIDs;
                Message[] messageDataList;
                string roleID;
                UserData userData;
                UserData masterUserData;
                CompanyData companyData;

                if (!string.IsNullOrEmpty(authToken))
                {
                    using (SSOAuthSoapClient proxy = new SSOAuthSoapClient())
                    {
                        var loginOk = proxy.VerifyTokenAndGetNew(GetSSOAuthData(ConfigurationManager.AppSettings["ApplicationID"], authToken), out sessionCulture, out newSSOAuthToken, out allowedApplicationIDs, out headerHTML, out footerHTML, out messageDataList, out roleID, out userData, out masterUserData, out companyData);
                        if (loginOk) return userData;

                        throw new Exception("Invalid AuthToken");
                    }
                }
                throw new Exception("AuthToken is null");
            }
            catch (Exception ex)
            {

                logger.Error("error = " + ex);
                return null;
            }

        }

        SSOAuthData GetSSOAuthData(string applicationID, string ssoAuthToken)
        {
            try
            {
                var ssoAuthData = new SSOAuthData
                {
                    ApplicationID = applicationID,
                    IPAddress = ConfigurationManager.AppSettings["IPAddress"],
                    SSOToken = ssoAuthToken
                };

                return ssoAuthData;
            }
            catch (Exception e)
            {
                logger.Error("error = " + e);
                return null;
            }
        }

        public List<ApplicationData> GetApplicationList()
        {
            try
            {

                using (SSOAuthSoapClient proxy = new SSOAuthSoapClient())
                {
                    return proxy.GetAllApplicationsData(ConfigurationManager.AppSettings["LanguageCode"]).ToList();
                }

            }
            catch (Exception ex)
            {
                ApplicationData app1 = new ApplicationData();
                app1.Name = "App1";
                app1.URL = "http://www.google.com";

                ApplicationData app2 = new ApplicationData();
                app2.Name = "App2";
                app2.URL = "http://www.google.com";

                ApplicationData app3 = new ApplicationData();
                app3.Name = "App3";
                app3.URL = "http://www.google.com";

                var a = new List<ApplicationData>();
                a.Add(app1);
                a.Add(app2);
                a.Add(app3);

                var app = new List<ApplicationData>();
                app = a.Select(x => new ApplicationData
                {
                    Name = x.Name,
                    URL = x.URL
                }).ToList();
               
                return app;
                //logger.Error("error = " + ex);
                //return null;
            }
        }

        public void DeleteRoute(int routeID)
        {
            try
            {
                _poiRepository.DeleteRoute(routeID);
                
            }
            catch (Exception e)
            {
                logger.Error("error = " + e);                
            }
        }

        public GetGroupPOIVM GetAllPois(List<string> GroupID)
        {
            try
            {
                var item = new GetGroupPOIVM();
                item.ClusterPoiList = _poiRepository.GetAllPoisByGroupID(GroupID.Select(int.Parse).ToList());
                return item;
            }
            catch (Exception e)
            {
                logger.Error("error = " + e);
                return null;
            }
        }

        public GetGroupPOIVM GetPoiGroups(int userID)
        {
            try
            {
                var item = new GetGroupPOIVM();
                item.GroupPoiList = _poiRepository.GetPoiGroups(userID);
                return item;
            }
            catch (Exception e)
            {
                logger.Error("error = " + e);
                return null;
            }
        }

        public int SaveRoute(int userID, string routeData, string routeName, bool optimalRoute, bool returnToStart)
        {
            try
            {
                return _poiRepository.SaveRoute(userID, routeData, routeName, optimalRoute, returnToStart);
            }
            catch (Exception e)
            {
                return 0;
                logger.Error("error = " + e);
            }
        }

        public List<RouteData> GetRoutesForUser(int userID)
        {
            try
            {                
                return _poiRepository.GetRoutesForUser(userID);
            }
            catch (Exception e)
            {
                logger.Error("error = " + e);
                return null;
            }
        }

        public GetPOIVM GetRoutesByRouteId(int ID)
        {
            try
            {
                var item = new GetPOIVM();
                item.RouteData = _poiRepository.GetRoutesByRouteId(ID);
                return item;
            }
            catch (Exception e)
            {
                logger.Error("error = " + e);
                return null;
            }
        }

        #endregion

    }
}