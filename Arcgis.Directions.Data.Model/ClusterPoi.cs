using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcgis.Directions.Data.Model
{
    public class ClusterPoi
    {
        public string image { get; set; }
        public string caption { get; set; }
        public string link { get; set; }
        public string full_name { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string mapIconColour { get; set; }
    }
}
