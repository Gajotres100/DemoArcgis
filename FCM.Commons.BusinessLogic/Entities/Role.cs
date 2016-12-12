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

#endregion Using

namespace FCM.Commons.BusinessLogic.Entities
{
    /// <summary>
    /// Role business entity.
    /// </summary>
    [Serializable]
    public sealed class Role
    {
        #region Properties

        /// <summary>
        /// Gets or sets the role ID.
        /// </summary>
        /// <value>The role ID.</value>
        public string RoleID { get; private set; }

        /// <summary>
        /// Gets or sets the role description.
        /// </summary>
        /// <value>The role description.</value>
        public string Description { get; private set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="roleID">The role ID.</param>
        /// <param name="description">The description.</param>
        public Role(string roleID, string description)
        {
            RoleID = roleID;
            Description = description;
        }

        #endregion Constructors
    }
}