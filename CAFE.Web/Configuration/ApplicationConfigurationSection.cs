
using System.Configuration;

namespace CAFE.Web.Configuration
{
    /// <summary>
    /// Web/App.config section for store global and common parameters configuration
    /// </summary>
    public class ApplicationConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Indicate that user must be accepted by admin afret its registration
        /// </summary>
        [ConfigurationProperty("IsUsersAcceptanceNeed", DefaultValue = false, IsRequired = true)]
        public bool IsUsersAcceptanceNeed
        {
            get
            {
                return (bool)this["IsUsersAcceptanceNeed"];
            }
            set
            {
                this["IsUsersAcceptanceNeed"] = value;
            }
        }


        /// <summary>
        /// Gets or sets User uploads file root directory
        /// </summary>
        [ConfigurationProperty("ApploadsRoot", DefaultValue = "~/App_Data/Uploads/", IsRequired = true)]
        public string ApploadsRoot
    {
            get
            {
                return (string)this["ApploadsRoot"];
            }
            set
            {
                this["ApploadsRoot"] = value;
            }
        }

        /// <summary>
        /// Indicate that can changes
        /// </summary>
        /// <returns></returns>
        public override bool IsReadOnly()
        {
            return false;
        }
    }
}