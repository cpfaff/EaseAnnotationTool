using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace CAFE.Core.Configuration
{
    public class RelatedSearchFiltersConfiguration : IConfiguration
    {
        public IEnumerable<RelatedFilterScope> RelatedFiltersScopes { get; set; } = new List<RelatedFilterScope>();


        public RelatedFilterScope GetRelatedFieldsByFilterItem(string filterItemName)
        {
            RelatedFilterScope foudScope = null;
            foreach (var relatedScope in RelatedFiltersScopes)
            {
                if (foudScope != null)
                    break;
                foreach(var relatedFilter in relatedScope.RelatedFiltersCollection)
                {
                    if(relatedFilter.Key.ToLower() == filterItemName.ToLower())
                    {
                        foudScope = relatedScope;
                        break;
                    }
                }
            }

            //if(foudScope != null)
            //{
            //    foreach(var relatedFilter in foudScope.RelatedFiltersCollection.Where(w => 
            //        w.Key.ToLower() != filterItemName.ToLower()))
            //    {
            //        related.Add(relatedFilter.Key);
            //    }
            //}

            return foudScope;
        }
    }

    public class RelatedFilterScope
    {

        public string Key { get; set; }

        public IEnumerable<FilterElement> RelatedFiltersCollection { get; set; } = new List<FilterElement>();

    }

    public class FilterElement
    {

        public string Key { get; set; }

    }



}
