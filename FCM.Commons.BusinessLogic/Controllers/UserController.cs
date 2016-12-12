#region Copyright (C) 2014, GDi Gisdata d.o.o., All rights reserved.

/*================================================================================================*\
*   Project Title:      FCM.Commons.BusinessLogic (Class Library)                                  *
*   Project Version:    1.0.0028.0                                                                 *
*   Company:            NETSinapsa & GDi Gisdata d.o.o.                                            *
*   Authors:            Petar Maletić (NETSinapsa)                                                 *
*   Contributors:       ----------------------------------                                         *
*   Copyright/TM:       Copyright (C) 2014, GDi Gisdata d.o.o. All rights reserved.                *
*   Information:        http://www.gisdata.hr, http://www.netsinapsa.hr                            *
*   Contact:            petar.maletic@netsinapsa.hr                                                *
\*================================================================================================*/

#endregion Copyright (C) 2014, GDi Gisdata d.o.o., All rights reserved.

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Services.Protocols;
using FCM.Commons.BusinessLogic.Entities;
using FCM.Commons.ConfigProvider;
using FCM.Commons.Constants;
using FCM.Commons.GlobalHelper;
using FCM.Commons.Utilities.Miscellaneous;
using FCM.Commons.Utilities.Text;
using FCM.Commons.Utilities.Web;
// SSOauth web service model references.
using SSOAuth_ApplicationData = FCM.Commons.BusinessLogic.SSO_SSOAuthWR.ApplicationData;
using SSOAuth_CompanyData = FCM.Commons.BusinessLogic.SSO_SSOAuthWR.CompanyData;
using SSOAuth_Message = FCM.Commons.BusinessLogic.SSO_SSOAuthWR.Message;
using SSOAuth_Role = FCM.Commons.BusinessLogic.SSO_SSOAuthWR.Role;
using SSOAuth_SSOAuthData = FCM.Commons.BusinessLogic.SSO_SSOAuthWR.SSOAuthData;
using SSOAuth_UserBasicData = FCM.Commons.BusinessLogic.SSO_SSOAuthWR.UserBasicData;
using SSOAuth_UserData = FCM.Commons.BusinessLogic.SSO_SSOAuthWR.UserData;
// WebAppsLauncher web service model references.
using WebApps_ApplicationRole = FCM.Commons.BusinessLogic.SSO_WebAppsLauncherWR.ApplicationRole;
using WebApps_CompanyData = FCM.Commons.BusinessLogic.SSO_WebAppsLauncherWR.CompanyData;
using WebApps_Message = FCM.Commons.BusinessLogic.SSO_WebAppsLauncherWR.Message;
using WebApps_SSOWebAppsLauncherAuthData = FCM.Commons.BusinessLogic.SSO_WebAppsLauncherWR.SSOWebAppsLauncherAuthData;
using WebApps_UserData = FCM.Commons.BusinessLogic.SSO_WebAppsLauncherWR.UserData;
// Database providers web service model references.
using Db_User = FCM.Commons.BusinessLogic.FCM_DbProvidersWR.UserModel;

#endregion Using

namespace FCM.Commons.BusinessLogic.Controllers
{
    // TODO: Implement SSO ChangeSessionCulture method!
    /// <summary>
    /// User business controller, handles all authentication, user and child user business logic.
    /// </summary>
    /// <remarks>
    /// Database logic is accessed via database providers web service methods and models. For 
    /// direct access change the database model references from database providers web service 
    /// to DataProvider.Model namespace and make direct calls to database providers methods.
    /// </remarks>
    public sealed class UserController : BaseBusinessLogic
    {
        #region Properties

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public long UserID
        {
            get
            {
                object obj = SessionUtils.GetValue(SessionHelper.BuildSessionKey("CurrentUserID"));
                return (obj == null) ? 0 : (long)obj;
            }
            set { SessionUtils.SetValue(SessionHelper.BuildSessionKey("CurrentUserID"), value); }
        }

        /// <summary>
        /// Gets or sets the SSO auth service token.
        /// </summary>
        /// <value>The SSO auth service token.</value>
        public string SSOAuthToken
        {
            get
            {
                object obj = SessionUtils.GetValue(SessionHelper.BuildSessionKey("SSOAuthToken"));
                return (obj == null) ? string.Empty : (string)obj;
            }
            set { SessionUtils.SetValue(SessionHelper.BuildSessionKey("SSOAuthToken"), value); }
        }

        /// <summary>
        /// Gets or sets the Web apps launcher service token.
        /// </summary>
        /// <value>The web apps launcher service token.</value>
        public string WebAppsToken
        {
            get
            {
                object obj = SessionUtils.GetValue(SessionHelper.BuildSessionKey("WebAppsToken"));
                return (obj == null) ? string.Empty : (string)obj;
            }
            set { SessionUtils.SetValue(SessionHelper.BuildSessionKey("WebAppsToken"), value); }
        }

        #endregion Properties

        #region Methods

        #region Child User

        // PUBLIC //

        /// <summary>
        /// Gets the child user.
        /// </summary>
        /// <param name="childUserID">The child user ID.</param>
        public User GetChildUser(long childUserID)
        {
            if (childUserID <= 0)
                return null;

            string cacheKey = CacheHelper.BuildCacheKey("ChildUser", childUserID);

            try
            {
                User childUser;

                if (FCMConfig.Data.EnableCache && CacheUtils.Contains(cacheKey))
                    childUser = (User)CacheUtils.GetItem(cacheKey);
                else
                {
                    Db_User dbChildUser = DbProvidersWS.GetChildUser(childUserID);
                    childUser = ReadChildUserFromDb(dbChildUser);

                    if (FCMConfig.Data.EnableCache)
                        CacheUtils.Add(cacheKey, childUser, null, DateTimeHelper.GetSvcProvDateTimeNow().AddSeconds(FCMConfig.Data.LongCacheDuration),
                                       TimeSpan.Zero,
                                       cacheNullObjects: FCMConfig.Data.CacheNullObjects);
                }

                return childUser;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, string.Format("childUserID:'{0}'", childUserID), ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the child users.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        public List<User> GetChildUsers(long userID)
        {
            if (userID <= 0)
                return null;

            try
            {
                Db_User[] dbChildUsers = DbProvidersWS.GetChildUsers(userID);
                List<User> childUsers = ReadChildUsersFromDb(dbChildUsers);

                return childUsers;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, string.Format("userID:'{0}', serviceType:'{1}'", userID), ex);
                return null;
            }
        }

        /// <summary>
        /// Synchronizes the child users.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="groupID">The group ID.</param>
        public void SynchronizeChildUsers(long userID, long groupID)
        {
            if (userID <= 0)
                return;

            try
            {
                string newSSOAuthToken;
                SSOAuth_UserBasicData[] userBasicDataList =
                    SSOAuthWS.GetGroupUsers(GetSSOAuthData(FCMConfig.Security.SSOApplicationID, SSOAuthToken),
                                            out newSSOAuthToken);

                SSOAuthToken = newSSOAuthToken;

                if (userBasicDataList == null)
                    return;

                foreach (SSOAuth_UserBasicData userBasicData in userBasicDataList)
                {
                    if (userBasicData.RoleID.ToLower() != "child")
                        continue;

                    bool synchronised =
                        DbProvidersWS.SynchronizeChildUser(ValidationUtils.GetLong(userBasicData.UserID, 0),
                                                           userBasicData.Username, userID, groupID, userBasicData.Deleted) == 1;

                    if (!synchronised)
                    {
                        Logger.Log(LogLevels.Info,
                                   string.Format("Child user synchronize error | childUserID:'{0}', username:'{1}', parentID='{2}' groupID:'{3}'",
                                                 userBasicData.UserID, userBasicData.Username, userID, groupID));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, string.Format("userID:'{0}', groupID:'{1}'", userID, groupID), ex);
            }
        }

        // PRIVATE //

        /// <summary>
        /// Reads the child user from database.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="dbChildUser">The database child user.</param>
        private static User ReadChildUserFromDb<T>(T dbChildUser) where T : class
        {
            if (typeof(T) == typeof(Db_User))
            {
                Db_User childUserFromDb = dbChildUser as Db_User;

                return childUserFromDb == null
                    ? null
                    : new User(childUserFromDb.UserID, childUserFromDb.Username, childUserFromDb.ParentID,
                               (UserStatuses)childUserFromDb.Status,
                               childUserFromDb.CreatedDate);
            }

            return null;
        }

        /// <summary>
        /// Reads the child users from database.
        /// </summary>
        /// <typeparam name="T">The type of objects that the collection will hold.</typeparam>
        /// <param name="dbChildUsers">The database child users.</param>
        private static List<User> ReadChildUsersFromDb<T>(ICollection<T> dbChildUsers) where T : class
        {
            if (dbChildUsers == null || dbChildUsers.Count == 0)
                return null;

            return dbChildUsers.Select(dbChildUser => ReadChildUserFromDb(dbChildUser)).ToList();
        }

        /// <summary>
        /// Synchronizes the user.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="parentID">The parent ID.</param>
        /// <param name="groupID">The group ID.</param>
        private static bool SynchronizeUser(long userID, string username, long parentID, long groupID, string firstName, string lastName, string place)
        {
            if (userID <= 0 || StringUtils.IsNullOrEmptyOrWS(username))
                return false;

            try
            {
                bool syncronized =
                    DbProvidersWS.SynchronizeUser(userID, username, (parentID == 0 ? FCMConstants.NotDefinedIntValue : parentID), groupID, firstName, lastName, place) == 1;
                return syncronized;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, string.Format("userID:'{0}', username:'{1}', parentID:'{2}', groupID:'{3}'",
                                                          userID, username, parentID, groupID), ex);

                return false;
            }
        }

        #endregion Child User

        #region SSO/User

        // PUBLIC //

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <param name="applicationID">The application ID.</param>
        /// <param name="applicationName">Name of the application.</param>
        public Application GetApplication(string applicationID, string applicationName)
        {
            if (StringUtils.IsNullOrEmptyOrWS(applicationID) &&
                StringUtils.IsNullOrEmptyOrWS(applicationName))
                return null;

            try
            {
                Application application = null;
                string culture = CultureHelper.GetCulture();
                string cacheKey = CacheHelper.BuildCacheKey("Application", culture, applicationID, applicationName);

                if (FCMConfig.Data.EnableCache && CacheUtils.Contains(cacheKey))
                    application = (Application)CacheUtils.GetItem(cacheKey);
                else
                {
                    List<Application> applications = GetApplications();

                    if (applications != null && applications.Count > 0)
                    {
                        application = new Application();

                        foreach (Application existingApplication in applications)
                        {
                            if (!StringUtils.IsNullOrEmptyOrWS(applicationID))
                            {
                                if (existingApplication.ApplicationID.ToLower() == applicationID.ToLower())
                                {
                                    application = existingApplication;
                                    break;
                                }
                            }
                            else
                            {
                                if (existingApplication.Name.ToLower() == applicationName.ToLower() ||
                                    (existingApplication.IsAdmin && applicationName == "$ADMIN"))
                                {
                                    application = existingApplication;
                                    break;
                                }
                            }
                        }
                    }

                    if (FCMConfig.Data.EnableCache)
                        CacheUtils.Add(cacheKey, application, null,
                                       DateTimeHelper.GetSvcProvDateTimeNow().AddSeconds(FCMConfig.Data.LongCacheDuration), TimeSpan.Zero,
                                       cacheNullObjects: FCMConfig.Data.CacheNullObjects);
                }

                return application;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, string.Format("applicationID:'{0}', applicationName:'{1}'", applicationID, applicationName), ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the applications.
        /// </summary>
        public List<Application> GetApplications()
        {
            //return null;

            try
            {
                List<Application> applications = null;
                string culture = CultureHelper.GetCulture();
                string cacheKey = CacheHelper.BuildCacheKey("Applications", culture);

                if (FCMConfig.Data.EnableCache && CacheUtils.Contains(cacheKey))
                    applications = (List<Application>)CacheUtils.GetItem(cacheKey);
                else
                {
                    SSOAuth_ApplicationData[] applicationsDataList = SSOAuthWS.GetAllApplicationsData(culture);

                    if (applicationsDataList != null && applicationsDataList.Length > 0)
                    {
                        applications = new List<Application>(applicationsDataList.Length);

                        foreach (SSOAuth_ApplicationData applicationData in applicationsDataList)
                        {
                            if (applicationData == null)
                                continue;

                            Application application = null;

                            if (ReadApplication(ref application, applicationData, GetAllRoles()))
                                applications.Add(application);
                        }
                    }

                    if (FCMConfig.Data.EnableCache)
                        CacheUtils.Add(cacheKey, applications, null,
                                       DateTimeHelper.GetSvcProvDateTimeNow().AddSeconds(FCMConfig.Data.LongCacheDuration), TimeSpan.Zero,
                                       cacheNullObjects: FCMConfig.Data.CacheNullObjects);
                }

                return applications;
            }
            catch (SoapException ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);

                if (!StringUtils.IsNullOrEmptyOrWS(ex.Message) && ex.Message.Contains("00401"))
                {
                    FCMBusiness.ClearSSOCache();
                    return GetApplications();
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the header and footer.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="footer">The footer.</param>
        /// <param name="forCurrentUser">if set to <c>true</c> gets the header and footer for current user.</param>
        public void GetHeaderAndFooter(out string header, out string footer, bool forCurrentUser = false)
        {
            header = string.Empty;
            footer = string.Empty;

            try
            {
                string culture = CultureHelper.GetCulture();
                string cacheKey = CacheHelper.BuildCacheKey("AppsHeaderFooter", culture, forCurrentUser);

                if (FCMConfig.Data.EnableCache && CacheUtils.Contains(cacheKey))
                {
                    header = (string)CacheUtils.GetItem(cacheKey);
                    footer = (string)CacheUtils.GetItem(cacheKey + "_AdditionalParam");
                }
                else
                {
                    if (forCurrentUser)
                        header = WebAppsLauncherWS.GetHeaderAndFooter(GetSSOWebAppsLauncherAuthData(), culture, out footer);
                    else
                        header = WebAppsLauncherWS.GetAllApplicationsHeaderAndFooter(culture, out footer);

                    if (FCMConfig.Data.EnableCache)
                    {
                        CacheUtils.Add(cacheKey, header, null,
                                       DateTimeHelper.GetSvcProvDateTimeNow().AddSeconds(FCMConfig.Data.LongCacheDuration), TimeSpan.Zero,
                                       cacheNullObjects: FCMConfig.Data.CacheNullObjects);

                        CacheUtils.Add(cacheKey + "_AdditionalParam", footer, null,
                                       DateTimeHelper.GetSvcProvDateTimeNow().AddSeconds(FCMConfig.Data.LongCacheDuration), TimeSpan.Zero,
                                       cacheNullObjects: FCMConfig.Data.CacheNullObjects);
                    }
                }
            }
            catch (SoapException ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);

                if (!StringUtils.IsNullOrEmptyOrWS(ex.Message) && ex.Message.Contains("00401"))
                {
                    FCMBusiness.ClearSSOCache();
                    GetHeaderAndFooter(out header, out footer);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
            }
        }

        /// <summary>
        /// Gets the SSO application auth token.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        public string GetSSOApplicationAuthToken(string applicationName)
        {
            if (StringUtils.IsNullOrEmptyOrWS(applicationName))
                return string.Empty;

            try
            {
                Application application = GetApplication("", applicationName);

                if (application == null)
                    return string.Empty;

                string newSSOAuthToken;
                string ssoAuthtoken = application.IsAdmin
                    ? SSOAuthWS.GetSSOMasterApplicationAuthToken(
                        GetSSOAuthData(FCMConfig.Security.SSOApplicationID, SSOAuthToken),
                        out newSSOAuthToken)
                    : SSOAuthWS.GetSSOApplicationAuthToken(
                        GetSSOAuthData(FCMConfig.Security.SSOApplicationID, SSOAuthToken),
                        application.ApplicationID,
                        out newSSOAuthToken);

                SSOAuthToken = newSSOAuthToken;

                if (StringUtils.IsNullOrEmptyOrWS(ssoAuthtoken))
                    return string.Empty;

                return ssoAuthtoken;
            }
            catch (SoapException ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);

                if (!StringUtils.IsNullOrEmptyOrWS(ex.Message) && ex.Message.Contains("00401"))
                {
                    FCMBusiness.ClearSSOCache();
                    return GetSSOApplicationAuthToken(applicationName);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Overrides the SSO.
        /// </summary>
        public User OverrideSSO()
        {
            return OverrideSSO(FCMConfig.Security.OverrideSSOUserID, FCMConfig.Security.OverrideSSOUsername,
                               FCMConfig.Security.OverrideSSOParentID);
        }

        /// <summary>
        /// Overrides the SSO.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="parentID">The parent ID.</param>
        public User OverrideSSO(long userID, string username, long parentID)
        {
            User user = new User(userID, username, parentID, FCMConfig.Security.OverrideSSOProvGroupID,
                                 FCMConfig.Security.OverrideSSORoleID)
            { IsSuperUser = FCMConfig.Security.OverrideSSOIsSuperUser };

            // TODO: Disabled temporary!
            // user.Firstname = "Testko";

            //bool synchronised = SynchronizeUser(user.UserID, user.Username, user.ParentID, user.ProvisioningGroupID,"Testkeeeeeeeeo", "Testic");

            //if (!synchronised)
            //{
            //    Logger.Log(LogLevels.Debug, string.Format("User synchronisation error | user.UserID:'{0}'", user.UserID));
            //    return null;
            //}

            if (user.IsChildUser)
            {
                user.ChildUserID = user.UserID;
                user.UserID = user.ParentID;
            }

            UserID = user.UserID;
            SSOAuthToken = "GISDATA-HC";
            SessionUtils.SetValue(SessionHelper.BuildSessionKey("LastSSOSessionRefreshTime"), DateTimeHelper.GetSvcProvDateTimeNow());

            return user;
        }

        /// <summary>
        /// Refreshes the session.
        /// </summary>
        public bool RefreshSession()
        {
            try
            {
                bool refreshed = false;
                string sessionKey = SessionHelper.BuildSessionKey("LastSSOSessionRefreshTime");

                if (SessionUtils.GetValue(sessionKey) != null && !StringUtils.IsNullOrEmptyOrWS(SSOAuthToken))
                {
                    DateTime lastSSOSessionRefreshTime = DateTimeHelper.GetDateTime(SessionUtils.GetValue(sessionKey), DateTime.MinValue);
                    TimeSpan timeDiff = DateTimeHelper.GetSvcProvDateTimeNow().Subtract(lastSSOSessionRefreshTime);

                    if ((timeDiff.TotalMinutes + 2) > FCMConfig.Security.SSOSessionTimeout)
                    {
                        string newSSOAuthToken;
                        refreshed =
                            SSOAuthWS.RefreshSession(GetSSOAuthData(FCMConfig.Security.SSOApplicationID, SSOAuthToken),
                                                     out newSSOAuthToken);

                        SSOAuthToken = newSSOAuthToken;

                        if (refreshed && !StringUtils.IsNullOrEmptyOrWS(newSSOAuthToken))
                            SessionUtils.SetValue(sessionKey, DateTimeHelper.GetSvcProvDateTimeNow());
                        else
                        {
                            Logger.Log(LogLevels.Debug, "Refresh session failed!");
                            throw new AuthenticationException("Refresh session error!");
                        }
                    }
                }

                return refreshed;
            }
            catch (SoapException ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);

                if (!StringUtils.IsNullOrEmptyOrWS(ex.Message) && ex.Message.Contains("00401"))
                {
                    FCMBusiness.ClearSSOCache();
                    return RefreshSession();
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
                return false;
            }
        }

        /// <summary>
        /// Verifies the SSO auth token.
        /// </summary>
        /// <param name="applicationID">The application ID.</param>
        /// <param name="ssoAuthToken">The sso auth token.</param>
        /// <param name="headerHTML">The header HTML.</param>
        /// <param name="footerHTML">The footer HTML.</param>
        public User VerifySSOAuthToken(string applicationID, string ssoAuthToken, out string headerHTML,
                                       out string footerHTML)
        {
            headerHTML = "";
            footerHTML = "";

            if (StringUtils.IsNullOrEmptyOrWS(applicationID) ||
                StringUtils.IsNullOrEmptyOrWS(ssoAuthToken))
                return null;

            try
            {
                string sessionCulture;
                string newSSOAuthToken;
                string[] allowedApplicationIDs;
                SSOAuth_Message[] messageDataList;
                string roleID;
                int groupID;
                SSOAuth_UserData userData;
                SSOAuth_UserData masterUserData;
                SSOAuth_CompanyData companyData;

                bool verified = SSOAuthWS.VerifyTokenAndGetNew(GetSSOAuthData(applicationID, ssoAuthToken), out sessionCulture,
                                                               out newSSOAuthToken, out allowedApplicationIDs, out headerHTML,
                                                               out footerHTML, out messageDataList, out roleID, out groupID,
                                                               out userData, out masterUserData, out companyData);

                SSOAuthToken = newSSOAuthToken;

                if (!verified)
                {
                    Logger.Log(LogLevels.Debug, string.Format("User not verified! | applicationID:'{0}', ssoAuthToken:'{1}'",
                                                              applicationID, ssoAuthToken));

                    return null;
                }

                User user = null;

                if (!ReadUser(ref user, userData, companyData, masterUserData, roleID, groupID, allowedApplicationIDs))
                    return null;

                if (!user.IsSuperUser)
                    SynchronizeUser(ValidationUtils.GetLong(masterUserData.UserID, 0), masterUserData.Username, -666, user.ProvisioningGroupID, user.Firstname, user.Lastname, user.Place);

                bool synchronised = SynchronizeUser(user.UserID, user.Username, user.ParentID, user.ProvisioningGroupID, user.Firstname, user.Lastname, user.Place);


                if (!synchronised)
                {
                    Logger.Log(LogLevels.Debug, string.Format("User not synchronized! | user.UserID:'{0}'", user.UserID));
                    return null;
                }

                if (user.IsChildUser)
                {
                    user.ChildUserID = user.UserID;
                    user.UserID = user.ParentID;
                }

                UserID = user.UserID;

                SessionUtils.SetValue(SessionHelper.BuildSessionKey("LastSSOSessionRefreshTime"), DateTimeHelper.GetSvcProvDateTimeNow());
                return user;
            }
            catch (SoapException ex)
            {
                Logger.Log(LogLevels.Error, string.Format("applicationID:'{0}'", applicationID), ex);

                if (!StringUtils.IsNullOrEmptyOrWS(ex.Message) && ex.Message.Contains("00401"))
                {
                    FCMBusiness.ClearSSOCache();
                    return VerifySSOAuthToken(applicationID, ssoAuthToken, out headerHTML, out footerHTML);
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, string.Format("applicationID:'{0}'", ssoAuthToken), ex);
                return null;
            }
        }

        /// <summary>
        /// Wap login.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public User WapLogin(string username, string password)
        {
            if (StringUtils.IsNullOrEmptyOrWS(username) || StringUtils.IsNullOrEmptyOrWS(password))
                return null;

            try
            {
                string culture = CultureHelper.GetCulture();
                byte[] passwordHash = EncryptionUtils.ComputeMD5Hash(string.Format("{0}/{1}", password, username.ToLower()));
                string passwordHashString = Convert.ToBase64String(passwordHash);

                string sessionToken;
                WebApps_UserData userData;
                WebApps_UserData masterUserData;
                WebApps_CompanyData companyData;
                int groupID;
                WebApps_ApplicationRole[] applicationRolesDataList;
                string headerHTML;
                string footerHTML;
                WebApps_Message[] messageDataList;
                bool passwordExpired;

                bool loggedOn = WebAppsLauncherWS.LogonUser(GetSSOWebAppsLauncherAuthData(), culture, username, passwordHashString,
                                                            out sessionToken, out userData, out masterUserData, out companyData,
                                                            out groupID, out applicationRolesDataList, out headerHTML, out footerHTML,
                                                            out messageDataList, out passwordExpired);

                WebAppsToken = sessionToken;

                if (!loggedOn)
                {
                    Logger.Log(LogLevels.Debug, string.Format("User not logged in! | username:'{0}'", username));
                    return null;
                }

                User user = null;

                if (!ReadUser(ref user, userData, companyData, masterUserData, groupID))
                    return null;

                if (!user.IsSuperUser)
                    SynchronizeUser(ValidationUtils.GetLong(masterUserData.UserID, 0), masterUserData.Username, -666, user.ProvisioningGroupID, user.Firstname, user.Lastname, user.Place);

                bool synchronised = SynchronizeUser(user.UserID, user.Username, user.ParentID, user.ProvisioningGroupID, user.Firstname, user.Lastname, user.Place);

                if (!synchronised)
                {
                    Logger.Log(LogLevels.Debug, string.Format("User not synchronized! | user.UserID:'{0}'", user.UserID));
                    return null;
                }

                if (user.IsChildUser)
                {
                    user.ChildUserID = user.UserID;
                    user.UserID = user.ParentID;
                }

                UserID = user.UserID;

                SessionUtils.SetValue(FCMConfig.Project.DefaultName + ".LastSSOSessionRefreshTime", DateTimeHelper.GetSvcProvDateTimeNow());
                return user;
            }
            catch (SoapException ex)
            {
                Logger.Log(LogLevels.Error, string.Format("username:'{0}'", username), ex);

                if (!StringUtils.IsNullOrEmptyOrWS(ex.Message) && ex.Message.Contains("00401"))
                {
                    FCMBusiness.ClearSSOCache();
                    return WapLogin(username, password);
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, string.Format("username:'{0}'", username), ex);
                return null;
            }
        }

        /// <summary>
        /// Wap logout.
        /// </summary>
        public bool WapLogout()
        {
            try
            {
                WebAppsLauncherWS.LogoffUserSession(GetSSOWebAppsLauncherAuthData());
                return true;
            }
            catch (SoapException ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);

                if (!StringUtils.IsNullOrEmptyOrWS(ex.Message) && ex.Message.Contains("00401"))
                    return WapLogout();

                return false;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);

                return false;
            }
        }

        // PRIVATE //

        /// <summary>
        /// Gets all roles.
        /// </summary>
        private static List<Role> GetAllRoles()
        {
            try
            {
                List<Role> roles = null;
                string culture = CultureHelper.GetCulture();
                string cacheKey = CacheHelper.BuildCacheKey("Roles", culture);

                if (FCMConfig.Data.EnableCache && CacheUtils.Contains(cacheKey))
                    roles = (List<Role>)CacheUtils.GetItem(cacheKey);
                else
                {
                    SSOAuth_Role[] rolesDataList = SSOAuthWS.GetAllRoles(culture);

                    if (rolesDataList != null && rolesDataList.Length > 0)
                    {
                        roles = new List<Role>(rolesDataList.Length);

                        foreach (SSOAuth_Role roleData in rolesDataList)
                        {
                            if (roleData == null)
                                continue;

                            Role role = null;

                            if (ReadRole(ref role, roleData))
                                roles.Add(role);
                        }
                    }

                    if (FCMConfig.Data.EnableCache)
                        CacheUtils.Add(cacheKey, roles, null, DateTimeHelper.GetSvcProvDateTimeNow().AddSeconds(FCMConfig.Data.LongCacheDuration),
                                       TimeSpan.Zero,
                                       cacheNullObjects: FCMConfig.Data.CacheNullObjects);
                }

                return roles;
            }
            catch (SoapException ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);

                if (!StringUtils.IsNullOrEmptyOrWS(ex.Message) && ex.Message.Contains("00401"))
                {
                    FCMBusiness.ClearSSOCache();
                    return GetAllRoles();
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the SSO auth data.
        /// </summary>
        /// <param name="applicationID">The application ID.</param>
        /// <param name="ssoAuthToken">The sso auth token.</param>
        private static SSOAuth_SSOAuthData GetSSOAuthData(string applicationID, string ssoAuthToken)
        {
            try
            {
                SSOAuth_SSOAuthData ssoAuthData = new SSOAuth_SSOAuthData
                {
                    ApplicationID = applicationID,
                    IPAddress = HttpContext.Current.Request.UserHostAddress,
                    SSOToken = ssoAuthToken
                };

                return ssoAuthData;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the SSO web apps launcher auth data.
        /// </summary>
        private WebApps_SSOWebAppsLauncherAuthData GetSSOWebAppsLauncherAuthData()
        {
            try
            {
                WebApps_SSOWebAppsLauncherAuthData ssoWebAppsLauncherAuthData = new WebApps_SSOWebAppsLauncherAuthData();

                byte[] sessionIDHash = EncryptionUtils.ComputeMD5Hash(HttpContext.Current.Session.SessionID);
                string sessionIDHashString = Convert.ToBase64String(sessionIDHash);

                ssoWebAppsLauncherAuthData.IPAddress = HttpContext.Current.Request.UserHostAddress;
                ssoWebAppsLauncherAuthData.SessionToken = WebAppsToken ?? string.Empty;
                ssoWebAppsLauncherAuthData.WebSessionID = sessionIDHashString;

                return ssoWebAppsLauncherAuthData;
            }
            catch (SoapException ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);

                if (!StringUtils.IsNullOrEmptyOrWS(ex.Message) && ex.Message.Contains("00401"))
                {
                    FCMBusiness.ClearSSOCache();
                    return GetSSOWebAppsLauncherAuthData();
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
                return null;
            }
        }

        /// <summary>
        /// Reads the application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="applicationData">The application data.</param>
        /// <param name="rolesDataList">The roles data list.</param>
        private static bool ReadApplication(ref Application application, SSOAuth_ApplicationData applicationData, IEnumerable<Role> rolesDataList)
        {
            if (applicationData == null)
                return false;

            try
            {
                if (application == null)
                {
                    string applicationTitle = StringUtils.IsNullOrEmptyOrWS(applicationData.Title)
                        ? applicationData.Name
                        : applicationData.Title;

                    application = new Application(applicationData.ApplicationID, applicationData.URL, applicationData.Name, applicationTitle,
                                                  applicationData.Admin, rolesDataList);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
                return false;
            }
        }

        /// <summary>
        /// Reads the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="roleData">The role data.</param>
        private static bool ReadRole(ref Role role, SSOAuth_Role roleData)
        {
            if (roleData == null)
                return false;

            try
            {
                if (role == null)
                    role = new Role(roleData.RoleID, roleData.Description);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
                return false;
            }
        }

        /// <summary>
        /// Reads the user/child user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userData">The user data.</param>
        /// <param name="companyData">The company data.</param>
        /// <param name="masterUserData">The master user data.</param>
        /// <param name="roleID">The role ID.</param>
        /// <param name="groupID">The group ID.</param>
        /// <param name="allowedApplicationIDs">The allowed application IDs.</param>
        private static bool ReadUser(ref User user, SSOAuth_UserData userData, SSOAuth_CompanyData companyData, SSOAuth_UserData masterUserData,
                                     string roleID, int groupID, string[] allowedApplicationIDs)
        {
            if (userData == null && companyData == null)
                return false;

            try
            {
                bool isSuperUser = masterUserData == null;

                if (userData != null)
                {
                    user = new User(ValidationUtils.GetLong(userData.UserID, 0), userData.Username,
                                    isSuperUser ? 0 : ValidationUtils.GetLong(masterUserData.UserID, 0),
                                    groupID, userData.Email, userData.Firstname, userData.Lastname, userData.Address.AddressName,
                                    userData.Address.HouseNumber, userData.Address.ZIP, userData.Address.City,
                                    userData.Telephone, userData.MSISDN, userData.OIB, string.Empty, string.Empty, false,
                                    userData.DefaultCulture, roleID, allowedApplicationIDs);

                    if (companyData != null && isSuperUser)
                    {
                        user.CompanyName = companyData.CompanyName;
                        user.PersonalID = companyData.OIB;
                        user.CompanyMB = companyData.MB;
                        user.Address = companyData.Address.AddressName;
                        user.HouseNumber = companyData.Address.HouseNumber;
                        user.PostalCode = companyData.Address.ZIP;
                        user.Place = companyData.Address.City;
                        user.Telephone = companyData.Telephone;
                        user.IsBusinessUser = true;
                    }

                    user.IsSuperUser = isSuperUser;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
                return false;
            }
        }

        /// <summary>
        /// Reads the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userData">The user data.</param>
        /// <param name="companyData">The company data.</param>
        /// <param name="masterUserData">The master user data.</param>
        /// <param name="groupID">The group ID.</param>
        private static bool ReadUser(ref User user, WebApps_UserData userData, WebApps_CompanyData companyData, WebApps_UserData masterUserData,
                                     int groupID)
        {
            if (userData == null && companyData == null)
                return false;

            try
            {
                bool isSuperUser = masterUserData == null;

                if (userData != null)
                {
                    user = new User(ValidationUtils.GetLong(userData.UserID, 0), userData.Username,
                                    isSuperUser ? 0 : ValidationUtils.GetLong(masterUserData.UserID, 0),
                                    ValidationUtils.GetLong(groupID, 0), userData.Email, userData.Firstname, userData.Lastname,
                                    userData.Address.AddressName, userData.Address.HouseNumber, userData.Address.ZIP, userData.Address.City,
                                    userData.Telephone, userData.MSISDN, userData.OIB, string.Empty, string.Empty, false,
                                    userData.DefaultCulture, string.Empty, null);

                    if (companyData != null && isSuperUser)
                    {
                        user.CompanyName = companyData.CompanyName;
                        user.PersonalID = companyData.OIB;
                        user.CompanyMB = companyData.MB;
                        user.Address = companyData.Address.AddressName;
                        user.HouseNumber = companyData.Address.HouseNumber;
                        user.PostalCode = companyData.Address.ZIP;
                        user.Place = companyData.Address.City;
                        user.Telephone = companyData.Telephone;
                        user.IsBusinessUser = true;
                    }

                    user.IsSuperUser = isSuperUser;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, exception: ex);
                return false;
            }
        }

        #endregion SSO/User

        #endregion Methods
    }
}