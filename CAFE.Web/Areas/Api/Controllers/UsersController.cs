using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CAFE.Core.Misc;
using CAFE.Core.Security;
using CAFE.Web.Areas.Api.Models;
using System.Web.Security;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace CAFE.Web.Areas.Api.Controllers
{
    [Authorize(Roles = Core.Misc.Constants.AdminRoleName)]
    public class UsersController : ApiController
    {
        private readonly ISecurityService _securityService;
        private readonly ISecurityServiceAsync _securityServiceAsync;
        private Microsoft.AspNet.Identity.UserManager<User> _userManager;

        public Microsoft.AspNet.Identity.UserManager<User> UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<Microsoft.AspNet.Identity.UserManager<User>>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public UsersController(Microsoft.AspNet.Identity.UserManager<User> userManager, ISecurityService securityService, ISecurityServiceAsync securityServiceAsync)
        {
            UserManager = userManager;
            _securityService = securityService;
            _securityServiceAsync = securityServiceAsync;
        }

        [HttpGet]
        public async Task<IEnumerable<UserViewModel>> GetUserList()
        {
            var users = await _securityServiceAsync.GetAllUsersAsync();
            var userModels = Mapper.Map<IEnumerable<User>, List<UserViewModel>>(users);
            return userModels;
        }

        [HttpGet]
        public async Task<IEnumerable<UserViewModel>> GetActiveUserList()
        {
            var users = await _securityServiceAsync.GetActiveUsersAsync();
            var userModels = Mapper.Map<IEnumerable<User>, List<UserViewModel>>(users);
            return userModels;
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateUser(UserViewModel model)
        {
            var foundUser = await _securityServiceAsync.GetUserByIdAsync(model.Id);
            var mappedUser = Mapper.Map(model, foundUser);
            await _securityServiceAsync.SaveUserAsync(mappedUser);
            return Ok();
        }

        [HttpPost]
        public async Task<IEnumerable<UserAcceptanceViewModel>> GetUserAcceptanceList()
        {
            var users = await _securityServiceAsync.GetActiveUsersAsync();
            var mappedUsers = Mapper.Map<List<User>, IEnumerable<UserAcceptanceViewModel>>(users.ToList());
            return mappedUsers;
        }

        [HttpPost]
        public async Task<IEnumerable<UserAcceptanceViewModel>> GetUnnacceptedUsersList()
        { 
            var isUserAdmin = User.IsInRole(Constants.AdminRoleName);

            if (!isUserAdmin)
                return null;

            var users = await _securityServiceAsync.GetUnacceptedUsersAsync();
            var mappedUsers = Mapper.Map<List<User>, IEnumerable<UserAcceptanceViewModel>>(users.ToList());
            return mappedUsers;
        }

        [HttpPost]
        public async Task<IHttpActionResult> AcceptUser([FromBody]string model)
        {
            var user = await _securityServiceAsync.AcceptUserAsync(model);
            try
            {
                var message = System.String.Format(
                        Messages.AccountRegistration_Messages_Emails_AcceptedAccountEmail,
                        user.Name,
                        user.Surname,
                        Url.Link("Default", new { Controller = "Account", Action = "Login" }));

                await UserManager.SendEmailAsync(
                    model,
                    Messages.AccountRegistration_Messages_Emails_AcceptedAccountEmailSubject,
                    message);

                return Ok();
            }
            catch(System.Exception ex){
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> DeleteUsers([FromBody] DeleteUsersModel model)
        {
            foreach (var userId in model.UsersIds)
                await _securityServiceAsync.RemoveUserAsync(await _securityServiceAsync.GetUserByIdAsync(userId), model.RemoveOwnData);

            return Ok();
        }
    }
}