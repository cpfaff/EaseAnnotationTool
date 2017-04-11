
using System.ComponentModel.DataAnnotations;

namespace CAFE.DAL.Models
{
    /// <summary>
    /// Contains data about last version of application in the online storage
    /// </summary>
    public class DbRemoteVersion : DbBase
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Version in x.x.x notation
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The version on server is newer
        /// </summary>
        public bool IsNew { get; set; }
    }
}
