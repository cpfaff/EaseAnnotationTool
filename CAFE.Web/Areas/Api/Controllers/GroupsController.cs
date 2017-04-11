
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using CAFE.Core.Security;
using CAFE.Web.Areas.Api.Models;
using CAFE.Core.Misc;

namespace CAFE.Web.Areas.Api.Controllers
{
    [Authorize(Roles = Constants.AdminRoleName)]
    public class GroupsController : ApiController
    {
        private readonly ISecurityService _securityService;

        public GroupsController(ISecurityService securityService)
        {
            _securityService = securityService;
        }


        [HttpGet]
        public GroupViewModel[] GetGroupList()
        {
            var groups = _securityService.GetAllGroups();
            var groupModels = Mapper.Map<IEnumerable<Group>, List<GroupViewModel>>(groups);

            return groupModels.ToArray();
        }

        [HttpPost]
        public GroupViewModel AddGroup(GroupViewModel group)
        {
           var groupDbModel = Mapper.Map<GroupViewModel, Group>(group);
           groupDbModel.Id = System.Guid.NewGuid().ToString();

           var newDbGroup = _securityService.AddGroup(groupDbModel);

           var newDbGroupView = Mapper.Map<Group, GroupViewModel>(newDbGroup);

            return newDbGroupView;
        }

        [HttpPost]
        public bool DeleteGroup(Group group)
        {
            return _securityService.RemoveGroup(group);
        }

        [HttpPost]
        public IEnumerable<UserViewModel> SearchUsers(User userModel)
        {
            var key = userModel.Name;
            var foundDbUSers = _securityService.SearchUsers(key);

            var usersCollectionView = Mapper.Map<IEnumerable<User>, List<UserViewModel>>(foundDbUSers);

            return usersCollectionView;
        }

        
        [HttpPost]
        public bool UpdateGroup(AddUserToGroupModel userAndGroupId)
        {
            return _securityService.UpdateGroup(
                groupId: userAndGroupId.groupId,
                groupName: userAndGroupId.groupName,
                userAddedIds: userAndGroupId.userAddedIds,
                userDeletedIds: userAndGroupId.userDeletedIds
            );
        }
        
    }
}