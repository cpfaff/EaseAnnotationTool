using System.Collections.Generic;

namespace CAFE.Core.Security
{
    /// <summary>
    /// Contract for services that manage users
    /// </summary>
    public interface ISecurityService
    {
        #region User Services
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetActiveUsers();
        IEnumerable<User> GetUnacceptedUsers();
        User GetUserById(string id);
        User GetByEmail(string email);
        User GetByUserName(string userName);
        User CreateUser();
        User SaveUser(User user);
        void SaveUserBatch(IEnumerable<User> users);
        bool RemoveUser(User user, bool removeOwnData);
        IEnumerable<User> SearchUsers(string keyWord);
        User AcceptUser(string userId);

        #endregion

        #region Group Services
        IEnumerable<Group> GetAllGroups();
        Group GetGroupById(string id);
        Group GetGroupByName(string groupName);
        Group CreateGroup();
        Group AddGroup(Group group);
        bool RemoveGroup(Group group);

        bool AddGroupUser(string id);
        bool RemoveGroupUser(string id);

        bool UpdateGroup(string groupId, string groupName, List<string> userAddedIds, List<string> userDeletedIds);
        IEnumerable<Group> SearchGroups(string keyWord);
        #endregion

        #region UserFile Services
        IEnumerable<UserFile> GetUserFilesByUserId(string id, bool withAccesseble);
        UserFile GetUserFileById(string id);
        UserFile CreateUserFile();
        bool AddUserFiles(List<UserFile> userFiles);
        bool RemoveUserFiles(List<UserFile> userFiles);
        bool UpdateUserFile(UserFile userFile);
        User ChangeUserFileOwner(UserFile userFile);
        #endregion
    }
}
