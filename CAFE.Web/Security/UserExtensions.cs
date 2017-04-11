
using System.Security.Claims;
using System.Threading.Tasks;
using CAFE.Core.Security;
using Microsoft.AspNet.Identity;

namespace CAFE.Web.Security
{
    public static class UserExtensions
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this User user, ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}