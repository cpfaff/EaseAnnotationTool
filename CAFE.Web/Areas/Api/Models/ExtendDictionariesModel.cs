using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Areas.Api.Models
{
    public class ExtendDictionariesModel
    {
        public enum ImportFileTypes
        {
            Manual,
            Excel,
            CSV
        };

        public string ExtendableData { get; set; }
        public ImportFileTypes DataType { get; set; }
        public bool IsUrl { get; set; }
        public string DictionaryName { get; set; }
        public string Description { get; set; }
    }
}