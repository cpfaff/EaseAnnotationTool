using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    [Table("DbUserLogins")]
    public class DbUserLogin: DbBase
    {
        /// <summary>
        /// User's unique identifier
        /// </summary>
        [Key, Required]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Time { get; set; }
    }
}
