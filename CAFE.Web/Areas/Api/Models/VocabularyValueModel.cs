using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Areas.Api.Models
{
    public class VocabularyValueModel
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public string Uri { get; set; }
        public string Description { get; set; }
    }
}