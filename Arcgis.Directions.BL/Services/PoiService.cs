using Arcgis.Directions.Data.Repository.Poi;
using Arcgis.Directions.VM;
using log4net;
using System;
using System.Reflection;

namespace Arcgis.Directions.BL.Services
{
    public class PoiService
    {
        readonly
        #region Variables

        PoiRepository _poiRepository;
        static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors
        public PoiService()
        {
            _poiRepository = new PoiRepository();
        }
        #endregion

        #region Methods

        public GetPOIVM GetAvailablePoiByDescription(string keyword, int userID)
        {
            try
            {
                var item = new GetPOIVM();
                item.CusPoiList = _poiRepository.GetAvailablePoiByDescriptionOracle(keyword, userID);
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
                var item = new GetPOIVM();
                item.CusPoi = _poiRepository.GetPoiByIDOracle(id);
                return item;
            }
            catch (Exception e)
            {
                logger.Error("error = " + e);
                return null;
            }
        }

        public GetPOIVM GetLanguages()
        {
            try
            {
                var item = new GetPOIVM();
                item.LanguageList = _poiRepository.GetLanguagesOracle();
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
