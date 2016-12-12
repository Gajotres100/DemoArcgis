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

#endregion Using

namespace FCM.Commons.BusinessLogic.Entities
{
    /// <summary>
    /// Application business entity.
    /// </summary>
    [Serializable]
    public sealed class Application
    {
        #region Properties

        /// <summary>
        /// Gets or sets the application ID.
        /// </summary>
        /// <value>The application ID.</value>
        public string ApplicationID { get; private set; }

        /// <summary>
        /// Gets or sets the application URL.
        /// </summary>
        /// <value>The application URL.</value>
        public string Url { get; private set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the application title.
        /// </summary>
        /// <value>The application title.</value>
        public string Title { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this application is admin.
        /// </summary>
        /// <value><c>true</c> if this application is admin; otherwise, <c>false</c>.</value>
        public bool IsAdmin { get; private set; }

        /// <summary>
        /// Gets or sets the application roles.
        /// </summary>
        /// <value>The application roles.</value>
        public List<Role> ApplicationRoles { get; private set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="applicationID">The application ID.</param>
        /// <param name="url">The URL.</param>
        /// <param name="name">The name.</param>
        /// <param name="title">The title.</param>
        /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
        /// <param name="applicationRoles">The application roles.</param>
        public Application(string applicationID, string url, string name, string title, bool isAdmin, IEnumerable<Role> applicationRoles)
        {
            ApplicationID = applicationID;
            Url = url;
            Name = name;
            Title = title;
            IsAdmin = isAdmin;

            if (applicationRoles != null)
                ApplicationRoles = applicationRoles as List<Role>;
        }

        #endregion Constructors
    }
}