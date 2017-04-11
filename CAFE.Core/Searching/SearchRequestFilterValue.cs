
using System;
using System.Globalization;

namespace CAFE.Core.Searching
{
    public class SearchRequestFilterValue
    {
        public object Value { get; set; }

        public bool TryGetValue<T>(out T value)
        {
            value = default(T);
            return true;
        }

        public bool TryGetRange<T>(out SearchRequestFilterRange<T> value)
        {
            value = new SearchRequestFilterRange<T>();
            var vals = Value.ToString().Split(new [] {'-'}, StringSplitOptions.RemoveEmptyEntries);

            switch (typeof(T).Name)
            {
                case "DateTime":
                    DateTime resultMin;
                    DateTime resultMax;

                    var decisionMin = DateTime.TryParseExact(vals[0], "dd.M.yyyy h:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultMin);
                    var decisionMax = DateTime.TryParseExact(vals[1], "dd.M.yyyy h:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultMax);
                    if (decisionMax && decisionMin)
                    {
                        value.Min = (T)((object)resultMin);
                        value.Max = (T)((object)resultMax);

                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

    }
}
