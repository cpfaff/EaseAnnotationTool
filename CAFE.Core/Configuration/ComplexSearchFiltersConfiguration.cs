using System.Collections.Generic;

namespace CAFE.Core.Configuration
{
    public class ComplexSearchFiltersConfiguration : IConfiguration
    {
        public IEnumerable<ComplexFilterScope> ComplexFiltersScopes { get; set; }


        public ComplexFilterScope GetComplexFieldsByFilterItem(string filterItemName)
        {
            ComplexFilterScope foudScope = null;
            foreach (var complexScope in ComplexFiltersScopes)
            {
                if (foudScope != null)
                    break;
                foreach (var complexFilter in complexScope.ComplexFiltersCollection)
                {
                    if (complexFilter.Property.ToLower() == filterItemName.ToLower())
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

        public string BasePath { get; set; }
        public string Tooltip { get; set; }

        public IEnumerable<Complex.FilterElement> ComplexFiltersCollection { get; set; } = new List<Complex.FilterElement>();

    }

    namespace Complex
    {
        public class FilterElement
        {

            public string Property { get; set; }
            public string Description { get; set; }
            public FilterElementValueType Type { get; set; }
        }

        public enum FilterElementValueType
        {
            Select,
            Simple,
            DateAndTime,
            ReferenceValue,
            Numeric
        }
    }




}
