
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CAFE.Web.Areas.Api.Models
{
    /// <summary>
    /// Contains data about UI element on Annotation Item form and relation with it Resource url for get data
    /// </summary>
    [Serializable]
    public class UIElementModel
    {
        /// <summary>
        /// Element identifier on Annotation Form
        /// </summary>
        [JsonProperty(PropertyName = "elementid")]
        public string ElementIdOnUI { get; set; }

        /// <summary>
        /// Url for get data to load in UI element
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string UrlForGetData { get; set; }

        /// <summary>
        /// Element key in db
        /// </summary>
        public int Id { get; set; }
    }
}