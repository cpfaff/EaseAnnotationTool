
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using System.Web.Configuration;
using System.Data.Entity;

namespace CAFE.Services.Security
{
    /// <summary>
    /// Service that manage Groups
    /// </summary>
    public sealed class SecurityService : ISecurityService, ISecurityServiceAsync
    {
        private readonly IRepository<DbRole> _groupRepository;
        private readonly IRepository<DbUser> _userRepository;
        private readonly IRepository<DbUserFile> _userFileRepository;
        private readonly IRepository<DbAnnotationItem> _annotationItemRepository;
        private readonly IRepository<DbRole> _rolesRepository;
        private readonly IRepository<DbUserHiddenHelper> _hiddenUserHelperRepository;
        
        public SecurityService
        (
            IRepository<DbUser> userRepository, 
            IRepository<DbRole> groupRepository, 
            IRepository<DbUserFile> fileRepository,
            IRepository<DbAnnotationItem> annotationItemRepository,
            IRepository<DbRole> rolesRepository ,
            IRepository<DbUserHiddenHelper> hiddenUserHelperRepository
        )
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _userFileRepository = fileRepository;
            _annotationItemRepository = annotationItemRepository;
            _rolesRepository = rolesRepository;
            _hiddenUserHelperRepository = hiddenUserHelperRepository;
        }
        #region IUserService implementation

        
        public IEnumerable<User> GetValidUsers()
        {
            try
            {
                var allDataUsers = _userRepository.FindCollection(u => u.IsActive == true);
                var mappedUsers = Mapper.Map<IEnumerable<DbUser>, List<User>>(allDataUsers);
                return mappedUsers;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }
        

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                var allDataUsers = _userRepository.Select().Include(i => i.Roles);
                var mappedUsers = Mapper.Map<IEnumerable<DbUser>, List<User>>(allDataUsers);
                return mappedUsers;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }


        public IEnumerable<User> GetActiveUsers()
        {
            try
            {
                var allDataUsers = _userRepository.FindCollection(u => u.IsAccepted == true && u.IsActive == true);
                var mappedUsers = Mapper.Map<IEnumerable<DbUser>, List<User>>(allDataUsers);
                return mappedUsers;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }
        public IEnumerable<User> GetUnacceptedUsers()
        {
            try
            {
                var allDataUsers = _userRepository.FindCollection(u => u.IsAccepted == false && u.IsActive == true);
                var mappedUsers = Mapper.Map<IEnumerable<DbUser>, List<User>>(allDataUsers);
                return mappedUsers;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public User GetUserById(string id)
        {
            var dbUser = _userRepository.Find(f => f.Id.ToString().ToLower() == id.ToLower());
            if (dbUser == null) return null;

            return Mapper.Map(dbUser, new User());
        }

        public User GetByEmail(string email)
        {
            var dbUser = _userRepository.Find(f => f.Email.ToLower() == email.ToLower());
            if (dbUser == null) return null;

            return Mapper.Map(dbUser, new User());
        }

        public User GetByUserName(string userName)
        {
            var dbUser = _userRepository.Find(f => f.UserName.ToLower() == userName.ToLower());
            if (dbUser == null) return null;

            return Mapper.Map(dbUser, new User());
        }

        public User CreateUser()
        {
            return new User();
        }

        public User SaveUser(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Id))
                {
                    var newUser = Mapper.Map(user, new DbUser());
                    newUser.Roles.Add(_rolesRepository.Find(f => f.Name == user.Role));
                    _userRepository.Insert(newUser);

                }
                else
                {
                    var dbUser = _userRepository.Find(f => f.Id.ToString() == user.Id);
                    user.Email = dbUser.Email;
                    Mapper.Map(user, dbUser, a =>
                    {
                        ICollection<DbUserFile> _accFiles = null;
                        ICollection<DbUserFile> _ownedFiles = null;
                        a.BeforeMap((s, d) =>
                        {
                            _accFiles = d.AccessibleFiles.ToList();
                            _ownedFiles = d.OwnedFiles.ToList();
                            s.AccessibleFiles = null;
                            s.OwnedFiles = null;
                        });
                        a.AfterMap((s, d) =>
                        {
                            d.AccessibleFiles = _accFiles;
                            d.OwnedFiles = _ownedFiles;
                        });
                    });
                    /*
                    var role = dbUser.Roles.Where(w => w.IsGroup == false).FirstOrDefault();
                    if (role != null)
                    {
                        dbUser.Roles.Remove(role);
                    }*/
                    _userRepository.Update(dbUser);
                    var newRole = _rolesRepository.Find(f => f.Name == user.Role);
                    if (newRole != null)
                    {
                        dbUser.Roles.Add(newRole);
                        _userRepository.Update(dbUser);
                    }
                }
                return Mapper.Map(_userRepository.Select().Where(u => u.UserName == user.UserName).First(), user);
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public void SaveUserBatch(IEnumerable<User> users)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveUser(User user, bool removeOwnData)
        {
            try
            {
                var dbUserToRemove = _userRepository.Find(f => f.Id.ToString() == user.Id);
                dbUserToRemove.IsActive = false;
                dbUserToRemove.Email = string.Concat(dbUserToRemove.Id.ToString().Replace("-", ""), dbUserToRemove.Email);

                var permanentUserName = WebConfigurationManager.AppSettings["PermanentUserName"];
                var dbPermanentUser = _userRepository.Find(u => u.UserName == permanentUserName);

                for (var i = 0; i < dbUserToRemove.OwnedFiles.Count; i++)
                {
                    var file = dbUserToRemove.OwnedFiles.ToList()[i];
                    if (removeOwnData && file.AcceptedUsers.Count == 0) 
                        _userFileRepository.Delete(file);
                    else
                    {
                        file.Owner = dbPermanentUser;
                        _userFileRepository.Update(file);
                    }
                }

                var usersAnnotationItems = _annotationItemRepository.FindCollection(a => a.OwnerId == dbUserToRemove.Id.ToString()).ToList();
                for (var i = 0; i < usersAnnotationItems.Count; i++)
                {
                    var annotationItem = usersAnnotationItems[i];
                    if (removeOwnData)
                        _annotationItemRepository.Delete(annotationItem);
                    else
                    {
                        annotationItem.OwnerId = dbPermanentUser.Id.ToString();
                        annotationItem.OwnerName = dbPermanentUser.UserName;

                        _annotationItemRepository.Update(annotationItem);
                    }
                }

                _userRepository.Update(dbUserToRemove);

                return true;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public IEnumerable<User> SearchUsers(string keyWord)
        {
            try
            {
                var dbResults =
                    _userRepository.FindCollection(
                        u =>
                            (u.UserName.ToLower().Contains(keyWord.ToLower()) ||
                            u.Name.ToLower().Contains(keyWord.ToLower()) ||
                            u.Surname.ToLower().Contains(keyWord.ToLower())) && 
                            (
                                u.IsAccepted == true && u.IsActive == true
                            )
                );
                var users = Mapper.Map(dbResults, new List<User>());
                return users;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public User AcceptUser(string userId)
        {
            try
            {
                var dbUser = _userRepository.Find(u => u.Id.ToString() == userId);
                dbUser.IsAccepted = true;
                _userRepository.Update(dbUser);

                return Mapper.Map(dbUser, new User());
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public void RemoveUserAcceptances(IEnumerable<Guid> userIds)
        {
            try
            {
                var dbUsers = _userRepository.FindCollection(c => userIds.Contains(c.Id)).ToList();
         
                foreach(var dbUser in dbUsers)
                    _userRepository.Delete(dbUser);
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public void AddOreRemoveUserHiddenHelper(Guid userId, string helperName)
        {
            var existingHelper = _hiddenUserHelperRepository.Find(h => h.Name == helperName);
            if (null == _hiddenUserHelperRepository.Find(h => h.Name == helperName))
                _hiddenUserHelperRepository.Insert(new DbUserHiddenHelper
                {
                    Id = Guid.NewGuid(),
                    Name = helperName,
                    UserId = userId
                });
            else
                _hiddenUserHelperRepository.Delete(existingHelper);
        }


        #endregion

        #region IUserServiceAsync implementation
        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await Task.Run(() => GetActiveUsers());
        }

        public async Task<IEnumerable<User>> GetValidUsersAsync()
        {
            return await Task.Run(() => GetValidUsers());
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await Task.Run(() => GetAllUsers());
        }

        public async Task<IEnumerable<User>> GetUnacceptedUsersAsync()
        {
            return await Task.Run(() => GetUnacceptedUsers());
        }
        public async Task<User> GetUserByIdAsync(string id)
        {
            return await Task.Run(() => GetUserById(id));
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await Task.Run(() => GetByEmail(email));
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await Task.Run(() => GetByUserName(userName));
        }

        public async Task<User> CreateUserAsync()
        {
            return await Task.Run(() => CreateUser());
        }

        public async Task<User> SaveUserAsync(User user)
        {
            return await Task.Run(() => SaveUser(user));
        }

        public async Task SaveUserBatchAsync(IEnumerable<User> users)
        {
            await Task.Run(() => SaveUserBatch(users));
        }

        public async Task<bool> RemoveUserAsync(User user, bool removeOwnData)
        {
            return await Task.Run(() => RemoveUser(user, removeOwnData));
        }

        public async Task<User> AcceptUserAsync(string userId)
        {
            return await Task.Run(() => AcceptUser(userId));
        }

        public async Task RemoveUserAcceptancesAsync(IEnumerable<Guid> userIds)
        {
            try
            {
                await Task.Run(() => RemoveUserAcceptances(userIds));
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string keyWord)
        {
            return await Task.Run(() => SearchUsers(keyWord));
        }

        #endregion

        #region IGroupservice implementation

        public IEnumerable<Group> GetAllGroups()
        {
            try
            {
                var allDataGroups = _groupRepository.Select().Where(g => g.IsGroup == true).Include(l => l.Users).ToList();

                var mappedGroups = Mapper.Map<IEnumerable<DbRole>, List<Group>>(allDataGroups);
                return mappedGroups;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public Group GetGroupById(string id)
        {
            var DbRole = _groupRepository.Find(f => f.Id.ToString().ToLower() == id.ToLower());
            if (DbRole == null) return null;

            return Mapper.Map(DbRole, new Group());
        }

        public Group GetGroupByName(string groupName)
        {
            var DbRole = _groupRepository.Find(f => f.Name.ToLower() == groupName.ToLower());
            if (DbRole == null) return null;

            return Mapper.Map(DbRole, new Group());
        }

        public Group CreateGroup()
        {
            return new Group();
        }

        public Group AddGroup(Group group)
        {
            try
            {
                group.IsGroup = true;
                var dbRole = new DbRole();
                _groupRepository.Insert(Mapper.Map(group, dbRole));
                group.Id = dbRole.Id.ToString();
                return group;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public bool RemoveGroup(Group Group)
        {
            try
            {
                var dbGroup = _groupRepository.Find(g => g.Id.ToString() == Group.Id);
                _groupRepository.Delete(dbGroup);
                return true;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public bool AddGroupUser(string id)
        {
            try
            {
                //var userRepository =  IRepository<DbUserRole>;

                return true;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public bool RemoveGroupUser(string id)
        {
            try
            {
                //var userRepository =  IRepository<DbUserRole>;

                return true;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public bool UpdateGroup(string groupId, string groupName, List<string> userAddedIds, List<string> userDeletedIds)
        {
            try
            {
                var dbGroup = _groupRepository.Find(g => g.Id.ToString() == groupId);
                var usersUpdated = new List<DbUser>(dbGroup.Users);

                var usersToRemove = dbGroup.Users.Where(u => userDeletedIds.Contains(u.Id.ToString()));
                foreach(var userToRemove in usersToRemove)
                    usersUpdated.Remove(userToRemove);

                var usersIdsToAdd = userAddedIds.Where(uId => null == dbGroup.Users.FirstOrDefault(dbu => dbu.Id.ToString() == uId));
               
                foreach(var userIdToAdd in usersIdsToAdd)
                {
                    var userGuid = Guid.Parse(userIdToAdd);
                    var dbUser = _userRepository.Find(u => u.Id == userGuid);
                    usersUpdated.Add(dbUser);
                }
                 
                dbGroup.Users = usersUpdated;
                dbGroup.Name = groupName;

                _groupRepository.Update(dbGroup);
                
                return true;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public IEnumerable<Group> SearchGroups(string keyWord)
        {
            try
            {
                var dbResults =
                    _groupRepository.FindCollection(
                        g =>
                            g.IsGroup == true && 
                            g.Name.ToLower().Contains(keyWord.ToLower()));
                var groups = Mapper.Map(dbResults, new List<Group>());
                return groups;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        #endregion

        #region IGroupserviceAsync implementation

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await Task.Run(() => GetAllGroups());
        }

        public async Task<Group> GetGroupByIdAsync(string id)
        {
            return await Task.Run(() => GetGroupById(id));
        }

        public async Task<Group> CreateGroupAsync()
        {
            return await Task.Run(() => new Group());
        }

        public async Task<Group> AddGroupAsync(Group Group)
        {
            return await Task.Run(() => AddGroup(Group));
        }

        public async Task<bool> RemoveGroupAsync(Group Group)
        {
            return await Task.Run(() => RemoveGroup(Group));
        }

        public async Task<bool> AddGroupUserAsync(string id)
        {
            return await Task.Run(() => AddGroupUser(id));
        }

        public async Task<bool> RemoveGroupUserAsync(string id)
        {
            return await Task.Run(() => RemoveGroupUser(id));
        }
        public async Task<IEnumerable<Group>> SearchGroupsAsync(string keyWord)
        {
            return await Task.Run(() => SearchGroups(keyWord));
        }

        #endregion

        #region IUserFileService implementation

        public IEnumerable<UserFile> GetUserFilesByUserId(string userId, bool withAccesseble = false)
        {
            try
            {
                var allDataUserFiles = new List<UserFile>();
                var user = GetUserById(userId);

                foreach(var file in user.OwnedFiles)
                    file.Permission = UserFile.FilePermission.Owner;

                foreach (var file in user.AccessibleFiles)
                    file.Permission = UserFile.FilePermission.Read;

                allDataUserFiles.AddRange(user.OwnedFiles);
                if (withAccesseble)
                    allDataUserFiles.AddRange(user.AccessibleFiles);

                return allDataUserFiles;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public UserFile GetUserFileById(string id)
        {
            var dbUserFile = _userFileRepository.Find(f => f.Id.ToString().ToLower() == id.ToLower());
            if (dbUserFile == null) return null;

            return Mapper.Map(dbUserFile, new UserFile());
        }

        public UserFile CreateUserFile()
        {
            return new UserFile();
        }

        public bool AddUserFiles(List<UserFile> userFiles)
        {
            try
            {
                var dbUserFiles = Mapper.Map<List<UserFile>, List<DbUserFile>>(userFiles);

                foreach (var userFile in dbUserFiles)
                {
                    userFile.Owner = _userRepository.Find(f => f.Id.ToString().ToLower() == userFile.Owner.Id.ToString());

                    if (userFile.AccessMode == DbUserFile.DbFileAccessMode.Explicit)
                    {
                        var allUsers = userFile.AcceptedUsers.ToList();
                        userFile.AcceptedUsers.Clear();

                        for (var i = 0; i < allUsers.Count; i++)
                        {
                            var userId = allUsers[i].Id.ToString().ToLower();
                            userFile.AcceptedUsers.Add(_userRepository.Find(f => f.Id.ToString().ToLower() == userId));
                        }

                        var allGroups = userFile.AcceptedGroups.ToList();
                        userFile.AcceptedGroups.Clear();
                        for (var i = 0; i < allGroups.Count; i++)
                        {
                            var groupId = allGroups[i].Id.ToString().ToLower();
                            userFile.AcceptedGroups.Add(_groupRepository.Find(f => f.Id.ToString().ToLower() == groupId));
                        }
                    }
                }

                _userFileRepository.InsertCollection(dbUserFiles);

                
                return true;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public bool UpdateUserFile(UserFile userFile)
        {
            try
            {
                var dbUserFile = _userFileRepository.Find(f => f.Id == userFile.Id);

                dbUserFile.Description = userFile.Description;
                dbUserFile.AccessMode = (DbUserFile.DbFileAccessMode)userFile.AccessMode;

                dbUserFile.AcceptedUsers.Clear();
                dbUserFile.AcceptedGroups.Clear();

                if (dbUserFile.AccessMode == DbUserFile.DbFileAccessMode.Explicit)
                {
                    for (var i = 0; i < userFile.AcceptedUsers.Count; i++)
                    {
                        var userId = userFile.AcceptedUsers.ToList()[i].Id.ToString().ToLower();
                        dbUserFile.AcceptedUsers.Add(_userRepository.Find(f => f.Id.ToString().ToLower() == userId));
                    }

                    for (var i = 0; i < userFile.AcceptedGroups.Count; i++)
                    {
                        var groupId = userFile.AcceptedGroups.ToList()[i].Id.ToString().ToLower();
                        dbUserFile.AcceptedGroups.Add(_groupRepository.Find(f => f.Id.ToString().ToLower() == groupId));
                    }
                }

                _userFileRepository.Update(dbUserFile);

                return true;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }
        public User ChangeUserFileOwner(UserFile userFile)
        {
            try
            {
                var dbUserFile = _userFileRepository.Find(f => f.Id == userFile.Id);
                dbUserFile.Owner = _userRepository.Find(u => u.Id.ToString() == userFile.OwnerId);
                _userFileRepository.Update(dbUserFile);

                return Mapper.Map(dbUserFile.Owner, new User());
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }
        
        public bool RemoveUserFiles(List<UserFile> userFiles)
        {
            try
            {
                foreach (var file in userFiles)
                { 
                    var dbFile = _userFileRepository.Find(f => f.Id == file.Id);
                    _userFileRepository.Delete(dbFile);
                }

                return true;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public IEnumerable<UserFile> SearchUserFiles(string keyWord)
        {
            try
            {
                var dbResults =
                    _userFileRepository.FindCollection(
                        u =>
                            u.Description.ToLower().Contains(keyWord.ToLower()) ||
                            u.Name.ToLower().Contains(keyWord.ToLower()));
                var userFiles = Mapper.Map(dbResults, new List<UserFile>());
                return userFiles;
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        #endregion

        #region IUserFileServiceAsync implementation

        public async Task<IEnumerable<UserFile>> GetUserFilesByUserIdAsync(string userId, bool withAccesseble = false)
        {
            return await Task.Run(() => GetUserFilesByUserId(userId, withAccesseble));
        }

        public async Task<UserFile> GetUserFileByIdAsync(string id)
        {
            return await Task.Run(() => GetUserFileById(id));
        }

        public async Task<UserFile> CreateUserFileAsync()
        {
            return await Task.Run(() => CreateUserFile());
        }
        public async Task<bool> AddUserFilesAsync(List<UserFile> userFiles)
        {
            return await Task.Run(() => AddUserFiles(userFiles));
        }
        public async Task<bool> UpdateUserFileAsync(UserFile userFile)
        {
            return await Task.Run(() => UpdateUserFile(userFile));
        }

        public async Task<User> ChangeUserFileOwnerAsync(UserFile userFile)
        {
            return await Task.Run(() => ChangeUserFileOwner(userFile));
        }

        public async Task<bool> RemoveUserFilesAsync(List<UserFile> userFiles)
        {
            return await Task.Run(() => RemoveUserFiles(userFiles));
        }

        public async Task<IEnumerable<UserFile>> SearchUserFileAsync(string keyWord)
        {
            return await Task.Run(() => SearchUserFiles(keyWord));
        }

        public async Task AddUserHiddenHelperAsync(Guid userId, string helperName)
        {
            await Task.Run(() => AddOreRemoveUserHiddenHelper(userId, helperName));
        }

        #endregion

    }
}

