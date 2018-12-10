

using System;

namespace CAFE.Web.Areas.Api.Models
{
    public class TaxonomyImportModel
    {
        public string ExtendableData { get; set; }
        public AnnotationItemImportModel.AIImportTypes DataType { get; set; }
        public string ExtendableDataName { get; set; }

        public bool SaveFileAfterUpload { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}