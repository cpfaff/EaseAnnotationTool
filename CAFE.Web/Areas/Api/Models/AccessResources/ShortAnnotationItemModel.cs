
using System;

namespace CAFE.Web.Areas.Api.Models.AccessResources
{
    public class ShortAnnotationItemModel : CAFE.Core.Resources.AccessibleResourceDescriptorBase
    {
        public DateTime CreationDate { get; set; }
        public new string AccessMode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}