
namespace CAFE.Core.Configuration
{
    /// <summary>
    /// Email parameters configuration
    /// like host, port, credentials, etc.
    /// </summary>
    public sealed class EmailConfiguration : IConfiguration
    {
        /// <summary>
        /// SMTP server host name
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// SMTP server port number (from 1 to 65535) default is: 25, 465, 587
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// Indicate that SMTP server requires SSL connection
        /// </summary>
        public bool IsSslRequired { get; set; }

        /// <summary>
        /// Login for authorize on SMTP server. Usualy it's may be the email address
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Password for login or email address
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Name that print as "from ..."
        /// </summary>
        public string NameFrom { get; set; }
        public string TemplatesPath { get; set; }
    }
}
