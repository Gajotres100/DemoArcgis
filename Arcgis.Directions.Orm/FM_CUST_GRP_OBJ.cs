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

    public partial class FM_CUST_GRP_OBJ
    {
        public int GRP_OBJ_ID { get; set; }
        public string GRP_NAME { get; set; }
        public decimal OBJ_SORT { get; set; }
        public string OBJ_NAME { get; set; }
        public Nullable<int> POI_ID { get; set; }
        public Nullable<int> GRP_ID { get; set; }
    
        public virtual FM_CUST_GRP FM_CUST_GRP { get; set; }
        public virtual FM_CUST_POI FM_CUST_POI { get; set; }
    }
}
