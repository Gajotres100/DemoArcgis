using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcgis.Directions.Data;

namespace Arcgis.Directions.Data.Repository.Poi
{
    public class PoiRepository
    {
        public PoiRepository()
        {

        }
        public void GetAvailablePoi()
        {
            using (var context = new DbContext())
            //using (var context = new BloggingContext())
            //{
            //    // Query for all blogs with names starting with B 
            //    var blogs = from b in context.Blogs
            //                where b.Name.StartsWith("B")
            //                select b;

            //    // Query for the Blog named ADO.NET Blog 
            //    var blog = context.Blogs
            //                    .Where(b => b.Name == "ADO.NET Blog")
            //                    .FirstOrDefault();
            //}
        }
    }
}
