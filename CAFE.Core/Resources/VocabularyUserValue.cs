using System;

namespace CAFE.Core.Resources
{
    public class VocabularyUserValue
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
