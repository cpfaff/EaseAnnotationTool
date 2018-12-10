using CAFE.Core.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Xml.Serialization;
using AutoMapper;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.Web.Areas.Api.Models.AccessResources;
using AquaticPhysiognomy = CAFE.Core.Integration.AquaticPhysiognomy;
using DataFormat = CAFE.Core.Integration.DataFormat;
using GeologicalTimePeriod = CAFE.Core.Integration.GeologicalTimePeriod;
using LandUse = CAFE.Core.Integration.LandUse;
using OroBiome = CAFE.Core.Integration.OroBiome;
using PedoBiome = CAFE.Core.Integration.PedoBiome;
using SemiAquaticPhysiognomy = CAFE.Core.Integration.SemiAquaticPhysiognomy;
using TerrestrialPhysiognomy = CAFE.Core.Integration.TerrestrialPhysiognomy;
using ZonoBiome = CAFE.Core.Integration.ZonoBiome;
using CAFE.Web.Models;
using CAFE.Core.Resources;
using static CAFE.Web.Areas.Api.Models.ExtendDictionariesModel;
using Microsoft.AspNet.Identity;
using CAFE.Core.Security;
using CAFE.Web.ValidationAttributes;
using static CAFE.Web.Areas.Api.Models.AnnotationItemImportModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using CAFE.Web.Areas.Api.Models;
using CAFE.Web.Models.Dashboard;
using static CAFE.Web.Areas.Api.Models.ImportCollectionModel;
using System.Linq.Expressions;
using CAFE.Services.Integration;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using CAFE.Core.Plugins;
using CAFE.Web.Integration;
using CsvHelper;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CAFE.Web.Areas.Api.Controllers
{
    //[Authorize]
    public class AnnotationItemController : ApiController
    {
        private readonly IRepository<DAL.Models.DbAnnotationItem> _annotationRepository;
        private readonly IRepository<DAL.Models.DbUser> _usersRepository;
        private readonly IRepository<DAL.Models.DbRole> _groupsRepository;
        private readonly IUserDataIntegrationService _userDataIntegrationService;
        private readonly IRepository<DAL.Models.DbAnnotationItemAccessibleUsers> _annotationItemAccessibleUsers;
        private readonly IRepository<DAL.Models.DbAnnotationItemAccessibleGroups> _annotationItemAccessibleGroups;
        private readonly IVocabularyService _vocabularyService;
        private readonly ISecurityService _securityService;
        private readonly ISecurityServiceAsync _securityServiceAsync;
        private readonly IPluginsProvider _pluginsProvider;
        private readonly IRepository<DbUIElement> _uiElementsRepository;
        private readonly IAnnotationItemIntegrationService _AIintegtationService;
        private readonly Core.Configuration.IConfigurationProvider _configurationProvider;

        public AnnotationItemController(
                IRepository<DAL.Models.DbAnnotationItem> annotationRepository,
                IRepository<DAL.Models.DbAnnotationItemAccessibleUsers> annotationItemAccessibleUsers,
                IRepository<DAL.Models.DbAnnotationItemAccessibleGroups> annotationItemAccessibleGroups,
                IVocabularyService vocabularyService,
                ISecurityService securityService,
                IAnnotationItemIntegrationService AIintegtationService,
                ISecurityService userFilesService,
                Core.Configuration.IConfigurationProvider configurationProvider,
                IRepository<DAL.Models.DbUser> usersRepository,
                IRepository<DAL.Models.DbRole> groupsRepository,
                IUserDataIntegrationService userDataIntegrationService,
                ISecurityServiceAsync securityServiceAsync,
                IPluginsProvider pluginsProvider,
                IRepository<DbUIElement> uiElementsRepository
            )
        {
            _annotationItemAccessibleUsers = annotationItemAccessibleUsers;
            _annotationItemAccessibleGroups = annotationItemAccessibleGroups;
            _annotationRepository = annotationRepository;
            _vocabularyService = vocabularyService;
            _securityService = securityService;
            _AIintegtationService = AIintegtationService;
            _configurationProvider = configurationProvider;
            _usersRepository = usersRepository;
            _groupsRepository = groupsRepository;
            _userDataIntegrationService = userDataIntegrationService;
            _securityServiceAsync = securityServiceAsync;
            _pluginsProvider = pluginsProvider;
            _uiElementsRepository = uiElementsRepository;
        }

        /// <summary>
        /// Adds new Annotation Item
        /// </summary>
        /// <returns>Bool</returns>
        [Authorize]
        [HttpPost, AnnotationItemValidate]
        public Models.AccessResources.AnnotationItemViewModel AddAnnotationItem([FromBody]Models.AccessResources.AnnotationItemViewModel model)
        {
            try
            {
                var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                model.AnnotationItem.Id = Guid.NewGuid();
                model.AnnotationItem.OwnerId = currentUser.Id;
                model.AnnotationItem.OwnerName = currentUser.UserName;
                model.AnnotationItem.CreationDate = DateTime.Now;
                //model.AnnotationItem.CreationDate = new DateAndTimeModel
                //{
                //    Date = DateTime.Now,
                //    Time = DateTime.Now.ToString("HH:mm")
                //};
                model.IsAccessible = true;

                var annotation = Mapper.Map(model.AnnotationItem, new DbAnnotationItem());
                var dbAnnotationItem = _annotationRepository.Insert(annotation);

                if (model.AnnotationItem.AccessMode == AccessModes.Explicit)
                {
                    foreach (var item in model.AcceptedUsersAndGroups)
                    {
                        if (item.IsGroup)
                        {
                            var dbGroup = _groupsRepository.Find(u => u.Id.ToString() == item.Id);
                            _annotationItemAccessibleGroups.Insert(new DbAnnotationItemAccessibleGroups
                            {
                                AnnotationItem = dbAnnotationItem,
                                Id = Guid.NewGuid(),
                                Group = dbGroup
                            });
                        }
                        else
                        {
                            var dbUser = _usersRepository.Find(u => u.Id.ToString() == item.Id);
                            _annotationItemAccessibleUsers.Insert(new DbAnnotationItemAccessibleUsers
                            {
                                AnnotationItem = dbAnnotationItem,
                                Id = Guid.NewGuid(),
                                User = dbUser
                            });
                        }
                    }
                }

                model.AnnotationItem = Mapper.Map(dbAnnotationItem, new Web.Models.AnnotationItemModel());

                return model;
            }
            catch (Exception exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Updates Annotation Item
        /// </summary>
        /// <returns>Bool</returns>
        [Authorize]
        [HttpPost]
        public Models.AccessResources.AnnotationItemViewModel UpdateAnnotationItem([FromBody]Models.AccessResources.AnnotationItemViewModel model)
        {
            var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            model.AnnotationItem.OwnerId = currentUser.Id;
            model.AnnotationItem.OwnerName = currentUser.UserName;

            var dbAnnotationItem = _annotationRepository.Find(f => f.Id == model.AnnotationItem.Id);
            var mappedAI = Mapper.Map(model.AnnotationItem, dbAnnotationItem);

            _annotationRepository.Update(mappedAI);

            if (model.AnnotationItem.AccessMode == AccessModes.Explicit)
            {
                Mapper.Map(model, dbAnnotationItem);
                var existingAccessibleUsers = _annotationItemAccessibleUsers.FindCollection(au => au.AnnotationItem.Id == dbAnnotationItem.Id).ToList();
                foreach (var user in existingAccessibleUsers)
                    _annotationItemAccessibleUsers.Delete(user);

                var existingAccessibleGroups = _annotationItemAccessibleGroups.FindCollection(au => au.AnnotationItem.Id == dbAnnotationItem.Id).ToList();
                foreach (var group in existingAccessibleGroups)
                    _annotationItemAccessibleGroups.Delete(group);

                foreach (var item in model.AcceptedUsersAndGroups)
                {
                    if (item.IsGroup)
                    {
                        var dbGroup = _groupsRepository.Find(u => u.Id.ToString() == item.Id);
                        _annotationItemAccessibleGroups.Insert(new DbAnnotationItemAccessibleGroups
                        {
                            AnnotationItem = dbAnnotationItem,
                            Id = Guid.NewGuid(),
                            Group = dbGroup
                        });
                    }
                    else
                    {
                        var dbUser = _usersRepository.Find(u => u.Id.ToString() == item.Id);
                        _annotationItemAccessibleUsers.Insert(new DbAnnotationItemAccessibleUsers
                        {
                            AnnotationItem = dbAnnotationItem,
                            Id = Guid.NewGuid(),
                            User = dbUser
                        });
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Import AI from file
        /// </summary>
        /// <returns>Bool</returns>
        [Authorize]
        [HttpPost]
        public bool Import([FromBody]Models.AnnotationItemImportModel model)
        {
            try
            {
                var bytesArray = new byte[] { };
                byte[] transformationBytesArray = new byte[] { };
                var importFileData = model.ExtendableData;
                var transformationFileData = model.TransformationData;

                var importFileType = model.DataType;
                var transformationFileType = model.TransformationDataType;

                if (importFileType == AIImportTypes.FileFromURL)
                    bytesArray = new System.Net.WebClient().DownloadData(importFileData);
                else if(importFileType == AIImportTypes.FromUserFiles)
                {
                    var userFilePath = GetUserFilePath(importFileData);
                    if (null == userFilePath)
                        throw new Exception("File not found");

                    bytesArray = new System.Net.WebClient().DownloadData(userFilePath);
                }
                else if (importFileType == AIImportTypes.UploadedFile)
                {
                    var fileInfo = new Regex(@"^data:(.*);base64,").Match(importFileData);
                    bytesArray = Convert.FromBase64String(importFileData.Replace(fileInfo.Groups[0].Value, String.Empty));
                }

                if (model.UseTransormation)
                {
                    if (transformationFileType == AIImportTypes.FileFromURL)
                        transformationBytesArray = new System.Net.WebClient().DownloadData(transformationFileData);
                    else if (transformationFileType == AIImportTypes.UploadedFile)
                    {
                        var fileInfo = new Regex(@"^data:(.*);base64,").Match(transformationFileData);
                        transformationBytesArray = Convert.FromBase64String(transformationFileData.Replace(fileInfo.Groups[0].Value, String.Empty));
                    }
                }

                var extendableFile = System.Text.Encoding.Default.GetString(bytesArray);
                var transformationFile = System.Text.Encoding.Default.GetString(transformationBytesArray);

                if (model.SaveFileAfterUpload)
                {
                    var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
                    var basePath = appConfig.ApploadsRoot;
                    var userFilesPath = Path.Combine(basePath, "UserFiles");
                    var userId = User.Identity.GetUserId();

                    var extendableFileId = Guid.NewGuid();
                    var oldExtension = Path.GetExtension(model.ExtendableDataName);
                    var newFileName = string.Concat(extendableFileId, oldExtension);
                    var fileVirtualPath = userFilesPath + "/" + newFileName;
                    File.WriteAllBytes(HttpContext.Current.Server.MapPath(fileVirtualPath), bytesArray);
                    var filesToAdd = new List<UserFile> { };

                    var extendableFileModel = new UserFile
                    {
                        Id = extendableFileId,
                        Name = model.ExtendableDataName,
                        Owner = new User { Id = userId },
                        OwnerId = userId,
                        CreationDate = DateTime.Now,
                        Type = UserFile.FileType.Other
                    };
                    filesToAdd.Add(extendableFileModel);

                    if (model.UseTransormation)
                    {
                        var transformationFileId = Guid.NewGuid();
                        oldExtension = Path.GetExtension(model.TransformatioDataName);
                        newFileName = string.Concat(transformationFileId, oldExtension);
                        fileVirtualPath = userFilesPath + "/" + newFileName;
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fileVirtualPath), transformationBytesArray);
                        var transformationFileModel = new UserFile
                        {
                            Id = transformationFileId,
                            Name = model.TransformatioDataName,
                            Owner = new User { Id = userId },
                            OwnerId = userId,
                            CreationDate = DateTime.Now,
                            Type = UserFile.FileType.Other
                        };
                        filesToAdd.Add(transformationFileModel);
                    }

                    _securityService.AddUserFiles(filesToAdd);
                }
                
                var annotationItem = _AIintegtationService.ImportWithTransform(extendableFile, !model.UseTransormation ? transformationFile : null);
                var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var dbUser = _usersRepository.Find(u => u.Id.ToString() == currentUser.Id);

                var context = new ValidationContext(annotationItem, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(annotationItem, context, validationResults, true);

                if (!isValid)
                    throw new Exception("Invalid model.");

                if(model.BehaviorType.HasValue)
                {
                    switch(model.BehaviorType)
                    {
                        case AIImportBehavior.CreateClean:
                            var dbAnnotationItem = Mapper.Map(annotationItem, new DbAnnotationItem());
                            dbAnnotationItem.Id = Guid.NewGuid();
                            dbAnnotationItem.OwnerId = currentUser.Id;
                            dbAnnotationItem.OwnerName = currentUser.UserName;

                            dbAnnotationItem.CreationDate = DateTime.Now;

                            _annotationRepository.Insert(dbAnnotationItem);

                            break;
                        case AIImportBehavior.CopyFromExisting:
                            var existingAnnotation = _annotationRepository.Find(f => f.Id == model.CopyFromId);
                            var mappedExistingAnnotation = Mapper.Map(annotationItem, existingAnnotation);
                            var mappedAnnotationItem = Mapper.Map(existingAnnotation, new AnnotationItem());
                            var dbCopiedAnnotationItem = Mapper.Map(mappedAnnotationItem, new DbAnnotationItem());
                            dbCopiedAnnotationItem.Id = Guid.NewGuid();
                            dbCopiedAnnotationItem.OwnerId = currentUser.Id;
                            dbCopiedAnnotationItem.OwnerName = currentUser.UserName;

                            dbCopiedAnnotationItem.CreationDate = DateTime.Now;

                            _annotationRepository.Insert(dbCopiedAnnotationItem);
                            break;
                        case AIImportBehavior.UpdateExisting:
                            var existingAnnotation1 = _annotationRepository.Find(f => f.Id == model.CopyFromId);
                            var mappedExistingAnnotation1 = Mapper.Map(annotationItem, existingAnnotation1);
                            _annotationRepository.Update(mappedExistingAnnotation1);
                            break;
                    }
                }
                else
                {
                    var dbAnnotationItem = Mapper.Map(annotationItem, new DbAnnotationItem());
                    dbAnnotationItem.Id = Guid.NewGuid();
                    dbAnnotationItem.OwnerId = currentUser.Id;
                    dbAnnotationItem.OwnerName = currentUser.UserName;

                    dbAnnotationItem.CreationDate = DateTime.Now;

                    _annotationRepository.Insert(dbAnnotationItem);
                }

            }
            catch(DeserializeSchemaException ex)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "These file cannot be imported because its schema is not conform with an actual EASE schema. Please check Your data or use transformation");
                throw new HttpResponseException(message);
            }
            catch (Exception ex)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while importing Your data. If You are sure, Your data is conform with an actual EASE schema, please contact Your administrator and provide following details for an error: " + ex.Message);
                throw new HttpResponseException(message);
            }
            return true;
        }


        //CreateFromTemplate(Guid[] filesId, Guid? cloningId)

        /// <summary>
        /// Create new AI from uploads user's files
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<string> AddFromNewFiles()
        {
            var guids = await UploadFilesAsync();
            var uri = Url.Link("Default", new { controller = "AnnotationItem", area = "", action = "CreateFromTemplate", filesIds = string.Join(",", guids) });

            return uri;
        }

        /// <summary>
        /// Create new AI from existing user's files
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<string> AddFromExistingFiles(WizardDataModel model)
        {
            var guids = model.FilesIds;
            await Task.Delay(0);
            var uri = Url.Link("Default", new { controller = "AnnotationItem", area = "", action = "CreateFromTemplate", filesIds = string.Join(",", guids) });

            return uri;
        }

        /// <summary>
        /// Create new AI from uploads user's files and existing AI data
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<string> AddFromNewFilesAndExistingAnnotaions(string id)
        {
            var guids = await UploadFilesAsync();
            var cloneId = id;
            var uri = Url.Link("Default", new { controller = "AnnotationItem", area = "", action = "CreateFromTemplate", filesIds = string.Join(",", guids), cloningId = cloneId });

            return uri;
        }

        /// <summary>
        /// Create new AI from existing user's files and existing AI data
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<string> AddFromExistingFilesAndExistingAnnotaions(WizardDataModel model)
        {
            var guids = model.FilesIds;
            var cloneId = model.AnnotationItemId;
            var uri = Url.Link("Default", new { controller = "AnnotationItem", area = "", action = "CreateFromTemplate", filesIds = string.Join(",", guids), cloningId = cloneId });

            await Task.Delay(0);
            return uri;
        }

        /// <summary>
        /// Update existing AI from uploads user's files
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task UpdateFromNewFiles(string id)
        {
            var guids = await UploadFilesAsync();
            await UpdateIAWithFiles(Guid.Parse(id), guids);
        }

        /// <summary>
        /// Update existing AI from existing uses's files
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task UpdateFromExistingFiles(WizardDataModel model)
        {
            var guids = model.FilesIds;
            await UpdateIAWithFiles(Guid.Parse(model.AnnotationItemId), guids.Select(s=> int.Parse(s)));
        }


        private async Task UpdateIAWithFiles(Guid id, IEnumerable<int> filesIds)
        {
            var existingAi = _annotationRepository.Find(f => f.Id == id);
            if (existingAi != null)
            {
                DbResource resource;
                if (existingAi.Object.Resources.Any())
                {
                    resource = existingAi.Object.Resources.First();
                }
                else
                {
                    resource = new DbResource();
                    existingAi.Object.Resources.Add(resource);
                }
                if (resource.OfflineResources == null)
                {
                    resource.OfflineResources = new List<DbOfflineResource>();
                }
                foreach (var guid in filesIds)
                {

                    var file = await _securityServiceAsync.GetUserFileByIdAsync(guid.ToString());
                    if (file != null)
                    {
                        resource.OfflineResources.Add(new DbOfflineResource()
                        {
                            FileID = guid,
                            FileName = file.Name,
                            FilePath = GetUserFilePath(guid.ToString()),
                            MimeType = file.Type.ToString(),
                            DataFormat = new DataFormat()
                            {
                                Uri = "",
                                Value = MimeMapping.GetMimeMapping(GetUserFilePath(guid.ToString()))
                            }
                        });
                    }
                }


                _annotationRepository.Update(existingAi);
            }
            await Task.Delay(0);
        }

        /// <summary>
        /// Updates Annotation Item
        /// </summary>
        /// <returns>Bool</returns>
        [Authorize]
        [HttpPost]
        public bool SetAnnotationItemsAccessMode([FromBody]AIAccessModeChangeModel model)
        {
            var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            foreach (var id in model.Ids)
            {
                var dbAnnotationItem = _annotationRepository.Find(f => f.Id.ToString() == id);
                dbAnnotationItem.AccessMode = model.AccessMode;
                _annotationRepository.Update(dbAnnotationItem);

                var existingAccessibleUsers = _annotationItemAccessibleUsers.FindCollection(au => au.AnnotationItem.Id == dbAnnotationItem.Id).ToList();
                foreach (var user in existingAccessibleUsers)
                    _annotationItemAccessibleUsers.Delete(user);

                var existingAccessibleGroups = _annotationItemAccessibleGroups.FindCollection(au => au.AnnotationItem.Id == dbAnnotationItem.Id).ToList();
                foreach (var group in existingAccessibleGroups)
                    _annotationItemAccessibleGroups.Delete(group);

                try
                {
                    foreach (var item in model.UsersAndGroups)
                    {
                        if (item.IsGroup)
                        {
                            var dbGroup = _groupsRepository.Find(u => u.Id.ToString() == item.Id);
                            _annotationItemAccessibleGroups.Insert(new DbAnnotationItemAccessibleGroups
                            {
                                AnnotationItem = dbAnnotationItem,
                                Id = Guid.NewGuid(),
                                Group = dbGroup
                            });
                        }
                        else
                        {
                            var dbUser = _usersRepository.Find(u => u.Id.ToString() == item.Id);
                            _annotationItemAccessibleUsers.Insert(new DbAnnotationItemAccessibleUsers
                            {
                                AnnotationItem = dbAnnotationItem,
                                Id = Guid.NewGuid(),
                                User = dbUser
                            });
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return true;
        }

        /// <summary>
        /// Deletes Annotation Item
        /// </summary>
        /// <returns>Bool</returns>
        [Authorize]
        [HttpPost]
        public bool AnnotationItemsDelete([FromBody]List<ShortAnnotationItemModel> models)
        {
            foreach (var annotationItem in models)
            {
                var dbAnnotationItem = _annotationRepository.Find(f => f.Id == annotationItem.Id);

                var accessibleUsers =
                    _annotationItemAccessibleUsers.Select()
                        .Where(s => s.AnnotationItem.Id == annotationItem.Id)
                        .ToList();

                var accessibleGroups =
                    _annotationItemAccessibleGroups.Select()
                        .Where(s => s.AnnotationItem.Id == annotationItem.Id)
                        .ToList();

                foreach (var dbAnnotationItemAccessibleUserse in accessibleUsers)
                {
                    _annotationItemAccessibleUsers.Delete(dbAnnotationItemAccessibleUserse);
                }

                foreach (var dbAnnotationItemAccessibleGroupse in accessibleGroups)
                {
                    _annotationItemAccessibleGroups.Delete(dbAnnotationItemAccessibleGroupse);
                }
                _annotationRepository.Delete(dbAnnotationItem);
            }

            return true;
        }

        /// <summary>
        /// Export Annotation Item
        /// </summary>
        /// <returns>Bool</returns>
        [Authorize]
        [HttpPost]
        public string ExportAnnotationItems([FromBody]List<ShortAnnotationItemModel> models)
        {
            var host = this.RequestContext.Url.Request.RequestUri.Scheme + "://" + this.RequestContext.Url.Request.RequestUri.Authority;

            var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
            var basePath = appConfig.ApploadsRoot;
            var userFilesPath = Path.Combine(Environment.CurrentDirectory, basePath, "UserFiles");
            var rootPath = HttpContext.Current.Server.MapPath(userFilesPath);
            string serverPath;
            try
            {
                serverPath = _userDataIntegrationService.ExportAnnotationItems(currentUser, rootPath, models.Select(s => s.Id.ToString()), host);
            }
            catch (Exception ex)
            {
                throw;
            }
            var fileName = Path.GetFileName(serverPath);
            return userFilesPath + "/" + fileName;
        }

        /// <summary>
        /// Returns Annotation Item by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Models.AccessResources.AnnotationItemViewModel GetAnnotationItemById([FromUri]Guid id)
        {
            var dbAccessibleUsers = _annotationItemAccessibleUsers.FindCollection(a => a.AnnotationItem.Id == id).Select(a => a.User).ToList();
            var dbAccessibleGroups = _annotationItemAccessibleGroups.FindCollection(a => a.AnnotationItem.Id == id).Select(a => a.Group).ToList();

            var acceptedUsers = Mapper.Map<List<DbUser>, List<User>>(dbAccessibleUsers);
            var acceptedGroups = Mapper.Map<List<DbRole>, List<Core.Security.Group>>(dbAccessibleGroups);

            var dbAnnotationItem = _annotationRepository.Find(f => f.Id == id);
            var AIDBmodel = Mapper.Map<DbAnnotationItem, AnnotationItemModel>(dbAnnotationItem);

            var usersAndGroups = new List<UsersAndGroupsSearchResultsModel>();
            var isAccessible = false;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _usersRepository.Find(f => f.Id.ToString() == userId);

                if (dbAnnotationItem.OwnerId != null && dbAnnotationItem.OwnerId == user.Id.ToString())
                {
                    isAccessible = true;
                }
                //else if(dbAnnotationItem.AccessMode == AccessModes.Internal)
                //{
                //    isAccessible = true;

                //}

                //if (dbAccessibleUsers.Any(a => a.Id == user.Id) ||
                //dbAccessibleGroups.Any(a => a.Users.Any(a1 => a1.Id == user.Id)))
                //    isAccessible = true;

            }

            usersAndGroups.AddRange(acceptedUsers.Select(u => new UsersAndGroupsSearchResultsModel
            {
                Id = u.Id,
                IsGroup = false,
                Name = u.Surname + " " + u.Name
            }));

            usersAndGroups.AddRange(acceptedGroups.Select(u => new UsersAndGroupsSearchResultsModel
            {
                Id = u.Id,
                IsGroup = true,
                Name = u.Name
            }));

            var AIViewModel = new Models.AccessResources.AnnotationItemViewModel
            {
                AnnotationItem = AIDBmodel,
                AcceptedUsersAndGroups = usersAndGroups,
                IsAccessible = isAccessible,
                FilesNames = new Dictionary<string, string>()
            };

            //var uris = new List<UriModel>();

            //if(AIViewModel.AnnotationItem.Object.Resources.Count > 0)
            //    uris = AIViewModel.AnnotationItem.Object.Resources[0]?.OnlineResources[0]?.Uris;

            //if(uris.Count > 0)
            //{
            //    foreach(var uri in uris)
            //    {
            //        string fileId = Path.GetFileNameWithoutExtension(uri.DownloadUrl);
            //        var file = _userFilesService.GetUserFileById(fileId);
            //        if(null != file)
            //            AIViewModel.FilesNames.Add(fileId, file.Name);

            //        uri.DownloadUrl = "Api/UserFiles/GetUserFile/" + fileId;
            //    }
            //}

            return AIViewModel;
        }


        /// <summary>
        /// Returns a new Annotation Item cloned by existing AI
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Models.AccessResources.AnnotationItemViewModel CloneAnnotationItemById([FromUri]Guid id)
        {
            try
            {
                var dbAnnotationItem = _annotationRepository.Find(f => f.Id == id);

                //map to Core model for reset id's
                var cleanAi = Mapper.Map<AnnotationItem>(dbAnnotationItem);
                //and map reverse to prepare for DB ready model
                var cleanDbModel = Mapper.Map<DbAnnotationItem>(cleanAi);
                //and last step, convert clean DB model to UI model
                var AIUImodel = Mapper.Map<AnnotationItemModel>(cleanDbModel);

                var AIViewModel = new Models.AccessResources.AnnotationItemViewModel
                {
                    AnnotationItem = AIUImodel,
                    AcceptedUsersAndGroups = new List<UsersAndGroupsSearchResultsModel>(),
                    IsAccessible = true,
                    FilesNames = new Dictionary<string, string>()
                };

                return AIViewModel;
            }
            catch(Exception ex) { throw; }
        }


        /// <summary>
        /// Returns all Annotation Items
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<ShortAnnotationItemModel> GetAnnotationItems()
        {
            var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            IEnumerable<ShortAnnotationItemModel> dbAnnotationItems = new List<ShortAnnotationItemModel>();

            try
            {
                dbAnnotationItems =
                    _annotationRepository.Select()
                        .Where(f => f.OwnerId == currentUser.Id).ToList().
                        Select(s => new ShortAnnotationItemModel()
                        {
                            AccessMode = s.AccessMode.ToString(),
                            CreationDate = s.CreationDate,
                            Id = s.Id,
                            OwnerId = s.OwnerId,
                            OwnerName = s.OwnerName,
                            Name = s.Object.References.Descriptions.Count > 0 ? s.Object.References.Descriptions[0].Title: null,
                            Description = s.Object.References.Descriptions.Count > 0 ? s.Object.References.Descriptions[0].Abstract : null
                        });
            }
            catch (Exception exception)
            {
                throw;
            }
            //var models = Mapper.Map<IEnumerable<DbAnnotationItem>, List<ShortAnnotationItemModel>>(dbAnnotationItems);

            return dbAnnotationItems;
            //return models;
        }

        /// <summary>
        /// Returns all Annotation Items
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<ShortAnnotationItemModel> GetAllAnnotationItems()
        {
            var dbAnnotationItems =
                _annotationRepository.Select().ToList()
                    .Select(s => new ShortAnnotationItemModel()
                    {
                        AccessMode = s.AccessMode.ToString(),
                        CreationDate = s.CreationDate.Date,
                        Id = s.Id,
                        OwnerId = s.OwnerId,
                        OwnerName = s.OwnerName,
                        Name = s.Object.References.Descriptions[0].Title,
                        Description = s.Object.References.Descriptions[0].Abstract
                    });

            //var models = Mapper.Map<IEnumerable<DbAnnotationItem>, List<ShortAnnotationItemModel>>(dbAnnotationItems);

            return dbAnnotationItems;
            //return models;
        }

        /// <summary>
        /// Returns all Annotation Items
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<ShortAnnotationItemModel> GetUserAnnotationItems([System.Web.Http.FromUri]string userId)
        {
            var currentUser = _securityService.GetUserById(userId);
            var dbAnnotationItems =
                _annotationRepository.Select()
                    .Where(f => f.OwnerId == currentUser.Id).ToList()
                    .Select(s => new ShortAnnotationItemModel()
                    {
                        AccessMode = s.AccessMode.ToString(),
                        CreationDate = s.CreationDate.Date,
                        Id = s.Id,
                        OwnerId = s.OwnerId,
                        OwnerName = s.OwnerName,
                        Name = s.Object.References.Descriptions[0].Title,
                        Description = s.Object.References.Descriptions[0].Abstract
                    });

            //var models = Mapper.Map<IEnumerable<DbAnnotationItem>, List<ShortAnnotationItemModel>>(dbAnnotationItems);
            //return models;
            return dbAnnotationItems;
        }

        /// <summary>
        /// Copies AI
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string CopyAIToMyData([System.Web.Http.FromUri]string id)
        {
            var userid = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = _securityService.GetUserById(userid);
            var dbAnnotationItem = _annotationRepository.Find(ai => ai.Id.ToString() == id);

            var copiedAI = new DbAnnotationItem();
            Mapper.Map(dbAnnotationItem, copiedAI);

            copiedAI.Id = Guid.NewGuid();
            copiedAI.AccessMode = AccessModes.Private;
            copiedAI.OwnerId = userid;
            copiedAI.OwnerName = currentUser.UserName;
            _annotationRepository.Insert(copiedAI);

            return copiedAI.Id.ToString();
        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage ChangeAnnotationItemOwner([FromUri] string ownerId, [FromBody] string[] annotationItemIds)
        {
            var newOwnerUser = _usersRepository.Find(u => u.Id.ToString() == ownerId);
            if (newOwnerUser == null) throw new InvalidOperationException("Unknown user");

            for (int i = 0; i < annotationItemIds.Length; i++)
            {
                var annotationItemId = annotationItemIds[i];
                var ai = _annotationRepository.Find(a => a.Id.ToString() == annotationItemId);
                if (ai == null) throw new InvalidOperationException("Unknown Annotation item");


                ai.OwnerId = ownerId;
                ai.OwnerName = newOwnerUser.UserName;

                _annotationRepository.Update(ai);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpGet]//DbAnnotationObject
        public List<string> ParseClasses<T>()
        {
            var classEnumsDictionary = new Dictionary<string, object>();
            var elements = new List<string>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var propertyTypeFullName = property.PropertyType.FullName;
                // Here we call Regex.Match.
                Match match = Regex.Match(propertyTypeFullName, @"Cafe\.DAL\.Models\.(Db[A-z]+),?",
                    RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    string key = match.Groups[1].Value;
                    elements.Add(key);

                    MethodInfo method = typeof(AnnotationItemController).GetMethod("ParseClasses");
                    MethodInfo genericMethod = method.MakeGenericMethod(property.PropertyType);
                    var propertyEnumElements = (genericMethod.Invoke(this, null)) as List<string>;

                    elements.AddRange(propertyEnumElements);
                }

            }

            return elements;
        }

        [HttpGet]
        public object Test()
        {
            var result = ParseClasses<DbAnnotationObject>().Distinct();
            var res = result.Select(o => "c.CreateMap<" + o.Substring(2) + "Model, DAL.Models." + o + "> ();");

            System.IO.File.WriteAllText(@"A:\test.txt", String.Join("\n", res));
            return true;
        }


        public object GetAnnotationItemClassProperties<T>()
        {
            var classEnumsDictionary = new Dictionary<string, object>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var propertyEnumElements = new object();
                var ddd = property.PropertyType.GetProperty("Value");
                if (property.PropertyType.GetProperty("Value")?.PropertyType.Name == "String")
                {
                    var type = System.Type.GetType("CAFE.Core.Integration." + property.PropertyType.Name + "Vocabulary, CAFE.Core", false, true);
                    if (type != null)
                    {
                        try
                        {
                            propertyEnumElements = _vocabularyService.GetVocabularyValues(type, System.Web.HttpContext.Current.User.Identity.GetUserId()).ToList();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                    else
                        propertyEnumElements = String.Empty;
                }
                else if (property.PropertyType.BaseType == typeof(object) && property.PropertyType != typeof(string))
                {
                    MethodInfo method = typeof(AnnotationItemController).GetMethod("GetAnnotationItemClassProperties");
                    MethodInfo genericMethod = method.MakeGenericMethod(property.PropertyType);
                    propertyEnumElements = genericMethod.Invoke(this, null);
                }
                else
                    propertyEnumElements = String.Empty;

                if (!property.Name.Contains("Specified"))
                    classEnumsDictionary.Add(property.Name, propertyEnumElements);
            }

            return classEnumsDictionary;
        }

        private Dictionary<string, string> GetAnnotationItemEnumNames<T>()
        {
            var enumsDictionary = new Dictionary<string, string>();
            var enumType = typeof(T);
            var enemValues = Enum.GetNames(enumType).AsEnumerable();
            foreach (var enemValue in enemValues)
            {
                var findedAlias = enemValue;
                var memInfo = enumType.GetMember(enemValue);
                var attributes = memInfo[0].GetCustomAttributes(typeof(XmlEnumAttribute),
                    false);
                if (attributes.Length > 0)
                {
                    findedAlias = ((XmlEnumAttribute)attributes[0]).Name;
                }

                enumsDictionary.Add(enemValue, findedAlias);
            }

            return enumsDictionary;
        }

        private object GetVocabularies<T>()
        {
            var values = _vocabularyService.GetVocabularyValues(typeof(T), System.Web.HttpContext.Current.User.Identity.GetUserId());
            return values;
        }


        private IEnumerable<VocabularyValueModel> GetVocabularies(string vocabularyType, string searchToken)
        {
            var typeName = vocabularyType.First().ToString().ToUpper() + vocabularyType.Substring(1) + "Vocabulary";
            var annotationItemType = typeof(AnnotationItem);
            var annotationItemAsm = annotationItemType.Assembly;


            //var types = annotationItemAsm.GetTypes();
            //var vocabulatyTypes = types.Where(t => t.Name.EndsWith("Vocabulary"));
            //foreach(var voc in vocabulatyTypes)
            //{
            //    Trace.WriteLine(voc.Name);
            //}

            var type = annotationItemAsm.GetType(string.Concat(annotationItemType.Namespace, ".", typeName));
            var values = _vocabularyService.GetVocabularyValues(type, System.Web.HttpContext.Current.User.Identity.GetUserId(), searchToken);
            return Mapper.Map<IEnumerable<VocabularyValue>, IEnumerable<VocabularyValueModel>>(values);
        }

        [HttpGet]
        public Dictionary<string, string> GetSimpleTypesVocabularies()
        {
            return _vocabularyService.GetSimpleTypesDescriptions();
        }

        [HttpGet]
        public List<string> GetUserHiddenHelpers()
        {
            var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            return currentUser.HiddenHelpers.Select(h => h.Name).ToList();
        }

        [HttpPost]
        public void HideUserHelper([FromBody]string helpername)
        {
            _securityService.AddOreRemoveUserHiddenHelper(Guid.Parse(HttpContext.Current.User.Identity.GetUserId()), helpername);
        }

        private IEnumerable<VocabularyValueModel> GetVocabularies_<T>()
        {
            var values = _vocabularyService.GetVocabularyValues_(typeof(T), System.Web.HttpContext.Current.User.Identity.GetUserId());
            return Mapper.Map<IEnumerable<VocabularyValueModel>>(values);
        }

        [HttpGet]
        public object GetGeoLocations([FromUri]Models.LocationSearchModel data)
        {
            var values = _vocabularyService.GetVocabularyValuesExtended(typeof(CountryNameVocabulary), data.Type, data.Search, System.Web.HttpContext.Current.User.Identity.GetUserId()).ToArray();

            if (data.Type == "Country")
                return values.ToList();

            return values;
        }

        //[HttpPost]
        //public IEnumerable<VocabularyValue> SearchOrganismProperyValues([FromBody]Models.LocationSearchModel data)
        //{
        //    var type = System.Type.GetType("CAFE.Core.Integration." + data.Type + "Vocabulary, CAFE.Core", false, true);
        //    var values = _vocabularyService.GetVocabularyValuesExtended(type, data.Type, data.Search, System.Web.HttpContext.Current.User.Identity.GetUserId());

        //    return values;
        //}


        [HttpPost]
        public IEnumerable<SpeciesResolveValue> SearchOrganismSpecifies([FromBody]SpeciesSearchModel data)
        {
            List<SpeciesResolveValue> serviceData = new List<SpeciesResolveValue>();
            var speciesPlugin = _pluginsProvider.GetPluginsFor(typeof(SpeciesVocabulary)).FirstOrDefault();
            if (speciesPlugin != null)
            {
                var speciesTypedPlugin = speciesPlugin as IVocabularyExternalSourcePlugin<SpeciesResolveValue>;
                if(speciesTypedPlugin != null)
                {
                    serviceData = speciesTypedPlugin.GetValuesExtended(data.Search, "Species").DistinctBy(d => d.Species).ToList();
                }
            }

            if (data.Import != null)
            {
                var importedData = ImportTaxonomyClassification(data.Import);
                foreach (var importTaxonomyRowModel in importedData.Where(w => w.Species.Contains(data.Search)))
                {
                    serviceData.Add(new SpeciesResolveValue()
                    {
                        Score = importTaxonomyRowModel.Score,
                        Species = importTaxonomyRowModel.Species,
                        Names = new List<GlobalNamesRankValue>()
                        {
                            new GlobalNamesRankValue(){ Rank = "class", Value = importTaxonomyRowModel.Class},
                            new GlobalNamesRankValue(){ Rank = "division", Value = importTaxonomyRowModel.Division},
                            new GlobalNamesRankValue(){ Rank = "family", Value = importTaxonomyRowModel.Family},
                            new GlobalNamesRankValue(){ Rank = "genus", Value = importTaxonomyRowModel.Genus},
                            new GlobalNamesRankValue(){ Rank = "order", Value = importTaxonomyRowModel.Order}
                        }
                    });
                }
            }

            var scoredResources = new List<SpeciesResolveValue>();
            scoredResources.AddRange(serviceData.Where(s => s.Score >= (data.MinScore / 100))
                .OrderByDescending(o => o.Score).ThenBy(o => o.Species).Take(10));

            if (scoredResources.Count < 10)
            {
                var needCount = 10 - scoredResources.Count;
                scoredResources.AddRange(serviceData.Where(s => s.Score < (data.MinScore / 100))
                    .OrderByDescending(o => o.Score).ThenBy(o => o.Species).Take(needCount));
            }
            return scoredResources.OrderByDescending(o => o.Score).ThenBy(o => o.Species);
        }

        private IEnumerable<ImportTaxonomyRowModel> ImportTaxonomyClassification(TaxonomyImportModel model)
        {
            List<ImportTaxonomyRowModel> result = new List<ImportTaxonomyRowModel>();
            try
            {
                var bytesArray = new byte[] { };
                var importFileData = model.ExtendableData;

                var importFileType = model.DataType;

                if (importFileType == AIImportTypes.FileFromURL)
                    bytesArray = new System.Net.WebClient().DownloadData(importFileData);
                else if (importFileType == AIImportTypes.FromUserFiles)
                {
                    var userFilePath = GetUserFilePath(importFileData);
                    if (null == userFilePath)
                        throw new Exception("File not found");

                    bytesArray = new System.Net.WebClient().DownloadData(userFilePath);
                }
                else if (importFileType == AIImportTypes.UploadedFile)
                {
                    var fileInfo = new Regex(@"^data:(.*);base64,").Match(importFileData);
                    bytesArray = Convert.FromBase64String(importFileData.Replace(fileInfo.Groups[0].Value, String.Empty));
                }

                //var extendableFile = System.Text.Encoding.Default.GetString(bytesArray);

                if (model.SaveFileAfterUpload)
                {
                    var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
                    var basePath = appConfig.ApploadsRoot;
                    var userFilesPath = Path.Combine(basePath, "UserFiles");
                    var userId = User.Identity.GetUserId();

                    var extendableFileId = Guid.NewGuid();
                    var oldExtension = Path.GetExtension(model.ExtendableDataName);
                    var newFileName = string.Concat(extendableFileId, oldExtension);
                    var fileVirtualPath = userFilesPath + "/" + newFileName;
                    File.WriteAllBytes(HttpContext.Current.Server.MapPath(fileVirtualPath), bytesArray);
                    var filesToAdd = new List<UserFile> { };

                    var extendableFileModel = new UserFile
                    {
                        Id = extendableFileId,
                        Name = model.ExtendableDataName,
                        Owner = new User { Id = userId },
                        OwnerId = userId,
                        CreationDate = DateTime.Now,
                        Type = UserFile.FileType.Other
                    };
                    filesToAdd.Add(extendableFileModel);

                    _securityService.AddUserFiles(filesToAdd);
                }
                using (var memoryStream = new MemoryStream(bytesArray))
                using (var textReader = new StreamReader(memoryStream, Encoding.Default))
                {
                    var csv = new CsvReader(textReader);
                    var row = 0;
                    while (csv.Read())
                    {
                        row++;

                        var taxonomy = new ImportTaxonomyRowModel();

                        taxonomy.Species = csv.GetField<string>("species");
                        taxonomy.Score = csv.GetField<double>("score");
                        taxonomy.Class = csv.GetField<string>("class");
                        taxonomy.Division = csv.GetField<string>("division");
                        taxonomy.Domain = csv.GetField<string>("domain");
                        taxonomy.Family = csv.GetField<string>("family");
                        taxonomy.Genus = csv.GetField<string>("genus");
                        taxonomy.Order = csv.GetField<string>("order");

                        result.Add(taxonomy);
                    }
                }

            }
            catch (Exception ex)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while importing csv file. Please contact Your administrator and provide following details for an error: " + ex.Message);
                throw new HttpResponseException(message);
            }
            return result;

        }


        [HttpGet]
        public IEnumerable<VocabularyValueModel> SearchVocabularies(string searchToken, string elementId)
        {
            List<VocabularyValueModel> vocabularyValues = new List<VocabularyValueModel>();

            if (!string.IsNullOrEmpty(elementId))
            {
                //elementId = elementId.First().ToString().ToUpper() + elementId.Substring(1);
                vocabularyValues.AddRange(GetVocabularies(elementId, searchToken));

                if (searchToken != null && searchToken.Length >= 3)
                {
                    var uiElement = _uiElementsRepository.Find(f => f.ElementIdOnUI.ToLower() == elementId.ToLower());
                    if (uiElement != null)
                    {
                        var gfbioClient = new GfBioServiceClient();
                        var gfbioResults = gfbioClient.Search(searchToken, uiElement.UrlForGetData);
                        vocabularyValues.AddRange(gfbioResults.Select(s =>
                            new VocabularyValueModel { Value = s.Label, Uri = s.Uri }));
                    }
                }

            }

            return vocabularyValues;
        }

        [HttpGet]
        public object GetCommonVocabularies()
        {
            return new
            {
                Hosters = GetVocabularies_<HosterNameVocabulary>(),
                Positions = GetVocabularies_<PositionVocabulary>(),
                Persons = GetAnnotationItemClassProperties<Person>()
            };
        }

        [HttpGet]
        public object GetTimeVocabularies()
        {
            return new
            {
                GeologicalTimePeriods = GetAnnotationItemClassProperties<GeologicalTimePeriod>(),
                //TimeZonesVocabulary = GetVocabularies_<TimezoneVocabulary>(),
                //TemporalExtentTypeVocabulary = GetVocabularies<TemporalExtentTypeVocabulary>(),
                //TemporalResolutionTypeVocabulary = GetVocabularies<TemporalExtentTypeVocabulary>()
            };
        }

        [HttpGet]
        public object GetBiomeVocabularies()
        {
            return new
            {
                BiomeVocabularies = new
                {
                    ZonoVocabulary = GetAnnotationItemClassProperties<ZonoBiome>(),
                    OroVocabulary = GetAnnotationItemClassProperties<OroBiome>(),
                    PedoVocabulary = GetAnnotationItemClassProperties<PedoBiome>()
                },
                PhysiognomyVocabularies = new
                {
                    TerrestrialVocabulary = GetAnnotationItemClassProperties<TerrestrialPhysiognomy>(),
                    SemiAquaticVocabulary = GetAnnotationItemClassProperties<SemiAquaticPhysiognomy>(),
                    AquaticVocabulary = GetAnnotationItemClassProperties<AquaticPhysiognomy>()
                },
                PhysiognomyVocabulary = GetAnnotationItemClassProperties<LandUse>()
            };
        }

        [HttpGet]
        public object GetOrganismVocabularies()
        {
            return new
            {
                TaxonomyDictionaries = GetAnnotationItemClassProperties<Core.Integration.Taxonomy>()
            };
        }

        [HttpGet]
        public object GetChemicalVocabularies()
        {
            var dictionaries = GetAnnotationItemClassProperties<Core.Integration.ChemicalContext>();

            return new
            {
                ElementNameVocabulary = GetVocabularies<ElementNameVocabulary>(),
                IsotopeNameVocabulary = GetVocabularies<IsotopeNameVocabulary>(),
                CompoundTypeVocabulary = GetVocabularies<CompoundTypeVocabulary>(),
                CompoundClassVocabulary = GetVocabularies<CompoundClassVocabulary>(),
                ChemicalFunctionTypeVocabulary = GetVocabularies<ChemicalFunctionTypeVocabulary>()
            };
        }


        [HttpGet]
        public object GetMethodVocabularies()
        {
            return new
            {
                Approaches = new object[] { GetAnnotationItemClassProperties<Core.Integration.Approach>() },
                Factors = new object[] { GetAnnotationItemClassProperties<Core.Integration.Factor>() },
                DataFormats = new object[] { GetAnnotationItemClassProperties<Core.Integration.DataFormat>() }
            };
        }

        [HttpGet]
        public object GetProcessVocabularies()
        {
            return new
            {
                Processes = new
                {
                    ProcessNames = GetVocabularies<ProcessNameVocabulary>(),
                    ProcessSubjects = GetVocabularies<ProcessSubjectVocabulary>()
                },
                Interactions = new
                {
                    InteractionsPartners = GetVocabularies<InteractionPartnerVocabulary>(),
                    InteractionDirections = GetVocabularies<InteractionDirectionVocabulary>(),
                    InteractionQualities = GetVocabularies<InteractionQualityVocabulary>()
                }
            };
        }

        [HttpGet]
        public object GetSpaceVocabularies()
        {
            return new
            {
                Coordinate = GetAnnotationItemClassProperties<UtmCoordinate>(),
                Location = GetAnnotationItemClassProperties<Location>(),
                SpatialResolutions = GetAnnotationItemClassProperties<SpatialResolution>(),
                Uoms = GetVocabularies<ElevationUnitVocabulary>(),
            };

        }

        [HttpPost]
        public List<ExtendDictionaryResult> ExtendDictionaries([FromBody]Models.ExtendDictionariesModel data)
        {
            Dictionary<string, string> newValuesArray;
            var type = System.Type.GetType("CAFE.Core.Integration." + char.ToUpper(data.DictionaryName[0]) + data.DictionaryName.Substring(1) + "Vocabulary, CAFE.Core", false, true);
            var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            if (data.DataType == ImportFileTypes.Excel || data.DataType == ImportFileTypes.CSV)
            {
                byte[] bytesArray;

                if (data.IsUrl)
                    bytesArray = new System.Net.WebClient().DownloadData(data.ExtendableData);
                else
                {
                    var fileInfo = new Regex(@"^data:(.*);base64,").Match(data.ExtendableData);
                    bytesArray = Convert.FromBase64String(data.ExtendableData.Replace(fileInfo.Groups[0].Value, String.Empty));
                }

                var excelVocabularySource = new Services.Resources.ExcelVocabularySource(bytesArray, data.DataType == ImportFileTypes.Excel);
                newValuesArray = _vocabularyService.Import(currentUser, type, excelVocabularySource);
            }
            else
            {
                newValuesArray = new Dictionary<string, string> { { data.ExtendableData, data.Description } };
                _vocabularyService.ManualAdd(currentUser, type, newValuesArray);
            }

            var resultList = newValuesArray.Select(r => new ExtendDictionaryResult { Value = r.Key, Description = r.Value }).ToList();

            return resultList;
        }

        [HttpPost]
        public List<Dictionary<string, object>> ImportCollection([FromBody]Models.ImportCollectionModel data)
        {
            var type = System.Type.GetType("CAFE.Core.Integration." + data.AIClassName + ", CAFE.Core");

            if (null == type)
                throw new ArgumentException("Invalid AI Class name", "data.AIClassName");

            var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var newValues = new List<Dictionary<string, object>>();

            if (data.DataType == ImportCollectionFileTypes.Excel || data.DataType == ImportCollectionFileTypes.CSV)
            {
                byte[] bytesArray;

                if (data.IsUrl)
                    bytesArray = new System.Net.WebClient().DownloadData(data.ExtendableData);
                else
                {
                    var fileInfo = new Regex(@"^data:(.*);base64,").Match(data.ExtendableData);
                    bytesArray = Convert.FromBase64String(data.ExtendableData.Replace(fileInfo.Groups[0].Value, String.Empty));
                }

                var excelVocabularySource = new Services.Resources.ExcelVocabularySource(bytesArray, data.DataType == ImportCollectionFileTypes.Excel);
                newValues = _vocabularyService.ImportCollection(type, excelVocabularySource);
            }

            return newValues;
        }

        [HttpGet]
        public HttpResponseMessage GetCSVHeadersFile([FromUri]string AIClassName, bool isExcel)
        {
            var type = System.Type.GetType("CAFE.Core.Integration." + AIClassName + ", CAFE.Core");

            if (null == type)
                throw new ArgumentException("Invalid AI Class name", "AIClassName");

            var excelVocabularySource = new Services.Resources.ExcelVocabularySource(new byte[] { }, isExcel);
            var fileInBytesArray = excelVocabularySource.CreateHeadersFile(type);

            var result = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new ByteArrayContent((byte[])fileInBytesArray)
            };

            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "headers." + (isExcel ? "xlsx" : "csv")
                };

            result.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return result;
        }


        [HttpGet]
        public IEnumerable<VocabularyUserValue> GetUsersValues()
        {
            return _vocabularyService.GetAllExtenededUsersValues();
        }

        [HttpGet]
        public IEnumerable<ManagedVocabularyValueModel> GetAllVocabularyValues()
        {
            List<ManagedVocabularyValueModel> result = new List<ManagedVocabularyValueModel>();
            try
            {
                result.AddRange(_vocabularyService.GetAllGlobalValues().Select(s => new ManagedVocabularyValueModel() { IsGlobal = true, Type = s.Type, Value = s.Value, Description = s.Description, Id = s.Id }));
                result.AddRange(_vocabularyService.GetAllExtenededUsersValues().Select(s => new ManagedVocabularyValueModel() { IsGlobal = false, Owner = s.Owner, CreationDate = s.CreationDate.ToString(), Id = s.Id, Type = s.Type, Value = s.Value, Description = s.Description }));
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        [HttpPost]
        public bool AcceptUserMetadata([FromBody]List<VocabularyUserValue> values)
        {
            var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var valuesIds = new List<long>();

            foreach (var valueGrouped in values.GroupBy(v => v.Type))
            {
                valuesIds.Clear();
                foreach (var value in valueGrouped)
                    valuesIds.Add(value.Id);

                var type = System.Type.GetType("CAFE.Core.Integration." + valueGrouped.Key + ", CAFE.Core", false, true);
                _vocabularyService.AcceptUserMetadata(type, valuesIds);
            }

            return true;
        }

        //[HttpPost]
        //public ManagedVocabularyValueModel AddGlobalVocabularyValue(ManagedVocabularyValueModel model)
        //{
        //    var value = new VocabularyValue()
        //    {
        //        Description = model.Description,
        //        Id = model.Id,
        //        Type = model.Type,
        //        Value = model.Value
        //    };

        //    var result = _vocabularyService.Save(value);

        //    return new ManagedVocabularyValueModel() { IsGlobal = true, Type = result.Type, Value = result.Value, Description = result.Description, Id = result.Id };
        //}

        //[HttpPost]
        //public ManagedVocabularyValueModel UpdateGlobalVocabularyValue(ManagedVocabularyValueModel model)
        //{
        //    var value = new VocabularyValue()
        //    {
        //        Description = model.Description,
        //        Id = model.Id,
        //        Type = model.Type,
        //        Value = model.Value
        //    };

        //    var result = _vocabularyService.Save(value);

        //    return new ManagedVocabularyValueModel() { IsGlobal = true, Type = result.Type, Value = result.Value, Description = result.Description, Id = result.Id };
        //}

        [HttpPost]
        public ManagedVocabularyValueModel SaveGlobalVocabularyValue(ManagedVocabularyValueModel model)
        {
            var value = new VocabularyValue()
            {
                Description = model.Description,
                Id = model.Id,
                Type = model.Type,
                Value = model.Value
            };

            var result = _vocabularyService.Save(value);

            return new ManagedVocabularyValueModel() { IsGlobal = true, Type = result.Type, Value = result.Value, Description = result.Description, Id = result.Id };
        }

        [HttpPost]
        public bool DeleteGlobalVocabularyValue(IEnumerable<ManagedVocabularyValueModel> model)
        {
            try
            {
                foreach (var item in model)
                    _vocabularyService.Remove(item.Id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpGet]
        public IEnumerable<string> GetVocabularyTypes()
        {
            return _vocabularyService.GetVocabularyTypes();
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
        public IEnumerable<AnnotationItemSearchViewModel> SearchAI([FromBody]string keyWord)
        {
            if (null == keyWord || keyWord.Length == 0)
                return _annotationRepository.
                    Select(ai => new AnnotationItemSearchViewModel
                    {
                        Id = ai.Id,
                        Name = ai.Object.References.Descriptions?[0]?.Title,
                        Description = ai.Object.References.Descriptions?[0]?.Abstract
                    });

            var foundAIs =
            _annotationRepository.
            FindCollection(dbAI => (null != dbAI.Object.References.Descriptions.FirstOrDefault(d => d.Title.Contains(keyWord.ToLower()) || d.Abstract.Contains(keyWord.ToLower())))).
            Select(ai => new AnnotationItemSearchViewModel
            {
                Id = ai.Id,
                Name = ai.Object.References.Descriptions?[0]?.Title,
                Description = ai.Object.References.Descriptions?[0]?.Abstract
            });
            
            return foundAIs;
        }

        [HttpGet]
        public UserViewModel GetCurrentUserInfo()
        {
            var currentUser = _securityService.GetUserById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var usersCollectionView = Mapper.Map<User, UserViewModel>(currentUser);
            return usersCollectionView;
        }
        
        private string GetUserFilePath(string id)
        {
            var currentUserId = User.Identity.GetUserId();
            var fileModel = _securityService.GetUserFileById(id);
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
                return null;

            var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
            var fileExtension = Path.GetExtension(fileModel.Name);
            var basePath = appConfig.ApploadsRoot.Replace("/", "\\") + "\\UserFiles\\";

            var filePath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + basePath + fileModel.Id + fileExtension;
            return filePath;
        }

        private async Task<IEnumerable<int>> UploadFilesAsync()
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

            await _securityServiceAsync.AddUserFilesAsync(userFilesListToAdd);
            return userFilesListToAdd.Select(s => s.FileId);

        }
        private UserFile.FileType GetFileContentType(string MIMEType)
        {
            if (MIMEType.Contains("audio/"))
                return UserFile.FileType.Audio;
            else if (MIMEType.Contains("video/"))
                return UserFile.FileType.Video;
            else if (MIMEType.Contains("application/vnd.ms-excel"))
                return UserFile.FileType.Tabular;
            else if (MIMEType.Contains("image/"))
                return UserFile.FileType.Image;
            else
                return UserFile.FileType.Other;
        }

    }
}