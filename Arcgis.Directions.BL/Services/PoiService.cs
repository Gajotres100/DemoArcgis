using Arcgis.Directions.Data.Repository.Poi;
using Arcgis.Directions.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcgis.Directions.BL.Services
{
    public class PoiService
    {
        #region Variables

        PoiRepository _poiRepository; 

        #endregion

        #region Constructors
        public PoiService()
        {
            _poiRepository = new PoiRepository();
        }
        #endregion
        public GetPOIVM GetAvailablePoi(string keyword)
        {
            GetPOIVM item = new GetPOIVM();
            
            return item;
        }

    }
}
