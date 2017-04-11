
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models.Resources
{
    public class DbVocabularyValue : DbBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
