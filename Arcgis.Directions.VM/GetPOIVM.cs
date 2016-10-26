using Arcgis.Directions.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcgis.Directions.VM
{
    public class GetPOIVM : ViewModelBase
    {
        public List<CusPoi> CusPoiList { get; set; }
        public CusPoi CusPoi { get; set; }
        public CusPoiInput CusPoiInput { get; set; }
    }
}
