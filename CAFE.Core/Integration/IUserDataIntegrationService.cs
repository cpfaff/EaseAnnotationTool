

using System.Collections.Generic;
using CAFE.Core.Security;

namespace CAFE.Core.Integration
{
    /// <summary>
    /// Contract for Import/Export user's data (AnnotationItems, Files and Vocabulary extensions)
    /// </summary>
    public interface IUserDataIntegrationService
    {
        /// <summary>
        /// Exports all user's data as pack
        /// </summary>
        /// <param name="user">User who data is exporting</param>
        /// <param name="path">Path where export to</param>
        /// <returns>Exported data (base64 string)</returns>
        string ExportData(User user, string path, string host = "");

        /// <summary>
        /// Imports all user's data as pack
        /// </summary>
        /// <param name="user">User who data is importing</param>
        /// <param name="data">Importing data (base64 string)</param>
        /// <param name="path">Path where files get from</param>
        void ImportData(User user, string data, string path);

        /// <summary>
        /// Exports several annotation items into archive
        /// </summary>
        /// <param name="user">User who annotation items is importing</param>
        /// <param name="path">Path where to save</param>
        /// <param name="annotationItemsIds">List of annotaion identifier</param>
        /// <returns>Archive url</returns>
        string ExportAnnotationItems(User user, string path, IEnumerable<string> annotationItemsIds, string host = "");
    }
}
