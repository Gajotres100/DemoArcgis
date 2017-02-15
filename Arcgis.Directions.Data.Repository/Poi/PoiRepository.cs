using System.Collections.Generic;
using System.Linq;
using Arcgis.Directions.Orm;
using Arcgis.Directions.Data.Model;

namespace Arcgis.Directions.Data.Repository.Poi
{
    public class PoiRepository
    {
        public List<CusPoi> GetAvailablePoiByDescription(string keyword)
        {
            using (var context = new ProtalEntities())
            {
                    return context.GetAvailablePoiByDescription(keyword).Select(x => new CusPoi
                    {
                        PoiID = x.POI_ID,
                        Address = x.ADDRESS,
                        GeocodeStatus = x.GEOCODE_STATUS,
                        HouseNr = x.HOUSE_NR,
                        HtrsX = x.HTRS_X,
                        HTRS_Y = x.HTRS_Y,
                        PoiDesc = x.POI_DESC,
                        PoiName = x.POI_NAME,
                        SettleName = x.SETTL_NAME
                    }).ToList();
            }            
        }

        public CusPoi GetPoiByID(int id)
        {
            using (var context = new ProtalEntities())
            {
                return context.GetPoiByID(id).Select(x => new CusPoi
                {
                    PoiID = x.POI_ID,
                    Address = x.ADDRESS,
                    GeocodeStatus = x.GEOCODE_STATUS,
                    HouseNr = x.HOUSE_NR,
                    HtrsX = x.HTRS_X,
                    HTRS_Y = x.HTRS_Y,
                    PoiDesc = x.POI_DESC,
                    PoiName = x.POI_NAME,
                    SettleName = x.SETTL_NAME
                }).FirstOrDefault();
            }
        }

        public CusPoi GetPoiByIDOracle(int id)
        {
            using (var context = new EntitiesPortalOracle())
            {
                return context.POI_PLACES.Where(p => p.POI_PLACE_ID == id).Select(x => new CusPoi
                {
                    PoiID = x.POI_PLACE_ID,
                    Address = x.ADDRESS,
                    PoiDesc = x.DESCRIPTION,
                    PoiName = x.POI_PLACE_NAME,
                    WmX = x.WM_X,
                    WmY = x.WM_Y,
                    Zip = x.ZIP,
                    City = x.CITY,
                    HouseNoumber = x.HOUSE_NUMBER
                }).FirstOrDefault();
            }
        }

        public List<CusPoi> GetAvailablePoiByDescriptionOracle(string keyword, int userID)
        {
            using (var context = new EntitiesPortalOracle())
            {
                return context.POI_PLACES.Where(p => (p.ADDRESS.Contains(keyword) || p.DESCRIPTION.Contains(keyword) || p.POI_PLACE_NAME.Contains(keyword)) && p.USER_ID == userID).Select(x => new CusPoi
                {
                    PoiID = x.POI_PLACE_ID,
                    Address = x.ADDRESS,
                    PoiDesc = x.DESCRIPTION,
                    PoiName = x.POI_PLACE_NAME,
                    WmX = x.WM_X,
                    WmY = x.WM_Y,
                    Zip = x.ZIP,
                    City = x.CITY,
                    HouseNoumber = x.HOUSE_NUMBER,
                    UserID = x.USER_ID
                }).ToList();
            }
        }

        public List<Language> GetLanguages()
        {
            using (var context = new ProtalEntities())
            {
                return context.GetLanguages().Select(x => new Language
                {
                    LangID = x.LANG_ID,
                    Name = x.NAME,
                    Code = x.CODE
                }).ToList();
            }
        }

        public List<Language> GetLanguagesOracle()
        {
            using (var context = new EntitiesPortalOracle())
            {
                return context.LANGUAGES.Select(x => new Language
                {
                    LangID = x.LANG_ID,
                    Name = x.NAME,
                    Code = x.CODE                    
                }).ToList();
            }
        }
    }
}
