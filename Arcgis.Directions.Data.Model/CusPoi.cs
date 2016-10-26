using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcgis.Directions.Data.Model
{
    public class CusPoi
    {
        public int PoiID { get; set; }
        public string PoiName { get; set; }
        public string PoiDesc { get; set; }
        public string SettleName { get; set; }
        public string Address { get; set; }
        public string HouseNr { get; set; }
        public double GeocodeStatus { get; set; }
        public double? WmX { get; set; }
        public double? WmY { get; set; }
        public double? HtrsX { get; set; }
        public double? HTRS_Y { get; set; }
    }
}
