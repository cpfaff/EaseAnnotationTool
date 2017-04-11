using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace CAFE.Web.Areas.Api.Models.Search
{
    public class SearchFilerUrlParametersModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(SearchRequestFilterUrlParameters))
            {
                return false;
            }

            ValueProviderResult val = bindingContext.ValueProvider.GetValue(
                bindingContext.ModelName);
            if (val == null)
            {
                return false;
            }

            string key = val.RawValue as string;
            if (key == null)
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, "Wrong value type");
                return false;
            }

            var result = new SearchRequestFilterUrlParameters();
            try
            {
                var pairs = key.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var pair in pairs)
                {
                    var keyValue = pair.Split('=');
                    result.Add(new KeyValuePair<string, string>(keyValue[0], keyValue[1]));
                }
            }
            catch
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, "Cannot convert filter parameters");
                return false;
            }
            bindingContext.Model = result;
            return true;

        }
    }
}