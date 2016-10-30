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

        #region Metods

        public GetPOIVM GetAvailablePoiByDescription(string keyword)
        {
            GetPOIVM item = new GetPOIVM();
            item.CusPoiList = _poiRepository.GetAvailablePoiByDescription(keyword);
            return item;
        }

        public GetPOIVM GetPoiByID(int id)
        {
            GetPOIVM item = new GetPOIVM();
            item.CusPoi = _poiRepository.GetPoiByID(id);
            return item;
        }

        #endregion

    }
}
