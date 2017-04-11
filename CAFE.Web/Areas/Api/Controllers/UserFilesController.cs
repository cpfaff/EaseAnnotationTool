using AutoMapper;
using CAFE.Core.Security;
using CAFE.Web.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using CAFE.DAL.Interfaces;

namespace CAFE.Web.Controllers
{
    //[System.Web.Http.Authorize]
    public class UserFilesController : System.Web.Http.ApiController
    {
        private readonly ISecurityService _userFilesService;
        private readonly ISecurityServiceAsync _userFilesServiceAsync;
        private readonly Core.Configuration.IConfigurationProvider _configurationProvider;
        private readonly IRepository<DAL.Models.DbAnnotationItem> _annotationRepository;

        public UserFilesController(
            ISecurityService userFilesService, 
            ISecurityServiceAsync userFilesServiceAsync, 
            Core.Configuration.IConfigurationProvider configurationProvider,
            IRepository<DAL.Models.DbAnnotationItem> annotationRepository)
        {
            _userFilesService = userFilesService;
            _userFilesServiceAsync = userFilesServiceAsync;
            _configurationProvider = configurationProvider;
            _annotationRepository = annotationRepository;
        }

        private UserFile.FileType GetFileContentType(string MIMEType)
        {
            if (MIMEType.Contains("audio/"))
                return UserFile.FileType.Audio;
            else if (MIMEType.Contains("video/"))
                return UserFile.FileType.Video;
            else  if (MIMEType.Contains("application/vnd.ms-excel"))
                return UserFile.FileType.Tabular;
            else if (MIMEType.Contains("image/"))
                return UserFile.FileType.Image;
            else
                return UserFile.FileType.Other;
        }
        public string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }
        /// <summary>
        /// Returns a list of files owned by the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<UserFilesViewModel>> GetUserFilesList([System.Web.Http.FromUri]string userId)
		{
		    if (string.IsNullOrEmpty(userId) && !HttpContext.Current.User.Identity.IsAuthenticated) return new List<UserFilesViewModel>();
            var includeAccesibleFiles = true;

            if (string.IsNullOrEmpty(userId))
            {
                userId = User.Identity.GetUserId();
                includeAccesibleFiles = false;
            }

            var userFiles = await _userFilesServiceAsync.GetUserFilesByUserIdAsync(userId, includeAccesibleFiles);
            var userFilesModels = Mapper.Map<IEnumerable<UserFile>, List<UserFilesViewModel>>(userFiles);

            var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();

            var i = 0;
            foreach (var fileModel in userFilesModels)
            {
                var fileExtension = Path.GetExtension(fileModel.Name);
                fileModel.DownloadURL = GetBaseUrl() + "Api/UserFiles/GetUserFile/" + fileModel.Id;
                fileModel.SelectedUsersAndGroups = new List<UsersAndGroupsSearchResultsModel>();

                foreach (var user in userFiles.ToList()[i].AcceptedUsers)
                {
                    fileModel.SelectedUsersAndGroups.Add(new UsersAndGroupsSearchResultsModel
                    {
                        Id = user.Id,
                        IsGroup = false,
                        Name = user.Name + " " + user.Surname
                    });
                }

                foreach (var group in userFiles.ToList()[i].AcceptedGroups)
                {
                    fileModel.SelectedUsersAndGroups.Add(new UsersAndGroupsSearchResultsModel
                    {
                        Id = group.Id,
                        IsGroup = true,
                        Name = group.Name
                    });
                }

                i++;
            }

            return userFilesModels;
        }

        /// <summary>
		/// Adds new user file
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.Authorize]
        [HttpPost]
        public async Task<bool> AddUserFile()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage((System.Net.HttpStatusCode.UnsupportedMediaType)));
            }

            var context = HttpContext.Current.Request;
            var accessMode = (Core.Resources.AccessModes)int.Parse(context.Form["AccessMode"]);
            var fileDescription = context.Form["Description"];
            var selectedUSersAndGroups = context.Form["UsersAndGroups"];

            var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
            var basePath = appConfig.ApploadsRoot;
            var userFilesPath = Path.Combine(basePath, "UserFiles");

            var acceptedUsers = new List<User>();
            var acceptedGroups = new List<Core.Security.Group>();
            var userFilesListToAdd = new List<UserFile>();

            if (!String.IsNullOrEmpty(selectedUSersAndGroups) && accessMode == Core.Resources.AccessModes.Explicit)
            {
                var usersAndGroups = JsonConvert.DeserializeObject<List<UsersAndGroupsSearchResultsModel>>(selectedUSersAndGroups);
                foreach (var item in usersAndGroups)
                {
                    if (item.IsGroup)
                        acceptedGroups.Add(new Core.Security.Group { Id = item.Id });

                    else
                        acceptedUsers.Add(new Core.Security.User { Id = item.Id });
                }
            }
            else
            {
                acceptedGroups = new List<Core.Security.Group>();
                acceptedUsers = new List<Core.Security.User>();
            }

            if (context.Files.Count > 0)
                for (var i = 0; i < context.Files.Count; i++)
                {
                    var file = context.Files[i];

                    var fileId = Guid.NewGuid();
                    var oldExtension = Path.GetExtension(file.FileName);

                    var newFileName = string.Concat(fileId, oldExtension);
                    var fileVirtualPath = userFilesPath + "/" + newFileName;
                    var userId = User.Identity.GetUserId();
                    file.SaveAs(HttpContext.Current.Server.MapPath(fileVirtualPath));

                    var fileModel = new UserFile
                    {
                        Id = fileId,
                        Name = file.FileName,
                        Description = fileDescription,
                        AcceptedUsers = acceptedUsers,
                        AcceptedGroups = acceptedGroups,
                        Owner = new User { Id = userId },
                        OwnerId = userId,
                        CreationDate = DateTime.Now,
                        AccessMode = accessMode,
                        Type = GetFileContentType(file.ContentType)
                    };
                    userFilesListToAdd.Add(fileModel);
                }

            await _userFilesServiceAsync.AddUserFilesAsync(userFilesListToAdd);
            return true;
        }

        /// <summary>
		/// Updates new user file
		/// </summary>
		/// <returns>true/false</returns>
		[System.Web.Http.Authorize]
        [HttpPost]
        public async Task<bool> UpdateUserFile([System.Web.Http.FromBody]UserFilesEditingModel model)
        {
            var accessMode = model.AccessMode;
            var fileDescription = model.Description;
            var selectedUSersAndGroups = model.UsersAndGroups;

            var acceptedUsers = new List<User>();
            var acceptedGroups = new List<Core.Security.Group>();
            var userFilesListToAdd = new List<UserFile>();

            if (accessMode == Core.Resources.AccessModes.Explicit)
            {
                foreach (var item in selectedUSersAndGroups)
                {
                    if (item.IsGroup)
                        acceptedGroups.Add(new Core.Security.Group { Id = item.Id });
                    else
                        acceptedUsers.Add(new Core.Security.User { Id = item.Id });
                }
            }
            else
            {
                acceptedGroups = new List<Core.Security.Group>();
                acceptedUsers = new List<Core.Security.User>();
            }

            var fileModel = new UserFile
            {
                Id = Guid.Parse(model.Id),
                Description = model.Description,
                AcceptedUsers = acceptedUsers,
                AcceptedGroups = acceptedGroups,
                AccessMode = accessMode
            };

            await _userFilesServiceAsync.UpdateUserFileAsync(fileModel);
            return true;
        }

        /// <summary>
        /// Sets access mode to files collection
        /// </summary>
        /// <returns>true/false</returns>
        [System.Web.Http.Authorize]
        [HttpPost]
        public async Task<bool> SetFilesAccessMode([System.Web.Http.FromBody]UserFilesEditingModel [] models)
        {
            foreach (var model in models)
            {
                var accessMode = model.AccessMode;
                var fileDescription = model.Description;
                var selectedUSersAndGroups = model.UsersAndGroups;

                var acceptedUsers = new List<User>();
                var acceptedGroups = new List<Core.Security.Group>();
                var userFilesListToAdd = new List<UserFile>();

                if (accessMode == Core.Resources.AccessModes.Explicit)
                {
                    foreach (var item in selectedUSersAndGroups)
                    {
                        if (item.IsGroup)
                            acceptedGroups.Add(new Core.Security.Group { Id = item.Id });
                        else
                            acceptedUsers.Add(new Core.Security.User { Id = item.Id });
                    }
                }
                else
                {
                    acceptedGroups = new List<Core.Security.Group>();
                    acceptedUsers = new List<Core.Security.User>();
                }

                var fileModel = new UserFile
                {
                    Id = Guid.Parse(model.Id),
                    Description = model.Description,
                    AcceptedUsers = acceptedUsers,
                    AcceptedGroups = acceptedGroups,
                    AccessMode = accessMode
                };

                await _userFilesServiceAsync.UpdateUserFileAsync(fileModel);
            }
            return true;
        }

        /// <summary>
		/// Change user's file owner
		/// </summary>
		/// <returns>true/false</returns>
		[System.Web.Http.Authorize]
        [HttpPost]
        public async Task<User> SwitchUserFileOwner([System.Web.Http.FromBody]Models.SwitchUserFileOwnerModel model)
        {
            var fileModel = new UserFile
            {
                Id = Guid.Parse(model.UserFileId),
                //Owner = new User{Id= model.UserFileNewUserId }
                OwnerId = model.UserFileNewUserId
            };

            return await _userFilesServiceAsync.ChangeUserFileOwnerAsync(fileModel);
        }

        /// <summary>
        /// Searches Users and Groups for explicit access mode
        /// </summary>
        /// <returns>UsersAndGroupsSearchResultsModel</returns>
        [System.Web.Http.Authorize]
        [HttpPost]
        public async Task<List<UsersAndGroupsSearchResultsModel>> SearchUsersAndGroups([System.Web.Http.FromBody]UserFilesSearchRequestModel searchModel)
        {
            var context = HttpContext.Current.Request;
            var resultCollection = new List<UsersAndGroupsSearchResultsModel>();

            var foundUsers = await _userFilesServiceAsync.SearchUsersAsync(searchModel.KeyWord);
            var foundGroups = await _userFilesServiceAsync.SearchGroupsAsync(searchModel.KeyWord);


            foreach (var item in foundUsers)
            {
                resultCollection.Add(new UsersAndGroupsSearchResultsModel
                {
                    Id = item.Id,
                    IsGroup = false,
                    Name = item.Name + " " + item.Surname
                });
            }

            foreach (var item in foundGroups)
            {
                resultCollection.Add(new UsersAndGroupsSearchResultsModel
                {
                    Id = item.Id,
                    IsGroup = true,
                    Name = item.Name
                });
            }

            return resultCollection;
        }

        /// <summary>
        /// Deletes user's file
        /// <returns>true/false</returns>
        [System.Web.Http.Authorize]
        [HttpPost]
        public async Task<object> UserFilesDelete([System.Web.Http.FromBody]List<UserFilesViewModel> models)
        {
            var currentUserId = User.Identity.GetUserId();
            var userFilesModels = Mapper.Map<List<UserFilesViewModel>, List<UserFile>>(models);
            var userAIs = _annotationRepository.FindCollection(ai => ai.OwnerId == currentUserId);
            var AIFileIds = new List<Guid>();

            foreach(var ai in userAIs)
            {
                if (ai.Object.Resources != null)
                {

                    foreach(var res in ai.Object.Resources)
                    {
                        if(res.OnlineResourcesSpecified)
                        {
                            foreach(var rs in res.OnlineResources)
                            {
                                var fileName = Path.GetFileNameWithoutExtension(rs.DownloadUrl);
                                Guid fileId;
                                if (Guid.TryParse(fileName, out fileId))
                                    AIFileIds.Add(fileId);

                            }
                        }
                    }

                }
            }

            var rejectedFiles = userFilesModels.Where(f => AIFileIds.Contains(f.Id)).ToList();
            if (rejectedFiles.Count > 0)
                return new
                {
                    AttachedToAIFiles = String.Join(", ", rejectedFiles.Select(f => f.Name)),
                    Success = false
                };

            var result = await _userFilesServiceAsync.RemoveUserFilesAsync(userFilesModels);

            var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
            var basePath = appConfig.ApploadsRoot;
            var userFilesPath = Path.Combine(basePath, "UserFiles");

            foreach (var file in models)
            {
                var fileExtension = Path.GetExtension(file.Name);
                var path = System.Web.Hosting.HostingEnvironment.MapPath("~/" + userFilesPath + '/' + file.Id + fileExtension);
                File.Delete(path);
            }

            return new {
                Success = true
            };
        }

        /// <summary>
        /// Deletes user's file
        /// <returns>true/false</returns>
        [HttpGet]
        public HttpResponseMessage GetUserFile([System.Web.Http.FromUri]string id)
        {
            var currentUserId = User.Identity.GetUserId();
            var fileModel = _userFilesService.GetUserFileById(id);
            var userHasAccess = fileModel.AcceptedUsers.FirstOrDefault(u => u.Id == currentUserId);
            var usersGroupHasAccess = fileModel.AcceptedUsers.FirstOrDefault(u => u.Id == currentUserId);

            var userHasAccessByGroup = fileModel.AcceptedGroups.FirstOrDefault(g => null != g.Users.FirstOrDefault(u => u.Id == currentUserId));

            if (
                (fileModel.AccessMode != Core.Resources.AccessModes.Public) && 
                (
                    (fileModel.AccessMode == Core.Resources.AccessModes.Private && fileModel.OwnerId != currentUserId) ||
                    (fileModel.AccessMode == Core.Resources.AccessModes.Explicit && fileModel.OwnerId != currentUserId && null == userHasAccess && null == userHasAccessByGroup)
                )
            )
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotAcceptable);

            var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
            var fileExtension = Path.GetExtension(fileModel.Name);
            var basePath = appConfig.ApploadsRoot.Replace("/", "\\") + "\\UserFiles\\";

            var filePath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + basePath + fileModel.Id + fileExtension;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            var result = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
            };

            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileModel.Name
                };

            result.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return result;
        }
    }
}