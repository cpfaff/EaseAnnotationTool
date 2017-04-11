
namespace CAFE.Web.Areas.Api.Models
{
    public class ManagedVocabularyValueModel
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public string CreationDate { get; set; }
        public bool IsGlobal { get; set; }
    }
}