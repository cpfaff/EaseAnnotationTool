using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAFE.Web.ValidationAttributes
{
    public class AtLeastOneAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Validator.TryValidateObject(value, context, results, true);
            var collection = value as IEnumerable<object>;

            if(!collection.Any())
            {
                var compositeResults = new CompositeValidationResult(String.Format("Collection {0} must have one or more elements.", validationContext.DisplayName));
                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }
}
