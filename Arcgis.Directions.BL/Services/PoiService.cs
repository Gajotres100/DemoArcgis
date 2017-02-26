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

        public GetPOIVM GetStartupData()
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
                logger.Error("error = " + ex);
                return null;
            }
        }

        #endregion

    }
}