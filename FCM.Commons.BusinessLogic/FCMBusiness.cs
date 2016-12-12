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

using FCM.Commons.GlobalHelper;
using FCM.Commons.Utilities.Web;

#endregion Using

namespace FCM.Commons.BusinessLogic
{
    /// <summary>
    /// FCM business helper for the solution libraries and applications.
    /// </summary>
    public sealed class FCMBusiness
    {
        #region Methods

        /// <summary>
        /// Clears the SSO cache.
        /// </summary>
        public static void ClearSSOCache()
        {
            CacheUtils.ClearCache(CacheHelper.BuildCacheKey("Applications"));
            CacheUtils.ClearCache(CacheHelper.BuildCacheKey("Roles"));
        }

        #endregion Methods
    }
}