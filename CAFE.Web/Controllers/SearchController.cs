
using CAFE.Core.Configuration;
using CAFE.Core.Searching;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.Web.Models.Search;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CAFE.Web.Controllers
{
    public class SearchController : Controller
    {
        private IRepository<DbAnnotationItem> _annotationItemsRepository;
        private IRepository<DbUserFile> _usersFilesRepository;
        private IConfigurationProvider _configurationProvider;
        private IRepository<DbUser> _usersRepository;
        private ISearchService _searchService;

        public SearchController(IRepository<DbAnnotationItem> annotationItemsRepository,
            IRepository<DbUserFile> usersFilesRepository, IConfigurationProvider configurationProvider,
            IRepository<DbUser> usersRepository, ISearchService searchService)
        {
            _annotationItemsRepository = annotationItemsRepository;
            _usersFilesRepository = usersFilesRepository;
            _configurationProvider = configurationProvider;
            _usersRepository = usersRepository;
            _searchService = searchService;
        }


        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ResourceInfo(Guid id)
        {
            var model = new ResourceInfoModel();
            var userId = User.Identity.GetUserId();
            var currentUser = _usersRepository.Find(f => f.Id.ToString() == userId);

            if (_annotationItemsRepository.Select().Any(a => a.Id == id))
            {
                var foundAi = _annotationItemsRepository.Select().Where(w => w.Id == id).First();

                var firstDescription = foundAi.Object.References.Descriptions.First();
                model.Title = firstDescription.Title;
                model.Abstract = firstDescription.Abstract;
                model.OwnerName = foundAi.OwnerName;
                model.OwnerId = foundAi.OwnerId;

                if (null != userId)
                    model.HasAccess = _searchService.CheckThatAnnotationItemsIsAccessible(id, Guid.Parse(userId));

                model.Id = id;
                model.ResourceType = SearchResultItemType.AnnotationItem;
                model.Hosters = foundAi.Object.References.Hosters.Select(s => s.HosterName.Value).ToList();
                model.Link = "/AnnotationItem?id=" + id;

                foreach (var person in foundAi.Object.References.Persons)
                {
                    string email = "";
                    string phone = "";
                    string givenName = "";
                    string position = "";
                    string salutation = "";
                    string surname = "";

                    if (person.EmailAddress.Any())
                        email = person.EmailAddress.First();
                    if (person.PhoneNumber.Any())
                        phone = person.PhoneNumber.First();
                    if (person.GivenName.Any())
                        givenName = person.GivenName.First();
                    if (person.Position.Any())
                        position = person.Position.First().Value;
                    if (person.Salutation.Any())
                        salutation = person.Salutation.First();
                    if (person.SurName.Any())
                        surname = person.SurName.First();


                    model.Persons.Add(new PersonModel()
                    {
                        Email = email,
                        GivenName = givenName,
                        Phone = phone,
                        Position = position,
                        Salutation = salutation,
                        Surname = surname
                    });
                }
            }
            else if(_usersFilesRepository.Select().Any(a => a.Id == id))
            {
                var appConfig = _configurationProvider.Get<ApplicationConfiguration>();
                var basePath = appConfig.ApploadsRoot;
                var userFilesPath = Path.Combine(basePath, "UserFiles");

                var foundFile = _usersFilesRepository.Select().Where(w => w.Id == id).First();
                var fileExtension = Path.GetExtension(foundFile.Name);

                model.Title = foundFile.Name;
                model.Abstract = foundFile.Description;
                model.OwnerName = foundFile.Owner.Name;
                model.OwnerId = foundFile.Owner.Id.ToString();
                model.HasAccess = _searchService.CheckThatFileIsAccessible(id, Guid.Parse(userId));
                model.Id = id;
                model.ResourceType = SearchResultItemType.File;
                model.Link = userFilesPath + '/' + foundFile.Id + fileExtension;

            }
            else
            {
                throw new ApplicationException("Unknown file or annotation item");
            }
            return View(model);
        }

        
    }
}