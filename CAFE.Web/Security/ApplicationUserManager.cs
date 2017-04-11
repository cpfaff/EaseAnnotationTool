
using System;
using CAFE.Core.Notification;
using CAFE.Core.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace CAFE.Web.Security
{
    public class ApplicationUserManager : UserManager<User>
    {
        private readonly IdentityFactoryOptions<ApplicationUserManager> _options;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public ApplicationUserManager(IUserStore<User> store, 
            IdentityFactoryOptions<ApplicationUserManager> options,
            IEmailService emailService,
            ISmsService smsService)
            : base(store)
        {
            _options = options;
            _emailService = emailService;
            _smsService = smsService;

            Config();
        }

        private void Config()
        {
            // Configure validation logic for usernames
            UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = false;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is {0}"
            });
            var emailTokenProvider = new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            };
            RegisterTwoFactorProvider("Email Code", emailTokenProvider);
            EmailService = _emailService as IIdentityMessageService;
            SmsService = _smsService as IIdentityMessageService;

            
            var dataProtectionProvider = _options.DataProtectionProvider;
            if (dataProtectionProvider == null)
            {
                dataProtectionProvider = new DpapiDataProtectionProvider("CAFE.Web");
            }
            UserTokenProvider =
                new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));

        }
    }
}