using System.Collections.Generic;

namespace CAFE.Core.Configuration
{
    public class ComplexSearchFiltersConfiguration : IConfiguration
    {
        public IEnumerable<ComplexFilterScope> ComplexFiltersScopes { get; set; } = new List<ComplexFilterScope>();


        public ComplexFilterScope GetComplexFieldsByFilterItem(string filterItemName)
        {
            ComplexFilterScope foudScope = null;
            foreach (var complexScope in ComplexFiltersScopes)
            {
                if (foudScope != null)
                    break;
                foreach(var complexFilter in complexScope.ComplexFiltersCollection)
                {
                    if(complexFilter.Property.ToLower() == filterItemName.ToLower())
                    {
                        foudScope = complexScope;
                        break;
                    }
                }
            }

            return foudScope;
        }
    }

    public class ComplexFilterScope
    {

        public string Type { get; set; }

        public IEnumerable<Complex.FilterElement> ComplexFiltersCollection { get; set; } = new List<Complex.FilterElement>();

    }

    namespace Complex
    {
        public class FilterElement
        {

            public string Property { get; set; }

        }
    }




}
