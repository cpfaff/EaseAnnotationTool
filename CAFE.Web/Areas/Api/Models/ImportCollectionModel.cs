using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Areas.Api.Models
{
    public class ImportCollectionModel
    {
        public enum ImportCollectionFileTypes
        {
            Excel,
            CSV
        };

        public string ExtendableData { get; set; }
        public ImportCollectionFileTypes DataType { get; set; }
        public bool IsUrl { get; set; }
        public string AIClassName { get; set; }
    }
}