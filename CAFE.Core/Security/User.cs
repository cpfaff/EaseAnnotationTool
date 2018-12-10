using System.Collections.Generic;
using CAFE.Core.Integration;
using Microsoft.AspNet.Identity;
using Constants = CAFE.Core.Misc.Constants;
using DateTime = System.DateTime;

namespace CAFE.Core.Security
{
    /// <summary>
    /// Application-scoped user
    /// </summary>
    public class User : IUser<string>
    {
        public User()
        {
            Groups = new List<Group>();
            Role = Constants.UserRoleName;
        }

        public string Id { get; set; }

        /// <summary>
		/// User's e-mail
		/// </summary>
        public string Email { get; set; }

        /// <summary>
		/// Is user's e-mal confirmed
		/// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
		/// Users password Hash
		/// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// User's security stamp
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// User's phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User's phone confirm status
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Is Two factor authorization enabled
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
		/// User's lockout date
		/// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
		/// User's lockout status
		/// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
		/// User's fail logins
		/// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
		/// User's login name
		/// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User's Surname
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User's Surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// User's Salutation
        /// </summary>
        public string Salutation { get; set; }

        /// <summary>
        /// User's Postal address
        /// </summary>
        public string PostalAddress { get; set; }

        /// <summary>
        /// Indicates that user disabled or not
        /// true - enabled, false - disabled
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// User's role
        /// </summary>
        public string Role { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public string PhotoUrl { get; set; }

        /// <summary>
        /// User's hidden helpers
        /// </summary>
        public List<UserHiddenHelper> HiddenHelpers { get; set; }

        /// <summary>
        /// User's groups
        /// </summary>
        public List<Group> Groups { get; set; }

        /// <summary>
        /// User's owned files
        /// </summary>
        public List<UserFile> OwnedFiles { get; set; }

        /// <summary>
        /// User's accessible files
        /// </summary>
        public List<UserFile> AccessibleFiles { get; set; }

        /// <summary>
        /// User's accessible annotation items
        /// </summary>
        public List<AnnotationItemAccessibleUsers> AccessibleAnnotationItems { get; set; }
    }
}
