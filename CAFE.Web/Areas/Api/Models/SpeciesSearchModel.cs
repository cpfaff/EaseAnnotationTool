
namespace CAFE.Web.Areas.Api.Models
{
    public class SpeciesSearchModel
    {
        public string Search { get; set; }
        public double MinScore { get; set; }
        public double Coverage { get; set; }
        public TaxonomyImportModel Import { get; set; }
    }
}