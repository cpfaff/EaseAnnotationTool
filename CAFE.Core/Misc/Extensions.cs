
using System;

namespace CAFE.Core.Misc
{
    public static class Extensions
    {
        public static bool IsBaseValueType(this Type t)
        {
            return t.FullName.StartsWith("System.") && t.Name.ToLower() != "object";
        }

        public static bool IsNotNull(this object source)
        {
            var s = source as string;
            if (s != null) return !string.IsNullOrEmpty(s);
            return source != null;
        }
        public static bool IsNull(this object source)
        {
            return !IsNotNull(source);
        }
    }
}
