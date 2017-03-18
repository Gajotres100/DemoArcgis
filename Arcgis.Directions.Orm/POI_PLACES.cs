//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Arcgis.Directions.Orm
{
    using System;
    using System.Collections.Generic;
    
    public partial class POI_PLACES
    {
        public int POI_PLACE_ID { get; set; }
        public int USER_ID { get; set; }
        public Nullable<int> CHILD_USER_ID { get; set; }
        public Nullable<short> SERVICE_ID { get; set; }
        public int POI_GROUP_ID { get; set; }
        public string POI_PLACE_NAME { get; set; }
        public short POI_PLACE_TYPE { get; set; }
        public string POI_PLACE_CODE { get; set; }
        public decimal WM_X { get; set; }
        public decimal WM_Y { get; set; }
        public string ADDRESS { get; set; }
        public string HOUSE_NUMBER { get; set; }
        public string ZIP { get; set; }
        public string CITY { get; set; }
        public Nullable<int> JURISDICTION_ZONE { get; set; }
        public string DESCRIPTION { get; set; }
        public string REMARK { get; set; }
        public string MAP_ICON_NAME { get; set; }
        public string MAP_ICON_COLOR { get; set; }
        public System.DateTime CREATED_DT { get; set; }
        public Nullable<System.DateTime> MODIFIED_DT { get; set; }
        public Nullable<int> MODIFIED_BY { get; set; }
    
        public virtual POI_GROUPS POI_GROUPS { get; set; }

        public bool In(List<int> poiGroup)
        {
            throw new NotImplementedException();
        }
    }
}
