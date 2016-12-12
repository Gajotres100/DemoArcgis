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
using FCM.Commons.ConfigProvider;
using FCM.Commons.Constants;
using FCM.Commons.GlobalHelper;
using FCM.Commons.Utilities.Text;

#endregion Using

namespace FCM.Commons.BusinessLogic.Entities
{
    /// <summary>
    /// User business entity.
    /// </summary>
    [Serializable]
    public sealed class User
    {
        #region Properties

        // USER //

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <remarks>Always filled with MASTER(superusers) user ID.</remarks>
        /// <value>The user ID.</value>
        public long UserID { get; internal set; }

        /// <summary>
        /// Gets or sets the child user ID.
        /// </summary>
        /// <value>The child user ID.</value>
        public long ChildUserID { get; internal set; }

        public long ChildUserIDDB
        {
            get
            {

                if (ChildUserID.Equals(null)) return UserID;
                if (ChildUserID.Equals(0)) return UserID;
                return ChildUserID;


            }

        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; private set; }

        /// <summary>
        /// Gets or sets the user parent ID.
        /// </summary>
        /// <value>The user parent ID.</value>
        public long ParentID { get; private set; }

        /// <summary>
        /// Gets or sets the user provisioning group ID.
        /// </summary>
        /// <value>The user provisioning group ID.</value>
        public long ProvisioningGroupID { get; private set; }

        /// <summary>
        /// Gets or sets the user status.
        /// </summary>
        /// <value>The user status.</value>
        public UserStatuses Status { get; private set; }

        /// <summary>
        /// Gets the user status description.
        /// </summary>
        /// <value>The user status description.</value>
        //public string StatusDescription
        //{
        //    get
        //    {
        //        switch (Status)
        //        {
        //            case UserStatuses.Deleted:
        //                return ResHelper.GetGRes("FCMTerms", "Deleted");
        //            case UserStatuses.Pending:
        //                return ResHelper.GetGRes("FCMTerms", "Pending");
        //            case UserStatuses.Active:
        //                return ResHelper.GetGRes("FCMTerms", "Active");
        //            case UserStatuses.Suspended:
        //                return ResHelper.GetGRes("FCMTerms", "Suspended");
        //            default:
        //                return "--";
        //        }
        //    }
        //}

        /// <summary>
        /// Gets or sets the user created date.
        /// </summary>
        /// <value>The user created date.</value>
        public DateTime CreatedDate { get; private set; }

        /// <summary>
        /// Gets or sets the user created date formatted.
        /// </summary>
        /// <value>The user created date formatted.</value>
        //public string CreatedDateFormatted
        //{
        //    get
        //    {
        //        return CreatedDate == DateTime.MinValue
        //                   ? ResHelper.GetGRes("FCMTerms", "NoEntry")
        //                   : CreatedDate.ToString(FCMConfig.DateTime.LongDateTimeFormat);
        //    }
        //}

        // SSO USER //

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>The user email.</value>
        public string Email { get; private set; }

        /// <summary>
        /// Gets or sets the user firstname.
        /// </summary>
        /// <value>The user firstname.</value>
        public string Firstname { get; private set; }

        /// <summary>
        /// Gets or sets the user lastname.
        /// </summary>
        /// <value>The user lastname.</value>
        public string Lastname { get; private set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get
            {
                string fullName = Firstname + (StringUtils.IsNullOrEmptyOrWS(Lastname) ? "" : " " + Lastname);
                return StringUtils.IsNullOrEmptyOrWS(fullName) ? Username : fullName;
            }
        }

        /// <summary>
        /// Gets or sets the user address.
        /// </summary>
        /// <value>The user address.</value>
        public string Address { get; internal set; }

        /// <summary>
        /// Gets or sets user the house number.
        /// </summary>
        /// <value>The user house number.</value>
        public string HouseNumber { get; internal set; }

        /// <summary>
        /// Gets or sets user the postal code.
        /// </summary>
        /// <value>The user postal code.</value>
        public string PostalCode { get; internal set; }

        /// <summary>
        /// Gets or sets the user place.
        /// </summary>
        /// <value>The user place.</value>
        public string Place { get; internal set; }

        /// <summary>
        /// Gets or sets the user telephone.
        /// </summary>
        /// <value>The user telephone.</value>
        public string Telephone { get; internal set; }

        /// <summary>
        /// Gets or sets the user MSISDN.
        /// </summary>
        /// <value>The user MSISDN.</value>
        public string MSISDN { get; set; }

        /// <summary>
        /// Gets or sets the personal ID.
        /// </summary>
        /// <value>The personal ID.</value>
        public string PersonalID { get; internal set; }

        /// <summary>
        /// Gets or sets the user company name.
        /// </summary>
        /// <value>The user company name.</value>
        public string CompanyName { get; internal set; }

        /// <summary>
        /// Gets or sets the user company MB.
        /// </summary>
        /// <value>The user company MB.</value>
        public string CompanyMB { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether this user is business user.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this user is business user; otherwise, <c>false</c>.
        /// </value>
        public bool IsBusinessUser { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this user is child user.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this user is child user; otherwise, <c>false</c>.
        /// </value>
        public bool IsChildUser
        {
            get { return !IsSuperUser; }
        }

        /// <summary>
        /// Gets a value indicating whether this user is child user master.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this user is child user master; otherwise, <c>false</c>.
        /// </value>
        public bool IsChildUserMaster
        {
            get
            {
                if (StringUtils.IsNullOrEmptyOrWS(RoleID))
                    return false;

                return IsChildUser && RoleID.ToLower() == "master";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this user is POWERCHILD.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this user is child user master; otherwise, <c>false</c>.
        /// </value>
        public bool IsPowerChild
        {
            get
            {
                if (StringUtils.IsNullOrEmptyOrWS(RoleID))
                    return false;

                return IsChildUser && RoleID.ToLower() == "powerchild";
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this user is super user.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this user is super user; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuperUser { get; internal set; }

        /// <summary>
        /// Gets or sets the user default UI culture.
        /// </summary>
        /// <value>The user default UI culture.</value>
        public string DefaultCulture { get; private set; }

        /// <summary>
        /// Gets or sets the role ID.
        /// </summary>
        /// <value>The role ID.</value>
        public string RoleID { get; private set; }

        /// <summary>
        /// Gets or sets the allowed application IDs.
        /// </summary>
        /// <value>The allowed application IDs.</value>
        public string[] AllowedApplicationIDs { get; private set; }

        /// <summary>
        /// Gets or sets user minimum positioning time.
        /// </summary>
        /// <value>Minimum LBS positioning time.</value>
        public int LBSMinPositionigTime { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="parentID">The parent ID.</param>
        /// <param name="status">The status.</param>
        /// <param name="createdDate">The created date.</param>
        public User(long userID, string username, long parentID, UserStatuses status, DateTime createdDate)
        {
            UserID = userID;
            Username = username;
            ParentID = parentID;
            Status = status;
            CreatedDate = createdDate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="parentID">The parent ID.</param>
        /// <param name="provisioningGroupID">The provisioning group ID.</param>
        /// <param name="roleID">The role ID.</param>
        public User(long userID, string username, long parentID, long provisioningGroupID, string roleID)
        {
            UserID = userID;
            Username = username;
            ParentID = parentID;
            ProvisioningGroupID = provisioningGroupID;
            RoleID = roleID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="parentID">The parent ID.</param>
        /// <param name="provisioningGroupID">The provisioning group ID.</param>
        /// <param name="email">The email.</param>
        /// <param name="firstname">The firstname.</param>
        /// <param name="lastname">The lastname.</param>
        /// <param name="address">The address.</param>
        /// <param name="houseNumber">The house number.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="place">The place.</param>
        /// <param name="telephone">The telephone.</param>
        /// <param name="msisdn">The msisdn.</param>
        /// <param name="personalID">The personal ID.</param>
        /// <param name="companyName">Name of the company.</param>
        /// <param name="companyMB">The company MB.</param>
        /// <param name="isBusinessUser">if set to <c>true</c> [is business user].</param>
        /// <param name="defaultCulture">The default culture.</param>
        /// <param name="roleID">The role ID.</param>
        /// <param name="allowedApplicationIDs">The allowed application ID s.</param>
        public User(long userID, string username, long parentID, long provisioningGroupID, string email, string firstname, string lastname,
                    string address,
                    string houseNumber, string postalCode, string place, string telephone, string msisdn, string personalID, string companyName,
                    string companyMB, bool isBusinessUser, string defaultCulture, string roleID, string[] allowedApplicationIDs)
        {
            UserID = userID;
            Username = username;
            ParentID = parentID;
            ProvisioningGroupID = provisioningGroupID;
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            Address = address;
            HouseNumber = houseNumber;
            PostalCode = postalCode;
            Place = place;
            Telephone = telephone;
            MSISDN = msisdn;
            PersonalID = personalID;
            CompanyName = companyName;
            CompanyMB = companyMB;
            IsBusinessUser = isBusinessUser;
            DefaultCulture = defaultCulture;
            RoleID = roleID;
            AllowedApplicationIDs = allowedApplicationIDs;
        }

        #endregion Constructors
    }
}