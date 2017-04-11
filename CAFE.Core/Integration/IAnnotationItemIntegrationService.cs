
using System.Collections.Generic;
using CAFE.Core.Security;

namespace CAFE.Core.Integration
{
    /// <summary>
    /// Service contract for import/export annotation item to and from other data structure
    /// </summary>
    public interface IAnnotationItemIntegrationService
    {
        /// <summary>
        /// Imports data structure as string and transform with stylesheet(optional)
        /// </summary>
        /// <param name="source">Data structure to import</param>
        /// <param name="stylesheet">Transformation stylesheet</param>
        /// <returns>Imported annotation item</returns>
        AnnotationItem ImportWithTransform(string source, string stylesheet = null);

        /// <summary>
        /// Exports annotation item to string
        /// </summary>
        /// <param name="annotationItem">Annotation item instance</param>
        /// <returns>Prepared annotation item data structure</returns>
        string Export(AnnotationItem annotationItem, string host = "");

    }
}
