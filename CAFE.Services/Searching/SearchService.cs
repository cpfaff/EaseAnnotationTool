
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using CAFE.Core.Misc;
using CAFE.Core.Resources;
using CAFE.Core.Searching;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.Services.Extensions;
using IConfigurationProvider = CAFE.Core.Configuration.IConfigurationProvider;
using System.IO;
using CAFE.Core.Configuration;
using System.Data.Entity;
using System.Text;
using CAFE.Core.Integration;
using CAFE.DAL.Types;

namespace CAFE.Services.Searching
{
    public class SearchService : ISearchService
    {
        private readonly IRepository<DbUserFile> _filesRepository;
        private readonly IRepository<DbAnnotationItem> _annotationItemRepository;
        private readonly IRepository<DbSearchFilterCachedItem> _cachedSearchFilterRepository;
        private readonly IRepository<DbRemoteVersion> _versionRepository;
        private readonly IRepository<DbUser> _usersRepository;
        private readonly IVocabularyService _vocabularyService;
        private readonly IRepository<DbAnnotationItemAccessibleUsers> _accecibleUsersRepository;
        private readonly IRepository<DbAnnotationItemAccessibleGroups> _accecibleGroupsRepository;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IRepository<DbSchemaItemDescription> _schemaDescriptionsRepository;

        //private RelatedSearchFiltersConfiguration _filtersRelationConfiguration;
        private ComplexSearchFiltersConfiguration _complexfiltersConfiguration;

        public SearchService(IRepository<DbUserFile> filesRepository,
            IRepository<DbAnnotationItem> annotaionItemRepository,
            IRepository<DbSearchFilterCachedItem> cachedSearchFilterRepository,
            IRepository<DbRemoteVersion> versionRepository,
            IRepository<DbUser> usersRepository,
            IVocabularyService vocabularyService,
            IRepository<DbAnnotationItemAccessibleUsers> accecibleUsersRepository,
            IRepository<DbAnnotationItemAccessibleGroups> accecibleGroupsRepository,
            IRepository<DbSchemaItemDescription> schemaDescriptionsRepository,
            Core.Configuration.IConfigurationProvider configurationProvider)
        {
            _filesRepository = filesRepository;
            _annotationItemRepository = annotaionItemRepository;
            _cachedSearchFilterRepository = cachedSearchFilterRepository;
            _versionRepository = versionRepository;
            _usersRepository = usersRepository;
            _vocabularyService = vocabularyService;
            _accecibleUsersRepository = accecibleUsersRepository;
            _accecibleGroupsRepository = accecibleGroupsRepository;
            _configurationProvider = configurationProvider;
            _schemaDescriptionsRepository = schemaDescriptionsRepository;
        }

        private Func<DbAnnotationItem, string, bool> AISearchFunc = (dbAI, search) =>
        {
            var title = dbAI.Object.References.Descriptions?[0]?.Title?.ToLower();
            var description = dbAI.Object.References.Descriptions?[0]?.Abstract?.ToLower();
            return (title != null && title.Contains(search.ToLower()) || (description != null && description.Contains(search.ToLower())));
        };

        public async Task<IEnumerable<SearchResultItem>> SearchWithFiltersAsync(User user, SearchRequest searchRequest)
        {
            IEnumerable<SearchResultItem> result;

            if (searchRequest.ItemType == SearchResultItemType.All)
            {
                result = await SearchAllItemsWithFilterAsync(user, searchRequest.SearchText, searchRequest.Filters);
            }
            else if (searchRequest.ItemType == SearchResultItemType.AnnotationItem)
            {
                result = await SearchAnnotationsOnlyWithFilterAsync(user, searchRequest.SearchText, searchRequest.Filters);
            }
            else
            {
                result = await SearchFilesOnlyWithFilterAsync(user, searchRequest.SearchText, searchRequest.Filters);
            }

            return OrderSearchResultItems(result, searchRequest.OrderBy);
        }

        public async Task<IEnumerable<SearchRequestComplexFilter>> GetFilterParametersAsync(SearchResultItemType itemType, bool needToClear = true)
        {
            IEnumerable<SearchRequestComplexFilter> result = new List<SearchRequestComplexFilter>();

            if (itemType == SearchResultItemType.All)
            {
                result = await GetFilterParametersForAllAsync(needToClear);
            }
            else if (itemType == SearchResultItemType.AnnotationItem)
            {
                result = await GetFilterParametersForAnnotationItemsAsync(needToClear);
            }
            else
            {
                result = await GetFilterParametersForFilesAsync();
            }


            //Fill related filter items
            foreach (var resultItem in result)
            {
                //resultItem.RelatedFilters = new List<SearchRequestFilterItem>();

                try
                {
                    //var foundFilterScope = FiltersRelationConfiguration.RelatedFiltersScopes.Where(w => {

                    //    foreach (var key in w.Key.Split('_'))
                    //        if (resultItem.Name.Contains(key))
                    //            return true;

                    //    return false;
                    //}).FirstOrDefault();



                    /*
                    if(resultItem.Name.ToLower().Contains("geological"))
                    {
                        var i2 = 0;
                    }
                    */


                    //if (foundFilterScope != null)
                    //{
                    //    var sections = resultItem.Name.Split('.');
                    //    if (sections.Length > 1)
                    //    {
                    //        int foundLevel = sections.Length - 1;
                    //        //find need level..
                    //        for (var j = sections.Length - 1; j >= 0; j--)
                    //        {

                    //            var level = sections[j].Replace("[]", "").ToLower();
                    //            if (foundFilterScope.Key.ToLower() == level)
                    //            {
                    //                foundLevel = j;
                    //                break;
                    //            }
                    //        }

                    //        string[] newSectionLevels = new string[foundLevel + 1];
                    //        for (var i = 0; i <= foundLevel; i++)
                    //        {
                    //            newSectionLevels[i] = sections[i];
                    //        }

                    //        var keyPattern = string.Join(".", newSectionLevels);

                    //        //find related filters
                           
                    //        var fndItems = result.Where(w =>
                    //            w.Name.Contains(keyPattern) && w.Name != resultItem.Name).ToList();

                    //        Debug.WriteLine(string.Join(",", fndItems.Select(fi => fi.Name)));

                    //        foreach (var fndItem in fndItems)
                    //        {
                    //            var parts = fndItem.Name.Split('.');
                    //            Debug.WriteLine(parts.Length);
                    //            Debug.WriteLine(fndItem.Name);
                    //            string[] newParts = new string[sections.Length - foundLevel];
                    //            Debug.WriteLine(newParts.Length);

                    //            Array.Copy(parts, foundLevel, newParts, 0, sections.Length - foundLevel);

                    //            var newName = string.Join(".", newParts);

                    //            //bool foundMatch = false;
                    //            //foreach (var filterScopeItem in foundFilterScope.RelatedFiltersCollection)
                    //            //{
                    //            //    if (newName.ToLower().Contains(filterScopeItem.Key.ToLower()))
                    //            //    {
                    //            //        foundMatch = true;
                    //            //        break;
                    //            //    }
                    //            //}

                    //            //if (foundMatch)
                    //            //    resultItem.RelatedFilters.Add(fndItem);
                    //        }
                    //    }
                    //}

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return result;
        }

        public async Task<IEnumerable<VocabularyValue>> GetSelectValuesAsync(SearchRequestFilterItem filterItem, string userId)
        {
            if (filterItem.FilterType != FilterType.Select && filterItem.FilterType != FilterType.ReferenceValue)
                throw new InvalidOperationException("Filter type must be 'Select'");

            var result = new List<VocabularyValue>();

            var dbEntities = new Dictionary<string, List<string>> {
                { "DbLocation", new List<string>{ "LocationName" } },
                { "DbInteraction", new List<string>{ "InteractionName" } },
                { "DbCompound", new List<string>{ "CompoundName" } },
                { "DbPosition", new List<string>{ "Position[1]" } },
                { "DbPerson", new List<string>{ "PhoneNumber[1]", "EmailAddress[1]", "SurName[1]", "GivenName[1]", "Salutation[1]" } },
                { "DbUtmCoordinate", new List<string>{"UtmCoordinateZone", "UtmCoordinateSubZone" } }
            };

            var searchInDbEntity = dbEntities.SingleOrDefault(i => i.Value.Any(v => filterItem.Name.Contains(v)));
            if (null != searchInDbEntity.Key)
            {
                var repository = new DAL.DbContexts.ApplicationDbContext();
                var fieldName = filterItem.Name.Split('.').Last();

                if ("DbPerson" == searchInDbEntity.Key)
                    fieldName = fieldName.Replace("[1]", string.Empty) + "Values";
                else if ("DbPosition" == searchInDbEntity.Key)
                    fieldName = "Value";

                List<string> results = new List<string>();
                    results = await repository.Database.SqlQuery<string>($"SELECT DISTINCT {fieldName} FROM {searchInDbEntity.Key} WHERE DATALENGTH({fieldName}) > 0 ").ToListAsync();

                return results.Select(v => new VocabularyValue { Value = v });
            }

            if (filterItem.Name.Contains("LocationName"))
            {
                var annItms = _annotationItemRepository.Select().ToList();

                foreach(var ai in annItms)
                    if(ai.Object.Contexts?[0].SpaceContext?.Locations?.Count > 0)
                        result.AddRange(ai.Object.Contexts[0].SpaceContext.Locations.Select(l => new VocabularyValue{ Value = l.LocationName}));

                return result.Distinct();
            }

            
            //result.AddRange(await GetSelectValuesFromAnnotationItemsAsync(filterItem.Description.Split('.').Last(), userId));
            result.AddRange(await GetSelectValuesFromAnnotationItemsAsync(filterItem.ValueType, userId));
            result.AddRange(await GetSelectValuesFromFilesAsync(filterItem.Name));

            return result;
        }

        public async Task<IEnumerable<SearchResultItem>> SearchForAutocompleteAsync(string query, string userId)
        {
            await Task.Delay(0);
            var result = new List<SearchResultItem>();
            var tempResult =
                _filesRepository.FindCollection(f => (f.Name != null && f.Name.ToLower().Contains(query.ToLower())) ||
                                                     (f.Description != null && f.Description.ToLower()
                                                          .Contains(query.ToLower())));
            result.AddRange(tempResult
                .Select(s => new SearchResultItem() { ItemId = s.Id, Name = s.Name }));



            var annItms = _annotationItemRepository.FindCollection(x => AISearchFunc(x, query));

            result.AddRange(annItms
                .Select(s => new SearchResultItem() { ItemId = s.Id, Name = s.Object.References.Descriptions?[0]?.Title }));

            return result;
        }

        //internal RelatedSearchFiltersConfiguration FiltersRelationConfiguration
        //{
        //    get
        //    {
        //        if (_filtersRelationConfiguration == null)
        //        {
        //            _filtersRelationConfiguration = _configurationProvider.Get<RelatedSearchFiltersConfiguration>();
        //        }

        //        return _filtersRelationConfiguration;
        //    }
        //}

        internal ComplexSearchFiltersConfiguration ComplexFiltersConfiguration
        {
            get
            {
                if (_complexfiltersConfiguration == null)
                {
                    _complexfiltersConfiguration = _configurationProvider.Get<ComplexSearchFiltersConfiguration>();
                }

                return _complexfiltersConfiguration;
            }
        }
        #region internal logic

        private async Task<IEnumerable<SearchResultItem>> SearchAllItemsWithFilterAsync(User user, string search,
            IEnumerable<SearchRequestComplexFilter> filters)
        {
            await Task.Delay(0);
            var resut = new List<SearchResultItem>();

            var searchFilters = filters.ToList();
            resut.AddRange(await SearchAnnotationsOnlyWithFilterAsync(user, search, searchFilters));
            resut.AddRange(await SearchFilesOnlyWithFilterAsync(user, search, searchFilters));

            return resut;
        }

        private async Task<IEnumerable<SearchResultItem>> SearchAnnotationsOnlyWithFilterAsync(User user, string search,
            IEnumerable<SearchRequestComplexFilter> filters)
        {
            await Task.Delay(0);
            var resut = new List<SearchResultItem>();

            //_annotationItemRepository.TurnOffProxy();

            var searchQuery = BuildSqlQueryFor(filters, search);
            try
            {
                var foundKeys = _annotationItemRepository.SqlQuery(searchQuery);
                var ids = foundKeys.Select(s => s.Id).Distinct();
                var foundAnnotationItems = _annotationItemRepository.Select().Where(a => ids.Contains(a.Id)).ToList();
                foreach (var item in foundAnnotationItems)
                {
                    resut.Add(new SearchResultItem()
                    {
                        IsAccessible = CheckThatAnnotationItemsIsAccessibleFor(item, user == null ? null : Mapper.Map(user, new DbUser())),
                        ItemType = SearchResultItemType.AnnotationItem,
                        //Link = "/AnnotationItem?id=" + item.Id,//TODO: url here
                        Link = "/Search/ResourceInfo?id=" + item.Id,
                        Name = item.Object.References.Descriptions?[0]?.Title,
                        Owner = string.IsNullOrEmpty(item.OwnerId) ? null : new User() { Id = item.OwnerId, Name = item.OwnerName },
                        ItemId = item.Id,
                        AccessMode = item.AccessMode,
                        //CreationDate = annotationItem.CreationDate?.Date
                        CreationDate = item.CreationDate
                    });
                }
            }
            catch (Exception ex)
            {

            }


            return resut;
        }

        private async Task<IEnumerable<SearchResultItem>> SearchFilesOnlyWithFilterAsync(User user, string search,
            IEnumerable<SearchRequestComplexFilter> filters)
        {
            await Task.Delay(0);

            var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
            var basePath = appConfig.ApploadsRoot;
            var userFilesPath = Path.Combine(basePath, "UserFiles");

            var resut = new List<SearchResultItem>();

            var filesQuerable = _filesRepository.FindCollection(f => true);

            //foreach (var filter in filters)
            //{
            //    switch (filter.Name.ToLower())
            //    {
            //        case "filetype":
            //            filesQuerable = filesQuerable.Where(f => f.Type.ToString() == filter.Value.Value.ToString());
            //            break;
            //        case "creationdate":
            //            SearchRequestFilterRange<System.DateTime> range = null;
            //            if (filter.Value.TryGetRange(out range))
            //            {
            //                filesQuerable =
            //                    filesQuerable.Where(f => f.CreationDate > range.Min && f.CreationDate <= range.Max);
            //            }
            //            break;
            //        default:
            //            //TODO: for others dynamic properties

            //            break;
            //    }
            //}

            if (!string.IsNullOrEmpty(search))
            {
                filesQuerable =
                    filesQuerable.AsEnumerable()
                        .Where(
                            f =>
                                f.Name != null && f.Name.ToLower().Contains(search.ToLower()) ||
                                f.Description != null && f.Description.ToLower().Contains(search.ToLower()))
                        .AsQueryable();
            }

            var found = filesQuerable.ToList();
            foreach (var file in found)
            {
                var fileExtension = Path.GetExtension(file.Name);
                resut.Add(new SearchResultItem()
                {
                    //fileModel.DownloadURL = userFilesPath + '/' + fileModel.Id + fileExtension;
                    Description = file.Description,
                    IsAccessible = CheckThatFileIsAccessibleFor(file, user == null ? null : Mapper.Map(user, new DbUser())),
                    ItemType = SearchResultItemType.File,
                    //Link = user == null ? "/Account/Login" : userFilesPath + '/' + file.Id + fileExtension,
                    Link = "/Search/ResourceInfo?id=" + file.Id,
                    //Link = "/FileInfo?id=" + file.Id,
                    Name = file.Name,
                    Owner = file.Owner == null ? Mapper.Map(new DbUser(), new User()) : Mapper.Map(file.Owner, new User()),
                    ItemId = file.Id,
                    AccessMode = Mapper.Map<AccessModes>(file.AccessMode),
                    CreationDate = file.CreationDate
                });
            }

            return resut;
        }

        private IEnumerable<SearchResultItem> OrderSearchResultItems(IEnumerable<SearchResultItem> unordered, string property)
        {
            switch (property.ToLower())
            {
                case "creationdate":
                    return unordered.OrderBy(o => o.CreationDate);
                case "-creationdate":
                    return unordered.OrderByDescending(o => o.CreationDate);
                case "name":
                    return unordered.OrderBy(o => o.Name);
                case "-name":
                    return unordered.OrderByDescending(o => o.Name);
            }

            return unordered;
        }

        public bool CheckThatFileIsAccessible(Guid fileId, Guid userId)
        {
            var dbFile = _filesRepository.Find(f => f.Id == fileId);
            var dbUser = _usersRepository.Find(f => f.Id == userId);

            return CheckThatFileIsAccessibleFor(dbFile, dbUser);
        }
        private bool CheckThatFileIsAccessibleFor(DbUserFile file, DbUser dbUser)
        {
            if (file.AccessMode == DbUserFile.DbFileAccessMode.Public) return true;
            if (dbUser == null) return false;

            if (file.Owner == null || (file.Owner.Id == dbUser.Id)) return true;
            if (file.AccessMode == DbUserFile.DbFileAccessMode.Explicit)
            {
                var result = file.AcceptedUsers.Any(a => a.Id == dbUser.Id) ||
                             file.AcceptedGroups.Any(
                                 a =>
                                     ((a.IsGroup.HasValue && a.IsGroup.Value) || !a.IsGroup.HasValue) &&
                                     a.Users.Any(u => u.Id == dbUser.Id));
                return result;

            }
            else if (file.AccessMode == DbUserFile.DbFileAccessMode.Internal)
            {
                return true;
            }
            else
            {
                var result = file.AcceptedUsers.Any(a => a.Id == dbUser.Id);
                return result;

            }
            //else if (file.AccessMode == DbUserFile.DbFileAccessMode.Private)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            return false;
        }


        public bool CheckThatAnnotationItemsIsAccessible(Guid annotationItemId, Guid userId)
        {
            var dbAnnotation = _annotationItemRepository.Find(f => f.Id == annotationItemId);
            var dbUser = _usersRepository.Find(f => f.Id == userId);

            return CheckThatAnnotationItemsIsAccessibleFor(dbAnnotation, dbUser);
        }
        private bool CheckThatAnnotationItemsIsAccessibleFor(DbAnnotationItem annotationItem, DbUser dbUser)
        {
            if (annotationItem.AccessMode == AccessModes.Public) return true;
            if (dbUser == null) return false;

            if (annotationItem.OwnerId == null || (annotationItem.OwnerId == dbUser.Id.ToString())) return true;

            if (annotationItem.AccessMode == AccessModes.Explicit)
            {
                if (
                    _accecibleUsersRepository.Select()
                        .Any(a => a.User.Id == dbUser.Id && a.AnnotationItem.Id == annotationItem.Id) ||
                    _accecibleGroupsRepository.Select()
                        .Any(
                            a => a.Group.Users.Any(a1 => a1.Id == dbUser.Id) && a.AnnotationItem.Id == annotationItem.Id))
                {
                    return true;
                }
            }
            else if (annotationItem.AccessMode == AccessModes.Internal)
            {
                return true;
            }
            else
            {
                if (
                    _accecibleUsersRepository.Select()
                        .Any(a => a.User.Id == dbUser.Id && a.AnnotationItem.Id == annotationItem.Id))
                {
                    return true;
                }
            }
            //if (file.AccessMode == DbUserFile.DbFileAccessMode.Explicit)
            //{
            //    var result = file.AcceptedUsers.Any(a => a.Id == dbUser.Id) ||
            //                 file.AcceptedGroups.Any(
            //                     a =>
            //                         ((a.IsGroup.HasValue && a.IsGroup.Value) || !a.IsGroup.HasValue) &&
            //                         a.Users.Any(u => u.Id == dbUser.Id));
            //    return result;

            //}
            //else if (file.AccessMode == DbUserFile.DbFileAccessMode.Internal)
            //{
            //    return true;
            //}
            //else if (file.AccessMode == DbUserFile.DbFileAccessMode.Private)
            //{
            //    return false;
            //}
            //else
            //{
            return false;
            //}
        }

        private async Task<IEnumerable<SearchRequestComplexFilter>> GetFilterParametersForAllAsync(bool needToClear)
        {
            var list = new List<SearchRequestComplexFilter>();
            list.AddRange(await GetFilterParametersForFilesAsync());
            list.AddRange(await GetFilterParametersForAnnotationItemsAsync(needToClear));

            return list;
        }

        private async Task<IEnumerable<SearchRequestComplexFilter>> GetFilterParametersForFilesAsync()
        {
            await Task.Delay(0);
            var list = new List<SearchRequestComplexFilter>();

            //list.Add(new SearchRequestFilterItem()
            //{
            //    FilterType = FilterType.Select,
            //    Name = "FileType"
            //});
            //list.Add(new SearchRequestFilterItem()
            //{
            //    FilterType = FilterType.DateRange,
            //    Name = "CreationDate"
            //});

            return list;
        }

        private async Task<IEnumerable<SearchRequestComplexFilter>> GetFilterParametersForAnnotationItemsAsync(bool needToClear)
        {
            await Task.Delay(0);

            IEnumerable<SearchRequestComplexFilter> filters = new List<SearchRequestComplexFilter>();
            var listFilters = (List<SearchRequestComplexFilter>) filters;


            var filterDescriptions = _schemaDescriptionsRepository.FindCollection(f => true).ToList();

            IEnumerable<SearchRequestFilterItem> oldfilters = new List<SearchRequestFilterItem>();

            // First check that db exists cache of search filters model
            // and it have actual state
            needToClear = true;
            if (needToClear)
            //if (!HaveActualCachedSearchFilterItems() && needToClear)
            {
                var annotationObjectType = typeof(AnnotationObject);
                oldfilters = GetFiltersForType(annotationObjectType, "Object", "", filterDescriptions);

                //Save performed filter items model to cache
                try
                {
                    SaveToCacheFilterItems(oldfilters);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            else
            {
                oldfilters = GetFiltersForTypeFromCache(filterDescriptions);
            }


            filters = ComplexFiltersConfiguration.ComplexFiltersScopes.ToDomainList();
            foreach (var searchRequestComplexFilter in filters)
            {
                foreach (var searchRequestFilterItem in searchRequestComplexFilter.Items)
                {
                    var foundOldFilter =
                        oldfilters.Where(
                                f => f.Name.ToLower().StartsWith(searchRequestFilterItem.Name.Replace("[N]", "[]").Replace("[1]", "[]").ToLower()))
                            .FirstOrDefault();
                    if (foundOldFilter != null)
                        searchRequestFilterItem.ValueType = foundOldFilter.ValueType;
                    else
                    {
                        if (searchRequestFilterItem.Name.EndsWith("Persons[N].Position[1]"))
                            searchRequestFilterItem.ValueType = "PositionVocabulary";
                        else if(searchRequestFilterItem.Name.EndsWith("Hosters[N].HosterName"))
                            searchRequestFilterItem.ValueType = "HosterNameVocabulary";
                        else if (searchRequestFilterItem.Name.EndsWith("AccessMode"))
                            searchRequestFilterItem.ValueType = "AccessModes";
                        else if(searchRequestFilterItem.Name.EndsWith("Interactions[N].InteractionPartnerOne"))
                            searchRequestFilterItem.ValueType = "InteractionPartnerVocabulary";
                        else if (searchRequestFilterItem.Name.EndsWith("Interactions[N].InteractionPartnerTwo"))
                            searchRequestFilterItem.ValueType = "InteractionPartnerVocabulary";
                        //else if (searchRequestFilterItem.Name.EndsWith("Processes[N].ProcessSubject[1]"))
                        //    searchRequestFilterItem.ValueType = "ProcessSubjectVocabulary"; 
                        else
                            searchRequestFilterItem.ValueType = "String";
                    }
                }
            }
            return filters;

            //return filters;
        }

        private bool HaveActualCachedSearchFilterItems()
        {
            return _cachedSearchFilterRepository.Select().Any() && !_versionRepository.Find(f => true).IsNew;
            //return _cachedSearchFilterRepository.Select().Any();

        }

        private IEnumerable<SearchRequestFilterItem> GetFiltersForType(Type type, string pn, string directParentName, 
            IEnumerable<DbSchemaItemDescription> descriptions)
        {
            List<SearchRequestFilterItem> result = new List<SearchRequestFilterItem>();

            var props = type.GetProperties();

            foreach (var propertyInfo in props)
            {
                var prNm = propertyInfo.Name;
                var desc = propertyInfo.Name;

                var pt = propertyInfo.PropertyType;
                if (propertyInfo.PropertyType.IsGenericType)
                {
                    prNm += "[]";
                    pt = propertyInfo.PropertyType.GetGenericArguments()[0];
                    //prNm += "." + pt.Name;
                }

                if (pt.IsBaseValueType() && propertyInfo.Name != "LocationName")
                {
                    if (propertyInfo.Name == "Id") continue;

                    if (propertyInfo.Name == "Uri" || propertyInfo.Name == "Value")
                    {
                        desc = directParentName + "." + desc;
                    }

                    var pnArr = pn.Split('.');
                    var prevDesc = desc;
                    if (pnArr[pnArr.Length - 1].ToLower() == "datetime" && pnArr.Length >= 3)
                    {                        
                        desc = pnArr[pnArr.Length - 2] + "." + prevDesc;
                    }


                    var fileTypeFor = GetFileTypeFor(pt, prNm);

                    var name = string.IsNullOrEmpty(pn) ? prNm : pn + "." + prNm;
                    var path = name.Replace("[]", "");
                    var tooltip = "NA";
                    var foundDescription = descriptions.Where(d => d.Path.ToLower() == path.ToLower()).FirstOrDefault();
                    if (foundDescription != null)
                        tooltip = foundDescription.Description;
                    result.Add(new SearchRequestFilterItem()
                    {
                        FilterType = fileTypeFor,
                        Name = name,
                        Description = desc,
                        ValueType = propertyInfo.PropertyType.Name,
                        Tooltip = tooltip
                    });
                }
                else if (propertyInfo.PropertyType.Name == "DateAndTime")
                {
                    var fileTypeFor = FilterType.DateAndTime;

                    var name = string.IsNullOrEmpty(pn) ? prNm : pn + "." + prNm;
                    var path = name.Replace("[]", "");
                    var tooltip = "NA";
                    var foundDescription = descriptions.Where(d => d.Path.ToLower() == path.ToLower()).FirstOrDefault();
                    if (foundDescription != null)
                        tooltip = foundDescription.Description;
                    result.Add(new SearchRequestFilterItem()
                    {
                        FilterType = fileTypeFor,
                        Name = name,
                        Description = desc,
                        ValueType = propertyInfo.PropertyType.Name,
                        Tooltip = tooltip
                    });
                }
                else if ((pt.GetProperties().Any(p => p.Name == "Value") &&
                         pt.GetProperties().Any(p => p.Name == "Uri")) || propertyInfo.Name == "LocationName")
                {
                    var pnArr = pn.Split('.');
                    var prevDesc = desc;
                    if (pnArr[pnArr.Length - 1].ToLower() == "datetime" && pnArr.Length >= 3)
                    {
                        desc = pnArr[pnArr.Length - 2] + "." + prevDesc;
                    }

                    var name = string.IsNullOrEmpty(pn) ? prNm : pn + "." + prNm;
                    var path = name.Replace("[]", "");
                    var tooltip = "NA";
                    var foundDescription = descriptions.Where(d => d.Path.ToLower() == path.ToLower()).FirstOrDefault();
                    if (foundDescription != null)
                        tooltip = foundDescription.Description;

                    result.Add(new SearchRequestFilterItem()
                    {
                        FilterType = FilterType.Select,
                        Name = name,
                        Description = desc,
                        ValueType = $"{pt.Name}Vocabulary",
                        Tooltip = tooltip
                    });
                }
                else
                {
                    result.AddRange(GetFiltersForType(pt,
                        string.IsNullOrEmpty(pn) ? prNm : pn + "." + prNm, propertyInfo.Name, descriptions));
                }
            }

            return result;
        }

        private IEnumerable<SearchRequestFilterItem> GetFiltersForTypeFromCache(IEnumerable<DbSchemaItemDescription> descriptions)
        {
            var dbItems = _cachedSearchFilterRepository.Select().ToList();
            var mappedItems = Mapper.Map<IEnumerable<SearchRequestFilterItem>>(dbItems);

            foreach (var searchRequestFilterItem in mappedItems)
            {
                var foundDescription =
                    descriptions.Where(d => d.Path.ToLower() == searchRequestFilterItem.Name.Replace("[]", "").ToLower())
                        .FirstOrDefault();
                if (foundDescription != null)
                {
                    searchRequestFilterItem.Tooltip = foundDescription.Description;
                }
                else
                {
                    searchRequestFilterItem.Tooltip = "NA";
                }
            }
            return mappedItems;
        }

        private void SaveToCacheFilterItems(IEnumerable<SearchRequestFilterItem> items)
        {
            //Clear older filter cache
            //var dbItems = _cachedSearchFilterRepository.Select().ToList();
            //foreach (var dbSearchFilterCachedItem in dbItems)
            //{
            //    _cachedSearchFilterRepository.DeleteThroughState(dbSearchFilterCachedItem);
            //}
            _cachedSearchFilterRepository.Clear();

            //Fill to cache new items
            var mappedItems = Mapper.Map<IEnumerable<DbSearchFilterCachedItem>>(items);
            _cachedSearchFilterRepository.InsertCollection(mappedItems.ToList());
        }

        private Type TryFindVocabularyType(string name)
        {
            var assmName = Assembly.GetCallingAssembly().GetReferencedAssemblies().Where(a => a.Name == "CAFE.Core").FirstOrDefault();
            var assm = Assembly.Load(assmName);
            var foundType = assm.GetTypes().Where(t => t.Name.ToLower() == name.ToLower()).FirstOrDefault();

            return foundType;
        }

        private FilterType GetFileTypeFor(Type t, string propertyName)
        {
            if (t.Name == "DateTime")
            {
                return FilterType.DateAndTime;
            }
            else if (t.Name == "Boolean")
            {
                return FilterType.Flag;
            }
            else if (propertyName.ToLower() == "time")
            {
                return FilterType.DateAndTime;
            }
            else if (t.Name == "String")
            {
                var vocType = TryFindVocabularyType(propertyName);
                if (vocType == null)
                {
                    return FilterType.Simple;
                }
                else
                {
                    return FilterType.Select;
                }
            }
            else if (t.Name.StartsWith("Int") || t.Name == "Decimal" || t.Name == "Double")
            {
                return FilterType.DigitalRange;
            }
            else if (t.Name == "Byte[]")
            {
                return FilterType.Other;
            }
            else
            {
                throw new InvalidOperationException("Unknown type");
            }

        }

        private async Task<IEnumerable<VocabularyValue>> GetSelectValuesFromAnnotationItemsAsync(string property, string userId)
        {
            await Task.Delay(0);
            var result = new List<string>();

            var vocabularyType = TryFindVocabularyType(property);

            if (vocabularyType == null) throw new InvalidOperationException("Unknown vocabulary type");

            IEnumerable<VocabularyValue> vocs;
            try
            {
                vocs =
                    _vocabularyService.GetVocabularyValues(vocabularyType, userId).ToList();

            }
            catch (Exception exception)
            {
                throw;
            }
            return vocs;

        }

        private async Task<IEnumerable<VocabularyValue>> GetSelectValuesFromFilesAsync(string property)
        {
            await Task.Delay(0);

            var result = new List<VocabularyValue>();

            switch (property.ToLower())
            {
                case "filetype":
                    result.AddRange(_filesRepository.Select(s => new VocabularyValue{ Value = s.Type.ToString()}).ToList().Distinct());
                    break;
                default:
                    //TODO: for others dynamic properties

                    break;
            }
            return result;
        }

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        private bool IsSimple(Type type)
        {
            return type.IsPrimitive
              || type.Equals(typeof(string));
        }

        private string BuildSqlQueryFor(IEnumerable<SearchRequestComplexFilter> filters, string search)
        {
            var usedKeys = new List<string>()
            {
                "AI",
                "AO",
                //"CT"
            };
            StringBuilder builder = new StringBuilder("SELECT AI.* FROM [dbo].[DbAnnotationItem] AS AI INNER JOIN [dbo].[DbAnnotationObject] AS AO ON AI.Object_Id = AO.Id JOIN [dbo].[DbReferences] AS RF ON AO.References_Id = RF.Id JOIN [dbo].[DbDescription] AS DS ON DS.DbReferences_Id = RF.Id ");

            //StringBuilder builder = new StringBuilder("SELECT AI.* FROM [dbo].[DbAnnotationItem] AS AI INNER JOIN [dbo].[DbAnnotationObject] AS AO ON AI.Object_Id = AO.Id JOIN [dbo].[DbAnnotationContext] AS CT ON AI.Object_Id = CT.DbAnnotationObject_Id INNER JOIN [dbo].[DbReferences] AS RF ON AO.References_Id = RF.Id JOIN [dbo].[DbDescription] AS DS ON DS.DbReferences_Id = RF.Id ");

            //var contextType = typeof(DbAnnotationContext);
            var annotationObjectType = typeof(DbAnnotationObject);
            var filterStatements = new List<string>();
            var mappedJoinKeys = new Dictionary<string, string>();
            var addedPaths = new List<string>();

            var searchRequestComplexFilters = filters as SearchRequestComplexFilter[] ?? filters.ToArray();
            foreach (var searchRequestComplexFilter in searchRequestComplexFilters)
            {

                var complexFilterName = searchRequestComplexFilter.Name;

                switch (complexFilterName)
                {
                    default:
                        break;
                }

                foreach (var searchRequestFilterItem in searchRequestComplexFilter.Items)
                {
                    if((searchRequestFilterItem.Value == null || (searchRequestFilterItem.Value.Value == null || string.IsNullOrEmpty(searchRequestFilterItem.Value.Value.ToString()))) 
                        && (!searchRequestFilterItem.Values.Any())) continue;



                    //Access mode
                    if (searchRequestFilterItem.Name.EndsWith(".AccessMode"))
                    {
                        var intEnumStrings = new List<string>();
                        foreach (var searchRequestFilterValue in searchRequestFilterItem.Values)
                        {
                            intEnumStrings.Add(((int) Enum.Parse(typeof(AccessModes),
                                searchRequestFilterValue.Value.ToString())).ToString());

                        }

                        filterStatements.Add("AI.AccessMode IN (" + string.Join(",", intEnumStrings) + ") ");
                        continue;
                    }



                    var parsedFilterKey = ParseFiltersKey(searchRequestFilterItem);

                    var mappedPropertyTypes = new Dictionary<string, Type>();

                    for (int i = 1; i < parsedFilterKey.Length; i++)
                    {
                        var currentKey = parsedFilterKey[i];
                        var currentPathArray = new string[i];
                        Array.Copy(parsedFilterKey, 1, currentPathArray, 0, i);
                        var currentPath = string.Join(".", currentPathArray);

                        if (i == 1)
                        {
                            currentPath = currentKey;
                            var foundProperty = annotationObjectType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Where(p => p.Name == currentKey.Replace("[]", "")).FirstOrDefault();

                            if (foundProperty == null)
                                throw new ApplicationException("Requested property not found");

                            var propertyType = foundProperty.PropertyType;
                            var isCollection = false;

                            if(propertyType.GetInterfaces().Contains(typeof(IEnumerable)) && propertyType.IsGenericType)
                            //if (typeof(IEnumerable<>).IsAssignableFrom(propertyType))
                            {
                                propertyType = propertyType.GetGenericArguments()[0];
                                isCollection = true;
                            }

                            mappedPropertyTypes.Add(currentPath, propertyType);

                            var propertyTypeName = propertyType.Name;
                            var endPropertyTypeName = propertyTypeName;// + "s";
                                                                       //if (propertyTypeName.EndsWith("s"))
                                                                       //{
                                                                       //    var singledPropertyName = propertyTypeName.PadRight(propertyTypeName.Length - 1);
                                                                       //    if (AlreadyExistType(singledPropertyName))
                                                                       //        endPropertyTypeName += "1";
                                                                       //}

                            //if (AlreadyExistType(endPropertyTypeName))
                            //    endPropertyTypeName += "1";

                            var propertyKey = "";


                            if (!mappedJoinKeys.ContainsKey(currentKey))
                            {
                                propertyKey = "KEY" + RandomNumber(RandomNumber(1, 100), RandomNumber(1000, 10000)).ToString();
                                if (usedKeys.Contains(propertyKey))
                                    propertyKey = "KEY" + RandomNumber(RandomNumber(1, 100), RandomNumber(1000, 10000)).ToString();
                                usedKeys.Add(propertyKey);

                                mappedJoinKeys.Add(currentKey, propertyKey);
                            }
                            else
                            {
                                propertyKey = mappedJoinKeys[currentKey];
                            }

                            if (!addedPaths.Contains(currentPath))
                            {
                                if (isCollection)
                                {
                                    builder.Append("JOIN ");
                                    builder.Append("[dbo].[");
                                    builder.Append(endPropertyTypeName);
                                    builder.Append("] AS ");
                                    builder.Append(propertyKey);
                                    builder.Append(" ON ");
                                    builder.Append(propertyKey);
                                    builder.Append(".");
                                    builder.Append(annotationObjectType.Name);
                                    builder.Append("_Id = ");
                                    builder.Append("AO");
                                    builder.Append(".Id ");

                                }
                                else
                                {
                                    builder.Append("INNER JOIN ");
                                    builder.Append("[dbo].[");
                                    builder.Append(endPropertyTypeName);
                                    builder.Append("] AS ");
                                    builder.Append(propertyKey);
                                    builder.Append(" ON AO.");
                                    builder.Append(foundProperty.Name);
                                    builder.Append("_Id = ");
                                    builder.Append(propertyKey);
                                    builder.Append(".Id ");

                                }

                                addedPaths.Add(currentPath);
                            }
                        }
                        else if ((i + 1) == parsedFilterKey.Length)
                        {
                            var previousPathArray = new string[i - 1];
                            Array.Copy(parsedFilterKey, 1, previousPathArray, 0, i - 1);
                            var previousPath = string.Join(".", previousPathArray);

                            var previousKey = mappedJoinKeys[previousPath];

                            var previousType = mappedPropertyTypes[previousPath];
                            var previousTypeProperties = previousType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                            var currentProperty = previousTypeProperties
                                .Where(p => p.Name == currentKey.Replace("[]", "")).FirstOrDefault();

                            if (null == currentProperty)
                            {
                                var notSimpleProperties = previousTypeProperties.Where(p => !IsSimple(p.PropertyType));
                                foreach (var prop in notSimpleProperties)
                                {
                                    var props = prop.PropertyType.GetProperties();
                                    currentProperty = props.Where(p => p.Name == currentKey.Replace("[]", "")).FirstOrDefault();
                                    if (null != currentProperty)
                                        break;
                                }
                            }
                            var propertyType1 = currentProperty.PropertyType;
                            var isCollection1 = false;

                            var isPersistanceSerialized = false;
                            if (propertyType1.GetInterface("IEnumerable") != null)
                            {
                                if (propertyType1 == typeof(PersistableStringCollection))
                                {
                                    propertyType1 = typeof(string);
                                    isPersistanceSerialized = true;
                                }
                                if (propertyType1.IsGenericType)
                                    propertyType1 = propertyType1.GetGenericArguments()[0];
                                isCollection1 = true;
                            }

                            var currentPropertyType = propertyType1;
                            var operation = "=";

                            switch (searchRequestFilterItem.FilterType)
                            {
                                case FilterType.Simple:
                                case FilterType.Numeric:
                                    if (currentPropertyType.Name == "Int16" || currentPropertyType.Name == "Int32"
                                        || currentPropertyType.Name == "Int64" || currentPropertyType.Name == "UInt16"
                                        || currentPropertyType.Name == "UInt32" || currentPropertyType.Name == "UInt64"
                                        || currentPropertyType.Name == "Double" || currentPropertyType.Name == "Decimal")
                                    {
                                        if (searchRequestComplexFilter.Name == "Space.Bounding Box")
                                        {
                                            switch (searchRequestFilterItem.Name)
                                            {
                                                case "Object.Contexts[1].SpaceContext.BoundingBoxes[N].NorthBoundingCoordinate":
                                                case "Object.Contexts[1].SpaceContext.BoundingBoxes[N].EastBoundingCoordinate":
                                                    filterStatements.Add(previousKey + "." + currentKey.Replace("[]", "") + " <= " + searchRequestFilterItem.Value.Value + " ");

                                                    break;
                                                case "Object.Contexts[1].SpaceContext.BoundingBoxes[N].SouthBoundingCoordinate":
                                                case "Object.Contexts[1].SpaceContext.BoundingBoxes[N].WestBoundingCoordinate":
                                                    filterStatements.Add(previousKey + "." + currentKey.Replace("[]", "") + " >= " + searchRequestFilterItem.Value.Value + " ");

                                                    break;

                                            }
                                        }
                                        else
                                            filterStatements.Add(previousKey + "." + currentKey.Replace("[]", "") + " " + operation + " " + searchRequestFilterItem.Value.Value + " ");
                                    }
                                    else
                                    {
                                        filterStatements.Add(previousKey + "." + currentKey.Replace("[]", "") + /*(isPersistanceSerialized ? "_SerializedValue" : "") +*/ " " + operation + " N'" + searchRequestFilterItem.Value.Value + "' ");
                                    }
                                    break;
                                case FilterType.Select:
                                case FilterType.ReferenceValue:
                                    mappedPropertyTypes.Add(currentPath, propertyType1);

                                    var propertyTypeName1 = propertyType1.Name;
                                    var endPropertyTypeName1 = propertyTypeName1;
                                    //if (!endPropertyTypeName1.EndsWith("s"))
                                    //{
                                    //    endPropertyTypeName1 += "s";
                                    //}
                                    //var propertyNameForCheck = propertyTypeName1 + "s";
                                    //if (AlreadyExistType(propertyNameForCheck))
                                    //    endPropertyTypeName1 += "1";

                                    var propertyKey2 = "";
                                    if (!mappedJoinKeys.ContainsKey(currentPath))
                                    {
                                        propertyKey2 = "KEY" + RandomNumber(RandomNumber(1,100), RandomNumber(1000, 10000)).ToString();
                                        if (usedKeys.Contains(propertyKey2))
                                            propertyKey2 = "KEY" + RandomNumber(RandomNumber(1, 100), RandomNumber(1000, 10000)).ToString();
                                        usedKeys.Add(propertyKey2);

                                        mappedJoinKeys.Add(currentPath, propertyKey2);
                                    }
                                    else
                                    {
                                        propertyKey2 = mappedJoinKeys[currentPath];
                                    }

                                    var previousKey1 = mappedJoinKeys[previousPath];

                                    if (!addedPaths.Contains(currentPath))
                                    {


                                        if (isCollection1)
                                        {
                                            builder.Append("JOIN ");
                                            builder.Append("[dbo].[");
                                            builder.Append(endPropertyTypeName1);
                                            builder.Append("] AS ");
                                            builder.Append(propertyKey2);
                                            builder.Append(" ON ");
                                            builder.Append(propertyKey2);
                                            builder.Append(".");
                                            builder.Append(previousType.Name);
                                            builder.Append("_Id = ");
                                            builder.Append(previousKey1);
                                            builder.Append(".Id ");

                                        }
                                        else
                                        {
                                            builder.Append("INNER JOIN ");
                                            builder.Append("[dbo].[");
                                            builder.Append(endPropertyTypeName1);
                                            builder.Append("] AS ");
                                            builder.Append(propertyKey2);
                                            builder.Append(" ON ");
                                            builder.Append(previousKey);
                                            builder.Append(".");
                                            builder.Append(currentProperty.Name);
                                            builder.Append("_Id = ");
                                            builder.Append(previousKey1);
                                            builder.Append(".Id ");
                                        }

                                        addedPaths.Add(currentPath);
                                    }

                                    if (searchRequestFilterItem.Values != null && searchRequestFilterItem.Values.Any())
                                        filterStatements.Add(propertyKey2 + ".Value IN (" + string.Join(",", searchRequestFilterItem.Values.ToSqlParameterStrings()) + ") ");
                                    break;
                                //case FilterType.Flag:
                                //    filterStatements.Add(previousKey + "." + currentKey + " " + operation + " " + filterItem.Value.Value + " ");
                                //    break;
                                case FilterType.DateAndTime:
                                    var date = DateTime.Parse(searchRequestFilterItem.Value.Value.ToString());

                                    if (searchRequestComplexFilter.Name == "Time.Time Ranges")
                                    {
                                        if(searchRequestFilterItem.Name == "Object.Contexts[1].TimeContext.TimeRanges.TimeRange.RangeStart.DateTime")
                                            filterStatements.Add(previousKey + "." + currentKey.Replace("[]", "") + " >= '" + date.Date + "' ");
                                        else
                                            filterStatements.Add(previousKey + "." + currentKey.Replace("[]", "") + " <= '" + date.Date + "' ");
                                    }
                                    else
                                        filterStatements.Add(previousKey + "." + currentKey.Replace("[]", "") + " = '" + date.Date + "' ");
                                    break;
                                    //case FilterType.DigitalRange:
                                    //    SearchRequestFilterRange<double> intRange;
                                    //    if (filterItem.Value.TryGetRange(out intRange))
                                    //    {
                                    //        filterStatements.Add("(" + previousKey + "." + currentKey + " BETWEEN " + intRange.Min + " AND " + intRange.Max + ") ");
                                    //    }
                                    //    break;
                            }
                        }
                        else
                        {
                            var s1Array = new string[i - 1];
                            Array.Copy(parsedFilterKey, 1, s1Array, 0, i - 1);
                            var s2 = string.Join(".", s1Array);

                            var s3 = mappedPropertyTypes[s2];

                            var s4 = s3.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                            var foundProperty1 = s4
                                .Where(p => p.Name == currentKey.Replace("[]", "")).FirstOrDefault();

                            if (foundProperty1 == null)
                                throw new ApplicationException("Requested property not found");

                            var propertyType1 = foundProperty1.PropertyType;
                            var isCollection1 = false;

                            if (propertyType1.GetInterface("IEnumerable") != null)
                            {
                                propertyType1 = propertyType1.GetGenericArguments()[0];
                                isCollection1 = true;
                            }

                            mappedPropertyTypes.Add(currentPath, propertyType1);

                            var propertyTypeName1 = propertyType1.Name;
                            var endPropertyTypeName1 = propertyTypeName1;

                            var propertyKey1 = "";
                            if (!mappedJoinKeys.ContainsKey(currentPath))
                            {
                                propertyKey1 = "KEY" + RandomNumber(RandomNumber(1, 100), RandomNumber(1000, 10000)).ToString();
                                if (usedKeys.Contains(propertyKey1))
                                    propertyKey1 = "KEY" + RandomNumber(RandomNumber(1, 100), RandomNumber(1000, 10000)).ToString();
                                usedKeys.Add(propertyKey1);

                                mappedJoinKeys.Add(currentPath, propertyKey1);
                            }
                            else
                            {
                                propertyKey1 = mappedJoinKeys[currentPath];
                            }

                            var previousKey = mappedJoinKeys[s2];

                            if (!addedPaths.Contains(currentPath))
                            {


                                if (isCollection1)
                                {
                                    builder.Append("JOIN ");
                                    builder.Append("[dbo].[");
                                    builder.Append(endPropertyTypeName1);
                                    builder.Append("] AS ");
                                    builder.Append(propertyKey1);
                                    builder.Append(" ON ");
                                    builder.Append(propertyKey1);
                                    builder.Append(".");
                                    builder.Append(s3.Name);
                                    builder.Append("_Id = ");
                                    builder.Append(previousKey);
                                    builder.Append(".Id ");

                                }
                                else
                                {
                                    builder.Append("INNER JOIN ");
                                    builder.Append("[dbo].[");
                                    builder.Append(endPropertyTypeName1);
                                    builder.Append("] AS ");
                                    builder.Append(propertyKey1);
                                    builder.Append(" ON ");
                                    builder.Append(previousKey);
                                    builder.Append(".");
                                    builder.Append(foundProperty1.Name);
                                    builder.Append("_Id = ");
                                    builder.Append(propertyKey1);
                                    builder.Append(".Id ");
                                }

                                addedPaths.Add(currentPath);
                            }
                        }
                    }
                }
            }


            if (!string.IsNullOrEmpty(search) || filterStatements.Any())
                builder.Append("WHERE ");


            foreach (var filterStatement in filterStatements)
            {
                builder.Append(filterStatement);
                builder.Append(" AND ");
            }
            if (string.IsNullOrEmpty(search) && searchRequestComplexFilters.Any())
            {
                builder.Remove(builder.Length - 6, 5);
            }
            else if (!string.IsNullOrEmpty(search))
            {
                builder.Append("(DS.Abstract LIKE N'%");
                builder.Append(search);
                builder.Append("%' OR DS.Title LIKE N'%");
                builder.Append(search);
                builder.Append("%')");
            }

            var resString = builder.ToString();
            return resString;
        }

        private string[] ParseFiltersKey(SearchRequestFilterItem filterItem)
        {
            var parsedFilterKey = filterItem.Name.Replace("[1]", "[]").Replace("[N]", "[]")
                //.Replace("[]", "")
                .Split('.');


            //// getting last item to check
            //// if it 'Select' type and need additional join performs
            //// also it must contains 'Value' propty
            //var lastItemIsSelect = CheckIsItemIsSelectType(parsedFilterKey[parsedFilterKey.Length - 1]);
            //if (lastItemIsSelect)
            //{
            //    var tempArr = new string[parsedFilterKey.Length];
            //    Array.Copy(parsedFilterKey, 0, tempArr, 0, parsedFilterKey.Length);

            //    parsedFilterKey = new string[tempArr.Length + 1];
            //    Array.Copy(tempArr, 0, parsedFilterKey, 0, tempArr.Length);
            //    parsedFilterKey[parsedFilterKey.Length - 1] = "Value";
            //    tempArr = null;
            //}
            if (filterItem.ValueType == typeof(DateAndTime).Name)
            {
                var tempArr = new string[parsedFilterKey.Length];
                Array.Copy(parsedFilterKey, 0, tempArr, 0, parsedFilterKey.Length);

                parsedFilterKey = new string[tempArr.Length + 1];
                Array.Copy(tempArr, 0, parsedFilterKey, 0, tempArr.Length);
                parsedFilterKey[parsedFilterKey.Length - 1] = "Date";
                tempArr = null;
            }

            return parsedFilterKey;
        }

        private bool CheckIsItemIsSelectType(string keyName)
        {
            var foundType = TryFindVocabularyType(keyName);
            return foundType != null;
        }

        private bool AlreadyExistType(string multiplicatedName)
        {
            var namespc = "CAFE.DAL.Models";
            var asmName = "CAFE.DAL";

            var entryAssm = Assembly.GetExecutingAssembly();
            var dalAssemblyName = entryAssm.GetReferencedAssemblies().Where(a => a.Name == asmName).FirstOrDefault();
            var dalAssembly = Assembly.Load(dalAssemblyName);
            var dataModels = dalAssembly.GetTypes().Where(t => t.Namespace == namespc).ToList();

            if (dataModels.Any(a => a.Name == multiplicatedName))
                return true;

            return false;
        }

        #endregion
    }
}

