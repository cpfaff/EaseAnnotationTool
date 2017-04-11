using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Core.Resources
{
    public class CoordinatesCSVModel
    {
        public string UtmCoordinateZone { get; set; }

        public string UtmCoordinateSubZone { get; set; }

        public decimal UtmCoordinateEasting { get; set; }

        public string UtmCoordinateEastingUnit { get; set; }

        public decimal UtmCoordinateNorthing { get; set; }

        public string UtmCoordinateNorthingUnit { get; set; }

        public string UtmCoordinateHemisphere { get; set; }

        public string UtmCoordinateNorthingUnitUri { get; set; }

        public string UtmCoordinateEastingUnitUri { get; set; }

        public string UtmCoordinateHemisphereUri { get; set; }
    }
}