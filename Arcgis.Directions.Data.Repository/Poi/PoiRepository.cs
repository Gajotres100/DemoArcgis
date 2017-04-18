using System.Collections.Generic;
using System.Linq;
using Arcgis.Directions.Orm;
using Arcgis.Directions.Data.Model;
using System.Text;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
                return context.POI_PLACES.Where(p => (p.ADDRESS.ToLower().Contains(keyword.ToLower()) || p.DESCRIPTION.ToLower().Contains(keyword.ToLower()) || p.POI_PLACE_NAME.ToLower().Contains(keyword.ToLower())) && p.USER_ID == userID).Select(x => new CusPoi
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

        public List<ClusterPoi> GetAllPoisByGroupID(List<int> poiGroup)
        {
            using (var context = new EntitiesPortalOracle())
            {
                return context.POI_PLACES.Where(p => poiGroup.Contains(p.POI_GROUP_ID)).AsEnumerable().Select(x => new ClusterPoi
                {
                    lat = $"{x.WM_X}",
                    lng = $"{x.WM_Y}",
                    full_name = x.DESCRIPTION,
                    caption = x.ADDRESS,
                    image = "https://cdn.geek.hr/wp-content/uploads/sites/10/store/mali-zec-slika.jpg",
                    link = "https://cdn.geek.hr/wp-content/uploads/sites/10/store/mali-zec-slika.jpg"
                }).ToList();
            }
        }

        public List<GroupPoi> GetPoiGroups(int userID)
        {
            using (var context = new EntitiesPortalOracle())
            {
                return context.POI_GROUPS.Where(g => g.USER_ID == userID).AsEnumerable().Select(x => new GroupPoi
                {
                    ChildUserID = x.CHILD_USER_ID.GetValueOrDefault(),
                    PoiGroupID = x.POI_GROUP_ID,
                    PoiGroupName = x.POI_GROUP_NAME,
                    PoiGroupType = x.POI_GROUP_TYPE,
                    ServiceID = x.SERVICE_ID.GetValueOrDefault(),
                    UserID = x.USER_ID
                }).ToList();
            }
        }

        public List<GroupPoi> GetPoiGroupsFromView(int userID)
        {
            using (var context = new EntitiesPortalOracle())
            {
                return context.V_ROUTING_POIS.Where(g => g.USER_ID == userID).OrderBy(g => g.POI_GROUP_ID).AsEnumerable().Select(x => new GroupPoi
                {                    
                    PoiGroupID = x.POI_GROUP_ID,
                    PoiGroupName = x.POI_GROUP_NAME,
                    PoiGroupType = x.POI_GROUP_TYPE,
                    ServiceID = x.SERVICE_ID.GetValueOrDefault(),
                    UserID = x.USER_ID,
                    PoiCount = Convert.ToInt32(x.POI_COUNT)
                }).ToList();
            }
        }

        

        public int SaveRoute(int userID, string routeData, string routeName, bool optimalRoute, bool returnToStar)
        {
            using (var context = new EntitiesPortalOracle())
            {
                USER_ROUTE userRouter = new USER_ROUTE
                {
                    NAME = routeName,
                    ROUTE = Encoding.UTF8.GetBytes(routeData),
                    USER_ID = userID,
                    OPTIMALROUTE = optimalRoute ? 1 : 0,
                    RETURNTOSTART = returnToStar ? 1 : 0
                };                

                context.USER_ROUTE.Add(userRouter);
                context.SaveChanges();
                return userRouter.ID;
            }
        }

        public List<RouteData> GetRoutesForUser(int userID)
        {
            using (var context = new EntitiesPortalOracle())
            {
                return context.USER_ROUTE.Where(r => r.USER_ID == userID).OrderBy(r => r.ID).AsEnumerable().Select(x => new RouteData
                {
                    ID = x.ID,
                    Name = x.NAME,
                    UserID = x.USER_ID,
                    CreationDate = x.CREATED_DT
                    //Route = JObject.Parse(Encoding.ASCII.GetString(x.ROUTE))
                }).ToList();
            }            
        }

        public void DeleteRoute(int routeID)
        {
            using (var context = new EntitiesPortalOracle())
            {
                var userRoute = context.USER_ROUTE.FirstOrDefault(r => r.ID == routeID);

                context.USER_ROUTE.Remove(userRoute);
                context.SaveChanges();
            }
        }

        public RouteData GetRoutesByRouteId(int ID)
        {
            using (var context = new EntitiesPortalOracle())
            {
                var a = context.USER_ROUTE.FirstOrDefault(r => r.ID == ID);
                RouteData routeData = new RouteData();
                string stringroute = Encoding.UTF8.GetString(a.ROUTE);
                routeData.Route = JsonConvert.DeserializeObject(stringroute);


                routeData.ID = a.ID;
                routeData.Name = a.NAME;
                routeData.UserID = a.USER_ID;
                routeData.OptimalRoute = a.OPTIMALROUTE == 1;
                routeData.ReturnToStart = a.RETURNTOSTART == 1;
                return routeData; 
            }
        }

        
    }
}
