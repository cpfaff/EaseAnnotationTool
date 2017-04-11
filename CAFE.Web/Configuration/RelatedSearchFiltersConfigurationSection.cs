
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CAFE.Web.Configuration
{
    public class RelatedSearchFiltersConfigurationSection : ConfigurationSection {

        [ConfigurationProperty( "RelatedFiltersScopes", IsRequired = true )]
        public ConfigElementsCollection RelatedFiltersScopes {
            get {
                return base["RelatedFiltersScopes"] as ConfigElementsCollection;
            }
        }

    }

    [ConfigurationCollection( typeof( RelatedFilterScope ), AddItemName = "RelatedFilterScope" )]
    public class ConfigElementsCollection : ConfigurationElementCollection, IEnumerable<RelatedFilterScope> {

        protected override ConfigurationElement CreateNewElement() {
            return new RelatedFilterScope();
        }

        protected override object GetElementKey( ConfigurationElement element ) {
            var l_configElement = element as RelatedFilterScope;
            if ( l_configElement != null )
                return l_configElement.Key;
            else
                return null;
        }

        public RelatedFilterScope this[int index] {
            get {
                return BaseGet( index ) as RelatedFilterScope;
            }
        }

        #region IEnumerable<RelatedFilterScope> Members

        IEnumerator<RelatedFilterScope> IEnumerable<RelatedFilterScope>.GetEnumerator() {
            return ( from i in Enumerable.Range( 0, this.Count )
                     select this[i] )
                    .GetEnumerator();
        }

        #endregion
    }

    public class RelatedFilterScope : ConfigurationElement {

        [ConfigurationProperty( "key", IsKey = true, IsRequired = true )]
        public string Key {
            get {
                return base["key"] as string;
            }
            set {
                base["key"] = value;
            }
        }

        [ConfigurationProperty( "RelatedFiltersCollection" )]
        public ConfigSubElementsCollection RelatedFiltersCollection {
            get {
                return base["RelatedFiltersCollection"] as ConfigSubElementsCollection;
            }
        }

    }

    [ConfigurationCollection( typeof( FilterElement ), AddItemName = "FilterElement" )]
    public class ConfigSubElementsCollection : ConfigurationElementCollection, IEnumerable<FilterElement> {

        protected override ConfigurationElement CreateNewElement() {
            return new FilterElement();
        }

        protected override object GetElementKey( ConfigurationElement element ) {
            var l_configElement = element as FilterElement;
            if ( l_configElement != null )
                return l_configElement.Key;
            else
                return null;
        }

        public FilterElement this[int index] {
            get {
                return BaseGet( index ) as FilterElement;
            }
        }

        #region IEnumerable<FilterElement> Members

        IEnumerator<FilterElement> IEnumerable<FilterElement>.GetEnumerator() {
            return ( from i in Enumerable.Range( 0, this.Count )
                     select this[i] )
                    .GetEnumerator();
        }

        #endregion
    }

    public class FilterElement : ConfigurationElement {

        [ConfigurationProperty( "key", IsKey = true, IsRequired = true )]
        public string Key {
            get {
                return base["key"] as string;
            }
            set {
                base["key"] = value;
            }
        }

    }


}