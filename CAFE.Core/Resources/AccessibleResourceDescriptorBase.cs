
using System;

namespace CAFE.Core.Resources
{
    public class AccessibleResourceDescriptorBase : IAccessibleResourceDescriptor
    {
        public Guid Id { get; set; }
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        public AccessModes AccessMode { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
