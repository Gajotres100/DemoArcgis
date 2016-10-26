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
        public float GeocodeStatus { get; set; }
        public float WmX { get; set; }
        public float WmY { get; set; }
        public float HtrsX { get; set; }
        public float HTRS_Y { get; set; }
    }
}
