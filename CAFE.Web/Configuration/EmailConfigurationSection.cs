
using System.Configuration;

namespace CAFE.Web.Configuration
{
    /// <summary>
    /// Web/App.config section for store email parameters configuration
    /// </summary>
    public class EmailConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// SMTP server host name
        /// </summary>
        [ConfigurationProperty("SmtpHost", DefaultValue="some.smtp.com", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 255)]
        public string SmtpHost
        {
            get
            {
                return (string)this["SmtpHost"];
            }
            set
            {
                this["SmtpHost"] = value;
            }
        }

        /// <summary>
        /// SMTP server port number (from 1 to 65535) default is: 25, 465, 587
        /// </summary>
        [ConfigurationProperty("SmtpPort", DefaultValue = 25, IsRequired = true)]
        [IntegerValidator(ExcludeRange = false, MinValue = 1, MaxValue = 65535)]
        public int SmtpPort
        {
            get
            {
                return (int)this["SmtpPort"];
            }
            set
            {
                this["SmtpPort"] = value;
            }
        }

        /// <summary>
        /// Indicate that SMTP server requires SSL connection
        /// </summary>
        [ConfigurationProperty("IsSslRequired", DefaultValue = false, IsRequired = true)]
        public bool IsSslRequired
        {
            get
            {
                return (bool)this["IsSslRequired"];
            }
            set
            {
                this["IsSslRequired"] = value;
            }
        }

        /// <summary>
        /// Login for authorize on SMTP server. Usualy it's may be the email address
        /// </summary>
        [ConfigurationProperty("Login", DefaultValue = "admin@domain.com", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 255)]
        public string Login
        {
            get
            {
                return (string)this["Login"];
            }
            set
            {
                this["Login"] = value;
            }
        }

        /// <summary>
        /// Password for login or email address
        /// </summary>
        [ConfigurationProperty("Password", DefaultValue = "Qwerty1;", IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 16)]
        public string Password
        {
            get
            {
                return (string)this["Password"];
            }
            set
            {
                this["Password"] = value;
            }
        }

        /// <summary>
        /// Name that print as "from ..."
        /// </summary>
        [ConfigurationProperty("NameFrom", DefaultValue = "Admin from domain.com", IsRequired = false)]
        [StringValidator(MinLength = 1, MaxLength = 255)]
        public string NameFrom
        {
            get
            {
                return (string)this["NameFrom"];
            }
            set
            {
                this["NameFrom"] = value;
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

        /// <summary>
        /// Email templates path
        /// </summary>
        [ConfigurationProperty("TemplatesPath", DefaultValue = "~/App_Data/", IsRequired = false)]
        [StringValidator(MinLength = 1, MaxLength = 500)]
        public string TemplatesPath
        {
            get
            {
                return (string)this["TemplatesPath"];
            }
            set
            {
                this["TemplatesPath"] = value;
            }
        }
        
    }
}