
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
            var rng = Value.ToString();
            var firstNeg = false;

            switch (typeof(T).Name)
            {
                case "DateTime":
                    DateTime resultMin;
                    DateTime resultMax;

                    var decisionMin = DateTime.TryParseExact(vals[0], "d.M.yyyy h:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultMin);
                    var decisionMax = DateTime.TryParseExact(vals[1], "d.M.yyyy h:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultMax);
                    if (decisionMax && decisionMin)
                    {
                        value.Min = (T)((object)resultMin);
                        value.Max = (T)((object)resultMax);

                        return true;
                    }
                    return false;
                case "Int32":
                    if (rng[0] == '-')
                        firstNeg = true;

                    int min, max;
                    if(int.TryParse(vals[0], out min) && int.TryParse(vals[1], out max))
                    {
                        value.Min = (T)((object)(min * (firstNeg ? -1 : 1)));
                        value.Max = (T)((object)max);
                        return true;
                    }
                    return false;

                case "Double":
                    if (rng[0] == '-')
                        firstNeg = true;

                    double minD, maxD;
                    if (double.TryParse(vals[0], out minD) && double.TryParse(vals[1], out maxD))
                    {
                        value.Min = (T)((object)(minD * (firstNeg ? -1 : 1)));
                        value.Max = (T)((object)maxD);
                        return true;
                    }
                    return false;


                default:
                    return false;
            }
        }

    }
}
