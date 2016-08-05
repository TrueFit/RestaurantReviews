using System.Linq;
using System;
using System.Collections.Generic;

namespace RestaurantReviews.Data {

    public partial class Restaurant {

        public static Restaurant GetByID(int RestaurantID) {
            using (var db = new RestaurantReviewsEntities()) {
                return db.Restaurants.Find(RestaurantID);
            }
        }

        public static List<Restaurant> GetByCity(string City) {
            using (var db = new RestaurantReviewsEntities()) {
                var ReturnList = db.Restaurants
                    .Where(r => r.City.IndexOf(City, StringComparison.OrdinalIgnoreCase) >= 0)
                    .OrderBy(r => r.Name);

                return ReturnList.ToList();
            }
        }

        public static List<Restaurant> GetByZipCode(string Zip) {
            using (var db = new RestaurantReviewsEntities()) {
                var ReturnList = db.Restaurants
                    .Where(r => r.City.IndexOf(Zip, StringComparison.OrdinalIgnoreCase) >= 0)
                    .OrderBy(r => r.Name);

                return ReturnList.ToList();
            }
        }

        public static Restaurant AddRestaurant(Restaurant NewRestaurant) {
            using (var db = new RestaurantReviewsEntities()) {
                db.Restaurants.Add(NewRestaurant);
                db.SaveChanges();
                return NewRestaurant;
            }
        }

        public static List<Review> GetReviews(int RestaurantID, bool SortReverseByDate = true) {
            using (var db = new RestaurantReviewsEntities()) {
                var Reviews = db.Reviews
                    .Where(r => r.RestaurantID == RestaurantID);

                if (SortReverseByDate) {
                    return Reviews.OrderByDescending(r => r.CreatedDateTime).ToList();
                }
                else {
                    return Reviews.OrderBy(r => r.CreatedDateTime).ToList();
                }
            }
        }

        public static List<User> GetUsers(int RestaurantID) {
            var UserIDs = Restaurant.GetReviews(RestaurantID)
                .Select(r => r.UserID)
                .Distinct().ToArray();

            using (var db = new RestaurantReviewsEntities()) {
                var ReturnList = db.Users.Where(r => UserIDs.Contains(r.Id)).ToList();
                return ReturnList;
            }
        }
    }
}
