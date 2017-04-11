
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Types
{
    [ComplexType]
    public class PersistableStringCollection : PersistableScalarCollection<string>
    {
        protected override string ConvertSingleValueToRuntime(string rawValue)
        {
            return rawValue;
        }

        protected override string ConvertSingleValueToPersistable(string value)
        {
            return value?.ToString();
        }
    }
}
