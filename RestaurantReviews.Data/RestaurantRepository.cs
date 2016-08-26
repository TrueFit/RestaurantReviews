using System.Collections.Generic;
using System.Linq;
using RestaurantReviews.Core;
using System.Data.Entity.Spatial;

namespace RestaurantReviews.Data
{
    public class RestaurantRepository: GenericRepositoryEF<Restaurant>
    {
        //overriding the GetById. If you get a single restaurant, it is assumed that you will want to get all of the reviews with the restaurant at the same time
        public override Restaurant GetById(int entityId)
        {
            Restaurant entity;
            using (RRContext ctx = Context)
            {
                entity = ctx.Restaurants.Include("Reviews").Where(r => r.Id == entityId).FirstOrDefault();
            }
            return entity;
        }

        public IEnumerable<Restaurant> GetRestaurantsByCity(string city)
        {
            return base.Filter(r => r.City == city).ToList();
        }

        public IEnumerable<Restaurant> GetRestaurantsWithNameContains(string name)
        {
            return base.Filter(r => r.RestaurantName.Contains(name)).ToList();
        }

        public IEnumerable<Restaurant> GetRestaurantsAroundMe(int distance, double longitude, double latitude)
        {
            List<Restaurant> nearbyRestaurants;

            //distance input would be in Miles and SQL Server does distance based on meters so convert the distance to meters
            double distanceMeters = distance * 1609.344;

            DbGeography myLoc = DbGeography.FromText(string.Format("POINT({0} {1})", longitude.ToString(), latitude.ToString()));
            using (RRContext ctx = Context)
            {
                nearbyRestaurants = ctx.Restaurants.Where(r => r.GeoLocation.Distance(myLoc) <= distanceMeters).ToList();
            }
            return nearbyRestaurants;
        }

        public int? CreateRestaurantReview(RestaurantReview restReview)
        {
            using (RRContext ctx = Context)
            {
                ctx.RestaurantReviews.Add(restReview);
                ctx.SaveChanges();
                return restReview.Id;
            }
        }
    }
}
