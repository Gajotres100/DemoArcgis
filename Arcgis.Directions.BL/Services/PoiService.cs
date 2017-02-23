using Arcgis.Directions.BL.SSOAuth;
using Arcgis.Directions.Data.Repository.Poi;
using Arcgis.Directions.VM;
using log4net;
using System;
using System.Configuration;
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

        public GetPOIVM GetLanguages()
        {
            try
            {
                var item = new GetPOIVM();
                item.LanguageList = _poiRepository.GetLanguagesOracle();
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
            string headerHTML = "";
            string footerHTML = "";
            string sessionCulture;
            string newSSOAuthToken;
            SSOAuth.ArrayOfString allowedApplicationIDs;
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
                    else
                    {
                        logger.Error("error = login failed");
                        return null;
                    }
                }
            }
            else
            {
                logger.Error("error = token is not provided" );
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

        #endregion

    }
}
