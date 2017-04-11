
namespace CAFE.Web.Areas.Api.Models.AccessResources
{
    /// <summary>
    /// Model of access wrapper for requested resource
    /// </summary>
    public class AccessibleResourceModel
    {
        public long Id { get; set; }
        /// <summary>
        /// Name of resource
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Resource description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Resource identifier
        /// </summary>
        public string ResourceId { get; set; }
        /// <summary>
        /// Resource owner unique identifier
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// Resource owner name
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Kind of resource content (or simple "Resource")
        /// </summary>
        public string Kind { get; set; }
    }
}