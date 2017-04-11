using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAFE.Core.Security
{
    /// <summary>
    /// Contract for services that manage users by async manner
    /// </summary>
    public interface ISecurityServiceAsync
    {
        #region User Services Async
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<IEnumerable<User>> GetUnacceptedUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUserNameAsync(string userName);
        Task<User> CreateUserAsync();
        Task<User> SaveUserAsync(User user);
        Task SaveUserBatchAsync(IEnumerable<User> users);
        Task<bool> RemoveUserAsync(User user, bool removeOwnData);
        Task<User> AcceptUserAsync(string userId);
        Task<IEnumerable<User>> SearchUsersAsync(string keyWord);

        #endregion

        #region Group Services Async
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task<Group> GetGroupByIdAsync(string id);
        Task<Group> CreateGroupAsync();
        Task<Group> AddGroupAsync(Group group);
        Task<bool> AddGroupUserAsync(string id);
        Task<bool> RemoveGroupUserAsync(string id);
        Task<bool> RemoveGroupAsync(Group group);
        Task<IEnumerable<Group>> SearchGroupsAsync(string keyWord);
        #endregion

        #region UserFile Services Async
        Task<IEnumerable<UserFile>> GetUserFilesByUserIdAsync(string id, bool withAccesseble = false);
        Task<UserFile> GetUserFileByIdAsync(string id);
        Task<UserFile> CreateUserFileAsync();
        Task<bool> AddUserFilesAsync(List<UserFile> userFiles);
        Task<bool> RemoveUserFilesAsync(List<UserFile> userFiles);
        Task<bool> UpdateUserFileAsync(UserFile userFile);
        Task<User> ChangeUserFileOwnerAsync(UserFile userFile);
        #endregion

    }
}
