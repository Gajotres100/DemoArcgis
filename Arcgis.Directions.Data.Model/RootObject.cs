using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcgis.Directions.Data.Model
{
    public class SpatialReference
    {
        public int wkid { get; set; }
        public int latestWkid { get; set; }
    }

    public class Extent
    {
        public string type { get; set; }
        public double xmin { get; set; }
        public double ymin { get; set; }
        public double xmax { get; set; }
        public double ymax { get; set; }
        public SpatialReference spatialReference { get; set; }
    }

    public class SpatialReference2
    {
        public int wkid { get; set; }
        public int latestWkid { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public SpatialReference2 spatialReference { get; set; }
    }

    public class Attributes
    {
        public int ObjectID { get; set; }
        public string Name { get; set; }
        public object RouteName { get; set; }
        public int Sequence { get; set; }
        public object TimeWindowStart { get; set; }
        public object TimeWindowEnd { get; set; }
        public int? ArriveCurbApproach { get; set; }
        public int? DepartCurbApproach { get; set; }
        public int SourceID { get; set; }
        public int SourceOID { get; set; }
        public double PosAlong { get; set; }
        public int SideOfEdge { get; set; }
        public int CurbApproach { get; set; }
        public int Status { get; set; }
        public int Attr_Minutes { get; set; }
        public int Attr_Kilometers { get; set; }
        public int Attr_Miles { get; set; }
        public double Cumul_Minutes { get; set; }
    }

    public class Feature
    {
        public Geometry geometry { get; set; }
        public object symbol { get; set; }
        public Attributes attributes { get; set; }
        public object infoTemplate { get; set; }
    }

    public class RootObject
    {
        public Extent extent { get; set; }
        public Feature feature { get; set; }
        public string name { get; set; }
    }
}
