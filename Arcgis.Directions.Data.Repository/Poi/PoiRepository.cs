using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcgis.Directions.Data;
using Arcgis.Directions.Orm;
using Arcgis.Directions.Data.Model;

namespace Arcgis.Directions.Data.Repository.Poi
{
    public class PoiRepository
    {
        public PoiRepository()
        {

        }
        public List<CusPoi> GetAvailablePoiByDescription(string keyword)
        {
            using (var context = new ProtalEntities())
            {
                    return context.GetAvailablePoiByDescription(keyword).Select(x => new CusPoi()
                    {
                        PoiID = x.POI_ID,
                        Address = x.ADDRESS,
                        GeocodeStatus = x.GEOCODE_STATUS,
                        HouseNr = x.HOUSE_NR,
                        HtrsX = x.HTRS_X,
                        HTRS_Y = x.HTRS_Y,
                        PoiDesc = x.POI_DESC,
                        PoiName = x.POI_NAME,
                        SettleName = x.SETTL_NAME,
                        WmX = x.WM_X,
                        WmY = x.WM_Y
                    }).ToList();
            }            
        }

        public CusPoi GetPoiByID(int id)
        {
            using (var context = new ProtalEntities())
            {
                return context.GetPoiByID(id).Select(x => new CusPoi()
                {
                    PoiID = x.POI_ID,
                    Address = x.ADDRESS,
                    GeocodeStatus = x.GEOCODE_STATUS,
                    HouseNr = x.HOUSE_NR,
                    HtrsX = x.HTRS_X,
                    HTRS_Y = x.HTRS_Y,
                    PoiDesc = x.POI_DESC,
                    PoiName = x.POI_NAME,
                    SettleName = x.SETTL_NAME,
                    WmX = x.WM_X,
                    WmY = x.WM_Y
                }).FirstOrDefault();
            }
        }
    }
}
