using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Areas.Api.Models
{
    public class AnnotationItemImportModel
    {
        public enum AIImportTypes
        {
            UploadedFile,
            FileFromURL,
            FromUserFiles
        };

        public string ExtendableData { get; set; }
        public AIImportTypes DataType { get; set; }
        public string ExtendableDataName { get; set; }

        public string TransformationData { get; set; }
        public string TransformatioDataName { get; set; }
        public AIImportTypes TransformationDataType { get; set; }

        public bool UseTransormation { get; set; }
        public bool SaveFileAfterUpload { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}