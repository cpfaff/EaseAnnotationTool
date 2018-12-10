

using System.Collections.Generic;

namespace CAFE.Core.Resources
{
    public class SpeciesResolveValue
    {
        public ICollection<GlobalNamesRankValue> Names { get; set; }
        public string Species { get; set; }
        public double Score { get; set; }
    }
}
