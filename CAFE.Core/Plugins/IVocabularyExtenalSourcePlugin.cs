using CAFE.Core.Resources;

namespace CAFE.Core.Plugins
{
    /// <summary>
    /// Contract for pluggable components that returns Vocabulary values from external online sources
    /// </summary>
    public interface IVocabularyExtenalSourcePlugin : IVocabularyExternalSourcePlugin<VocabularyValue>
    {

    }
}
