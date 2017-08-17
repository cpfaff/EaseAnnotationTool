
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CAFE.Web.Configuration
{
    public class ComplexSearchFiltersConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("ComplexFiltersScopes", IsRequired = true)]
        public ConfigElementsCollection RelatedFiltersScopes
        {
            get
            {
                return base["ComplexFiltersScopes"] as ConfigElementsCollection;
            }
        }
        public class ComplexFilterScope : ConfigurationElement
        {

            [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
            public string Type
            {
                get
                {
                    return base["type"] as string;
                }
                set
                {
                    base["type"] = value;
                }
            }

            [ConfigurationProperty("ComplexFiltersCollection")]
            public ConfigSubElementsCollection RelatedFiltersCollection
            {
                get
                {
                    return base["ComplexFiltersCollection"] as ConfigSubElementsCollection;
                }
            }

        }

        [ConfigurationCollection(typeof(ComplexFilterScope), AddItemName = "ComplexFilterScope")]
        public class ConfigElementsCollection : ConfigurationElementCollection, IEnumerable<ComplexFilterScope>
        {

            protected override ConfigurationElement CreateNewElement()
            {
                return new ComplexFilterScope();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                var l_configElement = element as ComplexFilterScope;
                if (l_configElement != null)
                    return l_configElement.Type;
                else
                    return null;
            }

            public ComplexFilterScope this[int index]
            {
                get
                {
                    return BaseGet(index) as ComplexFilterScope;
                }
            }

            #region IEnumerable<ComplexFilterScope> Members

            IEnumerator<ComplexFilterScope> IEnumerable<ComplexFilterScope>.GetEnumerator()
            {
                return (from i in Enumerable.Range(0, this.Count)
                        select this[i])
                        .GetEnumerator();
            }

            #endregion
        }

        [ConfigurationCollection(typeof(FilterElement), AddItemName = "FilterElement")]
        public class ConfigSubElementsCollection : ConfigurationElementCollection, IEnumerable<FilterElement>
        {

            protected override ConfigurationElement CreateNewElement()
            {
                return new FilterElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                var l_configElement = element as FilterElement;
                if (l_configElement != null)
                    return l_configElement.Property;
                else
                    return null;
            }

            public FilterElement this[int index]
            {
                get
                {
                    return BaseGet(index) as FilterElement;
                }
            }

            #region IEnumerable<FilterElement> Members

            IEnumerator<FilterElement> IEnumerable<FilterElement>.GetEnumerator()
            {
                return (from i in Enumerable.Range(0, this.Count)
                        select this[i])
                        .GetEnumerator();
            }

            #endregion
        }

        public class FilterElement : ConfigurationElement
        {

            [ConfigurationProperty("property", IsKey = true, IsRequired = true)]
            public string Property
            {
                get
                {
                    return base["property"] as string;
                }
                set
                {
                    base["property"] = value;
                }
            }

        }
    }
}