﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EntitiesPortalOracle : DbContext
    {
        public EntitiesPortalOracle()
            : base("name=EntitiesPortalOracle")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<V_ROUTING_POI_GROUPS> V_ROUTING_POI_GROUPS { get; set; }
        public DbSet<V_ROUTING_POI_PLACES> V_ROUTING_POI_PLACES { get; set; }
        public DbSet<V_ROUTING_POIS> V_ROUTING_POIS { get; set; }
        public DbSet<LANGUAGE> LANGUAGES { get; set; }
        public DbSet<POI_GROUP_ACCESS_RIGHTS> POI_GROUP_ACCESS_RIGHTS { get; set; }
        public DbSet<USER_ROUTE> USER_ROUTE { get; set; }
    }
}
