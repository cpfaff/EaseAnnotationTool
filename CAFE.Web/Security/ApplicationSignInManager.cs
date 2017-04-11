
using System.Security.Claims;
using System.Threading.Tasks;
using CAFE.Core.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CAFE.Web.Security
{
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override async Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
            // Add custom user claims here
            return userIdentity;
        }

    }
}