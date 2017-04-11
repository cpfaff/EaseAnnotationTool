using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Areas.Api.Models
{
    public class VocabularyUserValueModel
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Owner { get; set; }
        public DateTime CreationDate { get; set; }
    }
}