
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using AutoMapper;
using CAFE.Core.Integration;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.Core.Configuration;
using CAFE.Core.Resources;
using CAFE.DAL.Models.Resources;
using IConfigurationProvider = CAFE.Core.Configuration.IConfigurationProvider;

namespace CAFE.Services.Integration
{
    /// <summary>
    /// Service for Import/Export user's data (AnnotationItems, Files and Vocabulary extensions)
    /// to and from zip archive
    /// </summary>
    public class ArchiveUserDataIntegrationService : IUserDataIntegrationService
    {
        private readonly IAnnotationItemIntegrationService _annotationItemIntegrationService;
        private readonly IRepository<DbUser> _usersRepository;
        private readonly IRepository<DbAnnotationItemAccessibleUsers> _annotationUsersRepository;
        private readonly IRepository<DbAnnotationItemAccessibleGroups> _annotationGroupsRepository;
        private readonly IRepository<DbAnnotationItem> _annotationItemRepository;
        private readonly IRepository<DbVocabularyUserValue> _usersVocabularyRepository;
        private readonly IRepository<DbUserFile> _userFilesRepository;
        private readonly IVocabularyService _vocabularyService;
        private readonly ISecurityService _userFilesService;

        public ArchiveUserDataIntegrationService(IAnnotationItemIntegrationService annotationItemIntegrationService,
            IRepository<DbUser> usersRepository, IRepository<DbAnnotationItemAccessibleUsers> annotationUsersRepository,
            IRepository<DbAnnotationItemAccessibleGroups> annotationGroupsRepository, IRepository<DbAnnotationItem> annotationItemRepository,
            IRepository<DbVocabularyUserValue> usersVocabularyRepository, IRepository<DbUserFile> userFilesRepository,
            IVocabularyService vocabularyService, ISecurityService userFilesService)
        {
            _annotationItemIntegrationService = annotationItemIntegrationService;
            _usersRepository = usersRepository;
            _annotationUsersRepository = annotationUsersRepository;
            _annotationGroupsRepository = annotationGroupsRepository;
            _annotationItemRepository = annotationItemRepository;
            _usersVocabularyRepository = usersVocabularyRepository;
            _userFilesRepository = userFilesRepository;
            _vocabularyService = vocabularyService;
            _userFilesService = userFilesService;
        }


        /// <summary>
        /// Exports all user's data as pack
        /// </summary>
        /// <param name="user">User who data is exporting</param>
        /// <param name="path">Path where get files</param>
        /// <returns>Exported data (base64 string)</returns>
        public string ExportData(User user, string path, string host = "")
        {
            string zipData = null;

            var serializedAnnotationItems = GetAnnotationItems(user, null, host);//new Dictionary<string, string>();
            var files = GetFiles(user, path);
            var vocabularies = GetVocabularies(user);

            try
            {
                zipData = ZipToBase64(serializedAnnotationItems, files, vocabularies);
            }
            catch (Exception exception)
            {
                throw;
            }
            return zipData;
        }

        private string ZipToBase64(Dictionary<string, string> annotaionItems, Dictionary<string, byte[]> filesData,
            Dictionary<string, IEnumerable<string>> vocabularyData)
        {
            string result = string.Empty;

            var stream = new MemoryStream();
            //using (var stream = new MemoryStream())
            //{
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                //Puting annotation items into archive
                foreach (var annotaionItem in annotaionItems)
                {
                    var entry = archive.CreateEntry("AnnotationItems/" + annotaionItem.Key + ".xml", CompressionLevel.Optimal);
                    using (var entryStream = entry.Open())
                    using (var writer = new StreamWriter(entryStream))
                    {
                        writer.Write(annotaionItem.Value);
                    }
                }

                //Puting files into archive
                foreach (var fileData in filesData)
                {
                    var entry = archive.CreateEntry("Files/" + fileData.Key, CompressionLevel.Optimal);
                    using (var entryStream = entry.Open())
                    using (var fileToCompressStream = new MemoryStream(fileData.Value))
                    {
                        fileToCompressStream.CopyTo(entryStream);
                    }
                }

                //Puting vocabularies
                foreach (var vocabularyDataItem in vocabularyData)
                {
                    var entry = archive.CreateEntry("VocabularyExtensions/" + vocabularyDataItem.Key + ".csv", CompressionLevel.Optimal);
                    using (var entryStream = entry.Open())
                    using (var writer = new StreamWriter(entryStream))
                    {
                        foreach (var vocabularyValue in vocabularyDataItem.Value)
                        {
                            writer.WriteLine(vocabularyValue);
                        }
                        
                    }
                }
            }
            //}

            //Convert stream to string and close it
            var bytesResult = stream.ToArray();
            result = Convert.ToBase64String(bytesResult);
            stream.Close();

            return result;

        }

        private Dictionary<string, string> GetAnnotationItems(User user, IEnumerable<string> ids = null, string host = "")
        {
            var result = new Dictionary<string, string>();

            var accessibleItems = new List<DbAnnotationItem>();

            if (ids == null)
            {
                //Get annotation items accessible by user
                var userAccessibleItems = _annotationItemRepository.Select().Where(s => s.OwnerId == user.Id).ToList();

                //var userAccessibleItems =
                //    _annotationUsersRepository.Select()
                //        .Include(i => i.AnnotationItem)
                //        .Include(i => i.User)
                //        .Where(c => c.User.Id.ToString() == user.Id)
                //        .Select(s => s.AnnotationItem)
                //        .ToList();

                accessibleItems.AddRange(userAccessibleItems);

                ////Get annotation items accessible by group
                //var groupAccessibleItems =
                //    _annotationGroupsRepository.Select()
                //        .Include(i => i.AnnotationItem)
                //        .Include(i => i.Group)
                //        .Where(c => c.Group.Users.Any(a => a.Id.ToString() == user.Id))
                //        .Select(s => s.AnnotationItem)
                //        .ToList();

                //accessibleItems.AddRange(groupAccessibleItems);
            }
            else
            {
                accessibleItems.AddRange(_annotationItemRepository.FindCollection(c => ids.Contains(c.Id.ToString())).ToList());
            }

            //Enumerate each distincted annotation item and serialize it
            foreach (var annotationItem in accessibleItems.Distinct(Comparer<DbAnnotationItem>.Create((c1, c2) => (c1.Id == c2.Id) ? 0 : -1) as IEqualityComparer<DbAnnotationItem>))
            {
                var mappedAnnotationItem = Mapper.Map<AnnotationItem>(annotationItem);

                var serializedAnnotationItem = _annotationItemIntegrationService.Export(mappedAnnotationItem);

                var itemName = annotationItem.Object.References.Descriptions?[0]?.Title ?? Guid.NewGuid().ToString();
                if(!result.ContainsKey(itemName))
                    result.Add(itemName, serializedAnnotationItem);
            }

            return result;
        }

        private Dictionary<string, byte[]> GetFiles(User user, string path)
        {
            var result = new Dictionary<string, byte[]>();

            var accessibleFiles = new List<DbUserFile>();

            var dbUser =
                _usersRepository.Select()
                    .Include(i => i.AccessibleFiles)
                    .Include(i => i.Roles)
                    .Where(f => f.Id.ToString() == user.Id)
                    .FirstOrDefault();


            if(dbUser == null) throw new InvalidOperationException("User not found in DB");

            //Get files accessible by user
            //var userAccessibleFiles =
            //    dbUser.AccessibleFiles.ToList();
            var userAccessibleFiles =
                _userFilesRepository.Select().Where(w => w.Owner.Id == dbUser.Id).ToList();

            accessibleFiles.AddRange(userAccessibleFiles);

            //Get files accessible by group
            var groupAccessibleFiles =
                dbUser.Roles.SelectMany(s => s.AccessibleFiles).ToList();

            accessibleFiles.AddRange(groupAccessibleFiles);

            //Enumerate each distincted files and read it
            foreach (var file in accessibleFiles.Distinct(Comparer<DbUserFile>.Create((c1, c2) => (c1.Id == c2.Id) ? 0 : -1) as IEqualityComparer<DbUserFile>))
            {
                var filePath = Path.Combine(path, file.Id + Path.GetExtension(file.Name));

                if(File.Exists(filePath) && !result.ContainsKey(file.Name))
                    result.Add(file.Name, File.ReadAllBytes(filePath));
            }

            return result;
        }

        private Dictionary<string, IEnumerable<string>> GetVocabularies(User user)
        {
            var result = new Dictionary<string, IEnumerable<string>>();

            var vocabularies = _vocabularyService.GetAllExtenededUsersValues().Where(u => u.Owner == user.Id).ToList();

            var groupedVocabulary = vocabularies.GroupBy(g => g.Type).ToList();

            foreach (var vocabulary in groupedVocabulary)
            {
                if(!result.ContainsKey(vocabulary.Key))
                    result.Add(vocabulary.Key, vocabulary.Select(s => string.Concat(s.Id, ";",s.Value, ";", s.CreationDate)));
            }

            return result;

        }


        /// <summary>
        /// Imports all user's data as pack
        /// </summary>
        /// <param name="user">User who data is importing</param>
        /// <param name="data">Importing data (base64 string)</param>
        /// <param name="path">Path where files to download</param>
        public void ImportData(User user, string data, string path)
        {
            ArchiveContentModel unarchivedData;
            try
            {
                unarchivedData = UnzipBase64Data(user, data);
            }
            catch (Exception exception)
            {
                throw;
            }

            //try saves all data
            AddAnnotationItems(user, unarchivedData.AnnotationItems);
            AddFiles(user, unarchivedData.Files, path);
            AddVocabularies(user, unarchivedData.Vocabularies);

        }

        private class ArchiveContentModel
        {
            public Dictionary<string, string> AnnotationItems { get; set; } = new Dictionary<string, string>();
            public Dictionary<string, byte[]> Files { get; set; } = new Dictionary<string, byte[]>();
            public Dictionary<string, IEnumerable<string>> Vocabularies { get; set; } = new Dictionary<string, IEnumerable<string>>();
        }

        private ArchiveContentModel UnzipBase64Data(User user, string data)
        {
            var archiveBytes = Convert.FromBase64String(data);
            Stream stream = new MemoryStream(archiveBytes);

            var result = new ArchiveContentModel();

            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var zipArchiveEntry in archive.Entries)
                {
                    using (var reader = new StreamReader(zipArchiveEntry.Open()))
                    {
                        if (zipArchiveEntry.FullName.Contains("AnnotationItems/"))
                        {
                            //Unpack annotation item
                            result.AnnotationItems.Add(Path.GetFileNameWithoutExtension(zipArchiveEntry.Name), reader.ReadToEnd());
                        }
                        else if (zipArchiveEntry.FullName.Contains("Files/"))
                        {
                            //Unpack file
                            byte[] bytes;
                            using (var memstream = new MemoryStream())
                            {
                                reader.BaseStream.CopyTo(memstream);
                                bytes = memstream.ToArray();
                            }
                            result.Files.Add(zipArchiveEntry.Name, bytes);
                        }
                        else if (zipArchiveEntry.FullName.Contains("VocabularyExtensions/"))
                        {
                            //Unpack vocabulary values
                            var vocItems = new List<string>();
                            while (reader.Peek() >= 0)
                            {
                                vocItems.Add(reader.ReadLine());
                            }

                            result.Vocabularies.Add(Path.GetFileNameWithoutExtension(zipArchiveEntry.Name), vocItems);
                        }
                    }
                }
            }

            return result;
        }

        private void AddAnnotationItems(User user, Dictionary<string, string> annotationItems)
        {
            foreach (var annotationItem in annotationItems)
            {
                var deserialized = _annotationItemIntegrationService.ImportWithTransform(annotationItem.Value);
                var mapped = Mapper.Map<DbAnnotationItem>(deserialized);

                //Get first description as main
                var firstDescription = mapped.Object.References.Descriptions.First();
                mapped.Object.References.Descriptions.Remove(firstDescription);

                _annotationItemRepository.Insert(mapped);

                var ann = _annotationItemRepository.Find(u => u.Id == mapped.Id);
                var dbUser = _usersRepository.Find(f => f.Id.ToString() == user.Id);

                try
                {
                    _annotationUsersRepository.Insert(new DbAnnotationItemAccessibleUsers
                    {
                        AnnotationItem = ann,
                        User = dbUser
                    });
                }
                finally
                {
                    
                }
            }
        }

        private void AddFiles(User user, Dictionary<string, byte[]> files, string path)
        {

            var acceptedUsers = new List<User>();
            var acceptedGroups = new List<Core.Security.Group>();
            var userFilesListToAdd = new List<UserFile>();

            foreach (var file in files)
            {

                var fileId = Guid.NewGuid();
                var fileVirtualPath = Path.Combine(path, fileId + Path.GetExtension(file.Key));
                var userId = user.Id;
                
                File.WriteAllBytes(fileVirtualPath, file.Value);

                var fileModel = new UserFile
                {
                    Id = fileId,
                    Name = file.Key,
                    Description = "",
                    AcceptedUsers = acceptedUsers,
                    AcceptedGroups = acceptedGroups,
                    Owner = new User { Id = userId },
                    OwnerId = userId,
                    CreationDate = DateTime.Now,
                    AccessMode = AccessModes.Private,
                    Type = UserFile.FileType.Other
                };
                userFilesListToAdd.Add(fileModel);
            }

            _userFilesService.AddUserFiles(userFilesListToAdd);
        }

        private void AddVocabularies(User user, Dictionary<string, IEnumerable<string>> vocabularies)
        {
            var dbUser = _usersRepository.Find(f => f.Id.ToString() == user.Id);
            foreach (var vocabulary in vocabularies)
            {
                foreach (var value in vocabulary.Value)
                {
                    var spleatetValue = value.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                    _usersVocabularyRepository.Insert(new DbVocabularyUserValue()
                    {
                        CreationDate = DateTime.Parse(spleatetValue[2]),
                        Type = vocabulary.Key,
                        Value = spleatetValue[1],
                        User = dbUser
                    });
                }
            }
        }


        /// <summary>
        /// Exports several annotation items into archive
        /// </summary>
        /// <param name="user">User who annotation items is importing</param>
        /// <param name="path">Path where to save</param>
        /// <param name="annotationItemsIds">List of annotaion identifier</param>
        /// <returns>Archive url</returns>
        public string ExportAnnotationItems(User user, string path, IEnumerable<string> annotationItemsIds, string host = "")
        {
            string zipDataUrl = null;

            var serializedAnnotationItems = GetAnnotationItems(user, annotationItemsIds, host);//new Dictionary<string, string>();

            try
            {
                zipDataUrl = ZipAndSave(serializedAnnotationItems, path);
            }
            catch (Exception exception)
            {
                throw;
            }
            return zipDataUrl;
        }

        private string ZipAndSave(Dictionary<string, string> annotaionItems, string path)
        {


            var arhivePath = Path.Combine(path,
                "annotationitems_" + DateTime.Now.ToString("O").Replace(" ", "_").Replace(":", "").Replace("+", "") + ".zip");

            //var stream = new MemoryStream();

            using (var stream = File.OpenWrite(arhivePath))
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                    //Puting annotation items into archive
                    foreach (var annotaionItem in annotaionItems)
                    {
                        var entry = archive.CreateEntry("AnnotationItems/" + annotaionItem.Key + ".xml",
                            CompressionLevel.Optimal);
                        using (var entryStream = entry.Open())
                        using (var writer = new StreamWriter(entryStream))
                        {
                            writer.Write(annotaionItem.Value);
                        }
                    }

                }
            }

            return arhivePath;

        }

    }
}
