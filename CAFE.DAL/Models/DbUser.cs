using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    [Table("DbUsers")]
    public class DbUser: DbBase
    {
        public DbUser()
        {
            Roles = new List<DbRole>();
            OwnedFiles = new List<DbUserFile>();
            AccessibleFiles = new List<DbUserFile>();
            //AccessibleAnnotationItems = new List<DbAnnotationItemAccessibleUsers>();
        }

        /// <summary>
        /// User's unique identifier
        /// </summary>
        [Key, Required]
		public Guid Id { get; set; }

		/// <summary>
		/// User's e-mail
		/// </summary>
		[Required]
		public string Email { get; set; }

		/// <summary>
		/// Is user's e-mal confirmed
		/// </summary>
		public bool EmailConfirmed { get; set; }

		/// <summary>
		/// Users password Hash
		/// </summary>
		[Required]
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
		public System.DateTime? LockoutEndDateUtc { get; set; }

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
		[Required]
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
        /// User's Postal address
        /// </summary>
        public string PostalAddress { get; set; }
        public string Salutation { get; set; }
        public bool IsActive { get; set; }
        public bool IsAccepted { get; set; }
        public System.DateTime? AcceptanceDate { get; set; }
        public virtual ICollection<DbRole> Roles { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<DbUserFile> OwnedFiles { get; set; }
        [InverseProperty("AcceptedUsers")]
        public virtual ICollection<DbUserFile> AccessibleFiles { get; set; }
        public virtual ICollection<DbUserHiddenHelper> HiddenHelpers { get; set; }
        //public virtual ICollection<DbAnnotationItemAccessibleUsers> AccessibleAnnotationItems { get; set; } 
        public string PhotoUrl { get; set; }
    }
}
