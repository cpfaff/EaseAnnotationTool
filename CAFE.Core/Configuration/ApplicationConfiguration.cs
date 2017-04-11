
namespace CAFE.Core.Configuration
{
    /// <summary>
    /// Global application parameters/option configuration
    /// </summary>
    public sealed class ApplicationConfiguration : IConfiguration
    {
        /// <summary>
        /// Indicate that user must be accepted by admin afret its registration
        /// </summary>
        public bool IsUsersAcceptanceNeed { get; set; }


        /// <summary>
        /// Gets or sets User uploads file root directory
        /// </summary>
        public string ApploadsRoot { get; set; }
    }
}
