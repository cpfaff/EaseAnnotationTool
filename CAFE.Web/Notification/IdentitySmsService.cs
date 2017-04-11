
using System.Threading.Tasks;
using CAFE.Core.Notification;
using Microsoft.AspNet.Identity;

namespace CAFE.Web.Notification
{
    public class IdentitySmsService : ISmsService, IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await Task.Delay(0);
        }
    }
}