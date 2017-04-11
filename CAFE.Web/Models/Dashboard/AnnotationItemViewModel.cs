using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Models.Dashboard
{
    public class AnnotationItemViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CreationDate { get; set; }
        public string Description { get; set; }
    }
}