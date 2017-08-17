
using System.ComponentModel.DataAnnotations;

namespace CAFE.DAL.Models
{
    public class DbSchemaItemDescription : DbBase
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
    }
}
