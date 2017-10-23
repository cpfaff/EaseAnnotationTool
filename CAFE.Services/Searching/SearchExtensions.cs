
using System.Collections.Generic;
using System.Linq;
using CAFE.Core.Searching;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace CAFE.Services.Searching
{
    public static class SearchExtensions
    {
        public static string[] ToSqlParameterStrings(this List<SearchRequestFilterValue> values)
        {
            //Returns to SQL parameter format like N'value'
            return values.Select(s => "N'" + s.Value.ToString() + "'").ToArray();
        }
    }
}
