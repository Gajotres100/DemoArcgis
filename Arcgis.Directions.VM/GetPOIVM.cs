using Arcgis.Directions.Data.Model;
using System.Collections.Generic;

namespace Arcgis.Directions.VM
{
    public class GetPOIVM : ViewModelBase
    {
        public List<CusPoi> CusPoiList { get; set; }
        public CusPoi CusPoi { get; set; }
        public CusPoiInput CusPoiInput { get; set; }
        public List<Application> Applications { get; set; }

        public int PoiID { get; set; }
        public string PoiName { get; set; }
        public string PoiDesc { get; set; }
        public List<ClusterPoi> ClusterPoiList { get; set; }
        public List<GroupPoi> GroupPoiList { get; set; }
        public object DirectionsRoute { get; set; }
        public List<RouteData> RouteDataList { get; set; }
        public RouteData RouteData { get; set; }

    }
}