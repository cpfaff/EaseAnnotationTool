
using System.Collections.Generic;

namespace CAFE.Web.Areas.Api.Models
{
    public class WizardDataModel
    {
        public IEnumerable<string> FilesIds { get; set; }
        public string AnnotationItemId { get; set; }
    }
}