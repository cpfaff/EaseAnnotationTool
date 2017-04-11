
using System.ComponentModel.DataAnnotations;
using CAFE.Core.Searching;

namespace CAFE.DAL.Models
{
    public class DbSearchFilterCachedItem : DbBase
    {
        [Key]
        [MaxLength(500)]
        public string Name { get; set; }
        public string Description { get; set; }
        public FilterType FilterType { get; set; }
    }
}
