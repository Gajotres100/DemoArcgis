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
using System.Collections.Specialized;
using System.Text;
using FCM.Commons.BusinessLogic.FCM_DbProvidersWR;
using FCM.Commons.BusinessLogic.SSO_SSOAuthWR;
using FCM.Commons.BusinessLogic.SSO_WebAppsLauncherWR;
using FCM.Commons.ConfigProvider;
using FCM.Commons.GlobalHelper;
using FCM.Commons.Utilities.Factories;
using FCM.Commons.Utilities.Miscellaneous;
using FCM.Commons.Utilities.Web;
using User = FCM.Commons.BusinessLogic.Entities.User;

#endregion Using

namespace FCM.Commons.BusinessLogic
{
    /// <summary>
    /// Base (abstract) business logic from which all business controllers/processors will be 
    /// derived.
    /// </summary>
    public abstract class BaseBusinessLogic
    {
        #region Fields

        protected const int MaxRows = int.MaxValue;
        protected const int MinRows = int.MinValue;
        private readonly StringDictionary _brokenRules = new StringDictionary();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the database providers web service.
        /// </summary>
        internal static DbProviders DbProvidersWS
        {
            get
            {
                try
                {
                    WebServiceFactory svc = new WebServiceFactory(FCMConfig.Data.DbProvidersWSUrl,
                                                                  FCMConfig.Data.DbProvidersWSTimeout);

                    DbProviders dbProviders = svc.CreateInstance<DbProviders>();
                    return dbProviders;
                }
                catch (Exception ex)
                {
                    Logger.Log(LogLevels.Fatal, "Connection to database providers web service error.", ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the SSO authentication web service.
        /// </summary>
        internal static SSOAuth SSOAuthWS
        {
            get
            {
                try
                {
                    WebServiceFactory svc = new WebServiceFactory(FCMConfig.Security.SSOAuthWSUrl,
                                                                  FCMConfig.Security.SSOAuthWSTimeout);

                    SSOAuth ssoAuth = svc.CreateInstance<SSOAuth>();
                    return ssoAuth;
                }
                catch (Exception ex)
                {
                    Logger.Log(LogLevels.Fatal, "Connection to SSO authentication web service error.", ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the WebAppsLauncher web service.
        /// </summary>
        /// <value>The WebAppsLauncher web service.</value>
        internal static WebAppsLauncher WebAppsLauncherWS
        {
            get
            {
                try
                {
                    WebServiceFactory svc = new WebServiceFactory(FCMConfig.Security.WebAppsLauncherWSUrl,
                                                                  FCMConfig.Security.WebAppsLauncherWSTimeout);

                    WebAppsLauncher webAppsLauncher = svc.CreateInstance<WebAppsLauncher>();
                    return webAppsLauncher;
                }
                catch (Exception ex)
                {
                    Logger.Log(LogLevels.Fatal, "Connection to web applications launcher web service error.", ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <value>The current user.</value>
        public static User CurrentUser
        {
            get
            {
                object obj = SessionUtils.GetValue(SessionHelper.BuildSessionKey("CurrentUser"));
                return (obj == null) ? null : (User)obj;
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        public string Error
        {
            get { return ValidationMessage; }
        }

        /// <summary>
        /// Gets whether the object is valid or not.
        /// </summary>
        public bool IsValid
        {
            get
            {
                ValidationRules();
                return _brokenRules.Count == 0;
            }
        }

        /// <summary>
        /// If the object has broken business rules, use this property to get access to the different validation messages.
        /// </summary>
        public string ValidationMessage
        {
            get
            {
                if (!IsValid)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (string messages in _brokenRules.Values)
                    {
                        sb.AppendLine(messages);
                    }

                    return sb.ToString();
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified property name.
        /// </summary>
        /// <value>The property name.</value>
        public string this[string propertyName]
        {
            get { return _brokenRules.ContainsKey(propertyName) ? _brokenRules[propertyName] : string.Empty; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Add or remove a broken rule.
        /// </summary>
        protected void AddRule(string propertyName, string errorMessage, bool isBroken)
        {
            if (isBroken)
                _brokenRules[propertyName] = errorMessage;
            else
            {
                if (_brokenRules.ContainsKey(propertyName))
                    _brokenRules.Remove(propertyName);
            }
        }

        /// <summary>
        /// Gets the index of the page.
        /// </summary>
        /// <param name="startRowIndex">Start index of the row.</param>
        /// <param name="maximumRows">The maximum rows.</param>
        protected static int GetPageIndex(int startRowIndex, int maximumRows)
        {
            if (maximumRows <= 0)
                return 0;

            return (int)Math.Floor((double)startRowIndex / maximumRows);
        }

        /// <summary>
        /// Reinforces the business rules by adding additional rules to the broken rules collection.
        /// </summary>
        protected virtual void ValidationRules() {}

        #endregion Methods
    }
}