
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using Microsoft.AspNet.Identity;

namespace CAFE.Web.Security
{
    public class RepositoryBasedUserStore : IUserStore<User>, IUserPasswordStore<User>, 
        IUserLockoutStore<User, string>, IUserEmailStore<User>, IUserTwoFactorStore<User, string>,
        IUserPhoneNumberStore<User>, IUserSecurityStampStore<User>, IUserRoleStore<User>, 
        IUserLoginStore<User>
    {
        private readonly IRepository<DbUser> _repository;
        private readonly IRepository<DbRole> _roleRepository;
        private readonly ISecurityServiceAsync _securityServiceAsync;

        public RepositoryBasedUserStore(
            IRepository<DbUser> repository, 
            IRepository<DbRole> roleRepository, 
            ISecurityServiceAsync securityServiceAsync
        )
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _securityServiceAsync = securityServiceAsync;
        }

        #region IDisposable implementation

        public void Dispose()
        {
            
        }

        #endregion

        #region IUserStore implementation

        public async Task CreateAsync(User user)
        {
            await Task.Run(() =>
            {
                try
                {
                    user.IsActive = true;
                    var dbUser = Mapper.Map(user, new DbUser());
                    dbUser.Id = Guid.NewGuid();
                    _repository.Insert(dbUser);
                    Mapper.Map(dbUser, user);
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });

        }   

        public async Task UpdateAsync(User user)
        {
            await _securityServiceAsync.SaveUserAsync(user);
        }

        public async Task DeleteAsync(User user)
        {
            await Task.Run(() =>
            {
                try
                {
                    var dbUser = _repository.Find(f => f.Id.ToString() == user.Id);
                    Mapper.Map(user, dbUser);
                    dbUser.Roles.Clear();
                    _repository.Update(dbUser);
                    _repository.Delete(dbUser);
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task<User> FindByIdAsync(string userId)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return Mapper.Map(_repository.Find(f => f.Id.ToString().ToLower() == userId.ToLower() && f.IsActive), new User());
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var dbUser = _repository.Find(f => f.UserName.ToLower() == userName.ToLower() && f.IsActive);

                    return Mapper.Map(dbUser, new User());
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        #endregion

        #region IUserPasswordStore implementation
        public async Task SetPasswordHashAsync(User user, string passwordHash)
        {  
            await Task.Run(() =>
            {

                try
                {
                    user.PasswordHash = passwordHash;
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });

        }

        public async Task<string> GetPasswordHashAsync(User user)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return user.PasswordHash;
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task<bool> HasPasswordAsync(User user)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return !string.IsNullOrEmpty(user.PasswordHash);
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        #endregion

        #region IUserLockoutStore implementation
        public async Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return await Task.Run(() =>
            {
                if (user.LockoutEndDateUtc != null)
                    return System.DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Local);
                return default(DateTimeOffset);
            });
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user)
        {            
            return await Task.Run(() =>
            {
                try
                {
                    user.AccessFailedCount++;
                    var dbUser = _repository.Find(u => u.Id.ToString() == user.Id);
                    _repository.Update(Mapper.Map(user, dbUser));
                    return user.AccessFailedCount;
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task ResetAccessFailedCountAsync(User user)
        {
            await Task.Run(() =>
            {
                try
                {
                    user.AccessFailedCount = 0;
                    var dbUser = _repository.Find(u => u.Id.ToString() == user.Id);
                    _repository.Update(Mapper.Map(user, dbUser));
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task<int> GetAccessFailedCountAsync(User user)
        {
            return await Task.Run(() =>
            {
                return user.AccessFailedCount;
            });
        }

        public async Task<bool> GetLockoutEnabledAsync(User user)
        {
            return await Task.Run(() =>
            {
                return user.LockoutEnabled;
            });
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            await Task.Run(() =>
            {
                try
                {
                    user.LockoutEnabled = enabled;
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            await Task.Run(() =>
            {
                try
                {
                    user.LockoutEndDateUtc = lockoutEnd.DateTime;
                    var dbUser = _repository.Find(u => u.Id.ToString() == user.Id);
                    _repository.Update(Mapper.Map(user, dbUser));
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        #endregion

        #region IUserEmailStore implementation
        public async Task SetEmailAsync(User user, string email)
        {
            await Task.Run(() =>
            {

                try
                {
                    user.Email = email;
                    var dbUser = _repository.Find(u => u.Id.ToString() == user.Id);
                    _repository.Update(Mapper.Map(user, dbUser));
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task<string> GetEmailAsync(User user)
        {
            return await Task.Run(() => user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user)
        {
            return await Task.Run(() => user.EmailConfirmed);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            await Task.Run(() =>
            {
                try
                {
                    user.EmailConfirmed = confirmed;

                    var dbUser = _repository.Find(u => u.Id.ToString() == user.Id);
                    _repository.Update(Mapper.Map(user, dbUser));
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return Mapper.Map(_repository.Find(f => f.Email.ToLower() == email.ToLower()), new User());
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }


        #endregion

        #region IUserTwoFactorStore implementation
        public async Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            await Task.Run(() =>
            {
                try
                {
                    user.TwoFactorEnabled = enabled;
                    //var dbUser = _repository.Find(u => u.Id.ToString() == user.Id);
                    //_repository.Update(Mapper.Map(user, dbUser));
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return await Task.Run(() => user.TwoFactorEnabled);
        }

        #endregion

        #region IUserPhoneNumberStore implementation
        public async Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            await Task.Run(() =>
            {
                try
                {
                    user.PhoneNumber = phoneNumber;
                    //var dbUser = _repository.Find(u => u.Id.ToString() == user.Id);
                    //_repository.Update(Mapper.Map(user, dbUser));
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task<string> GetPhoneNumberAsync(User user)
        {
            return await Task.Run(() => user.PhoneNumber);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            return await Task.Run(() => user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            await Task.Run(() =>
            {
                try
                {
                    user.PhoneNumberConfirmed = confirmed;
                    //var dbUser = _repository.Find(u => u.Id.ToString() == user.Id);
                    //_repository.Update(Mapper.Map(user, dbUser));
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        #endregion

        #region IUserSecurityStampStore implementation

        public async Task SetSecurityStampAsync(User user, string stamp)
        {
            await Task.Run(() =>
            {
                try
                {
                    user.SecurityStamp = stamp;
                    //var dbUser = _repository.Find(u => u.Id.ToString() == user.Id);
                    //_repository.Update(Mapper.Map(user, dbUser));

                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task<string> GetSecurityStampAsync(User user)
        {
            return await Task.Run(() => user.SecurityStamp);
        }

        #endregion

        #region IUserRoleStore implementation
        public async Task AddToRoleAsync(User user, string roleName)
        {
            await Task.Run(() =>
            {

                try
                {
                    var dbUser = Mapper.Map(user, _repository.Find(f => f.Id.ToString() == user.Id));

                    //var foundRole = _roleRepository.SelectNoTracking().Where(f => f.Name == roleName).FirstOrDefault();
                    var foundRole = _roleRepository.Find(f => f.Name == roleName);
                    if (foundRole == null)
                    {
                        foundRole = new DbRole();
                        foundRole.Name = roleName;
                        foundRole.IsGroup = false;
                        //foundRole.Id = Guid.NewGuid();

                        _roleRepository.Insert(foundRole);
                    }
                    dbUser.Roles.Add(foundRole);
                    _repository.Update(dbUser);

                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task RemoveFromRoleAsync(User user, string roleName)
        {
            try
            {
                await Task.Run(() =>
                {
                    var dbUser = Mapper.Map(user, _repository.Find(f => f.Id.ToString() == user.Id));
                    var foundRole = _roleRepository.Find(f => f.Name == roleName);
                    dbUser.Roles.Remove(dbUser.Roles.FirstOrDefault(r => r.Id == foundRole.Id));
                    _repository.Update(dbUser);
                });
            }
            catch (Exception exception)
            {
                //TODO: log here
                throw;
            }
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var dbUser = _repository.Find(f => f.Id.ToString() == user.Id);
                    return dbUser.Roles.Select(s => s.Name).ToList();
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var dbUser = Mapper.Map(user, _repository.Find(f => f.Id.ToString() == user.Id));
                    return dbUser.Roles.Any(r => r.Name == roleName);
                }
                catch (Exception exception)
                {
                    //TODO: log here
                    throw;
                }
            });
        }
        #endregion

        #region IUserLoginStore implementation
        public async Task AddLoginAsync(User user, UserLoginInfo login)
        {
            await Task.Delay(0);
        }

        public async Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            await Task.Delay(0);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            return await Task.Run(() => new List<UserLoginInfo>());
        }

        public async Task<User> FindAsync(UserLoginInfo login)
        {
            return await Task.Run(() => default(User));
        }

        #endregion

    }
}
