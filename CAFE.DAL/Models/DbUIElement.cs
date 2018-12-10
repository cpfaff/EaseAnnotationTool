

using System.ComponentModel.DataAnnotations;

namespace CAFE.DAL.Models
{
    /// <summary>
    /// Contains data about UI element on Annotation Item form and relation with it Resource url for get data
    /// </summary>
    public class DbUIElement : DbBase
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        [Key, Required]
        public int Id { get; set; }

        /// <summary>
        /// Element identifier on Annotation Form
        /// </summary>
        public string ElementIdOnUI { get; set; }

        /// <summary>
        /// Url for get data to load in UI element
        /// </summary>
        public string UrlForGetData { get; set; }

    }
}
