using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcgis.Directions.Data.Model
{
    public class GroupPoi
    {
        public int PoiGroupID { get; set; }
        public int UserID { get; set; }
        public int ChildUserID { get; set; }
        public short ServiceID { get; set; }
        public string PoiGroupName { get; set; }
        public int PoiGroupType { get; set; }
        public bool Selected { get; set; }
    }
}
