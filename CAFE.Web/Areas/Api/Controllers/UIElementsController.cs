
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.Web.Areas.Api.Models;
using NLog;
using CAFE.Plugins.GlobalNameResolveService;
using System.Web.Http;

namespace CAFE.Web.Areas.Api.Controllers
{
    public class UIElementsController : ApiController
    {
        private readonly IRepository<DbUIElement> _uiElementsRepository;

        #region ctor

        public UIElementsController(ILogger logger, ISecurityService securityService,
            IRepository<DbUIElement> uiElementsRepository)// : base(logger, securityService)
        {
            _uiElementsRepository = uiElementsRepository;
        }

        #endregion

        /// <summary>
        /// Returns all UI elements
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        public IEnumerable<UIElementModel> Get()
        {
            var result = _uiElementsRepository.Select().ToList();
            return Mapper.Map<IEnumerable<DbUIElement>, IEnumerable<UIElementModel>>(result);
        }


        /// <summary>
        /// Find existing UI elements id's by query
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        public IEnumerable<UIElementIdModel> Search(string query)
        {
            List<UIElementIdModel> result = new List<UIElementIdModel>();
            var config = ConfigurationResolver.GetConfig();

            var itemsQuser = config.AvailableUIElements.AsQueryable();

            if (!string.IsNullOrEmpty(query))
                itemsQuser = itemsQuser.Where(f => f.Title.ToLower().Contains(query.ToLower()) || f.Id.ToLower().Contains(query.ToLower()));

            foreach (var item in itemsQuser)
            {
                result.Add(new UIElementIdModel
                {
                    Id = item.Id,
                    Title = item.Title
                });
            }

            return result;
        }

        /// <summary>
        /// Adds new UI element
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        public UIElementModel Add(UIElementModel model)
        {
            var mappedData = Mapper.Map<UIElementModel, DbUIElement>(model);
            _uiElementsRepository.Insert(mappedData);
            return Mapper.Map<DbUIElement, UIElementModel>(mappedData);
        }

        /// <summary>
        /// Updates existing UI element
        /// </summary>
        /// <param name="model"></param>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        public void Update(UIElementModel model)
        {
            var mappedData = Mapper.Map<UIElementModel, DbUIElement>(model);
            if (model.Id == 0)
            {
                _uiElementsRepository.Insert(mappedData);
            }
            else
            {
                _uiElementsRepository.Update(mappedData);
            }

        }

        /// <summary>
        /// Removes existing UI element
        /// </summary>
        /// <param name="model"></param>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        public void DeleteElements(IEnumerable<SimpleUIElementModel> model)
        {
            foreach (var item in model)
            {
                var foundItem = _uiElementsRepository.Find(f => f.Id == item.Id);
                _uiElementsRepository.Delete(foundItem);
            }
        }
    }
}