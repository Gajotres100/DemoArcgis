using Arcgis.Directions.Data.Repository.Poi;
using Arcgis.Directions.VM;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Arcgis.Directions.BL.Services
{
    public class PoiService
    {
        #region Variables

        PoiRepository _poiRepository;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);       

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
            try
            {
                throw new Exception("Gurtnaš");
                GetPOIVM item = new GetPOIVM();
                item.CusPoiList = _poiRepository.GetAvailablePoiByDescription(keyword);
                return item;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return null;
            }

        }

        public GetPOIVM GetPoiByID(int id)
        {
            try
            {
                GetPOIVM item = new GetPOIVM();
                item.CusPoi = _poiRepository.GetPoiByID(id);
                return item;
            }
            catch (Exception e)
            {
                logger.Error("error = " + e);
                return null;
            }
        }

        #endregion

    }
}
