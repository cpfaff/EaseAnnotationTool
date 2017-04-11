

namespace CAFE.Core.Resources
{

    /// <summary>
    /// Vocabulary atomic value
    /// </summary>
    public class VocabularyValue
    {
        public long Id { get; set; }
        /// <summary>
        /// The string value
        /// </summary>
        public string Value { get; set; }

        public string Type { get; set; }
        /// <summary>
        /// The resource identifier for value
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The resource identifier for value
        /// </summary>
        public string Description { get; set; }
    }
}
