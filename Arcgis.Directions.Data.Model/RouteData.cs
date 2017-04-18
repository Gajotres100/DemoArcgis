using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcgis.Directions.Data.Model
{
    public class RouteData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }
        public dynamic Route { get; set; }
        public bool OptimalRoute { get; set; }
        public bool ReturnToStart { get; set; }
        public DateTime? CreationDate { get; set; }

    }
}
