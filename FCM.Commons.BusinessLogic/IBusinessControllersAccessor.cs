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

using FCM.Commons.BusinessLogic.Controllers;

#endregion Using

namespace FCM.Commons.BusinessLogic
{
    /// <summary>
    /// IBusinessControllersAccessor interface.
    /// </summary>
    public interface IBusinessControllersAccessor
    {
        #region Data Members

        /// <summary>
        /// Gets or sets the user controller.
        /// </summary>
        /// <value>The user controller.</value>
        UserController UserController { get; set; }

        #endregion Data Members
    }
}