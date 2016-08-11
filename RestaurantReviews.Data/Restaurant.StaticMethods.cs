using System.Linq;
using System;
using System.Collections.Generic;

namespace RestaurantReviews.Data {

    public partial class Restaurant {

        /// <summary>
        /// Get a restaurant by ID from the DB
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns>a Restaurant object or null if not found</returns>
        public static Restaurant GetByID(int RestaurantID) {
            using (var db = new RestaurantReviewsEntities()) {
                return db.Restaurants.Find(RestaurantID);
            }
        }

        /// <summary>
        /// Get a list of restaurants by city from the DB
        /// </summary>
        /// <param name="City"></param>
        /// <returns>a list of Restaurants objects</returns>
        public static List<Restaurant> GetByCity(string City) {
            using (var db = new RestaurantReviewsEntities()) {
                var ReturnList = db.Restaurants
                    .Where(r => String.Compare(r.City, City, StringComparison.OrdinalIgnoreCase) == 0)
                    .OrderBy(r => r.Name);

                return ReturnList.ToList();
            }
        }

        /// <summary>
        /// Get a list of restaurants by zip code from the DB
        /// </summary>
        /// <param name="Zip"></param>
        /// <returns>a list of Restaurant objects</returns>
        public static List<Restaurant> GetByZipCode(string Zip) {
            using (var db = new RestaurantReviewsEntities()) {
                var ReturnList = db.Restaurants
                    .Where(r => String.Compare(r.ZipCode, Zip, StringComparison.OrdinalIgnoreCase) == 0)
                    .OrderBy(r => r.Name);

                return ReturnList.ToList();
            }
        }

        /// <summary>
        /// Add a new restaurant to the DB
        /// </summary>
        /// <param name="NewRestaurant"></param>
        /// <returns>the newly-created Restaurant</returns>
        public static Restaurant AddRestaurant(Restaurant NewRestaurant) {
            using (var db = new RestaurantReviewsEntities()) {
                db.Restaurants.Add(NewRestaurant);
                db.SaveChanges();
                return NewRestaurant;
            }
        }

        /// <summary>
        /// Get a list of reviews for a restaurant
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="SortReverseByDate"></param>
        /// <returns>a list of Restaurant objects</returns>
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

        /// <summary>
        /// Get a list of users who left a review for the given restauranr (RestaurantID)
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns>a list of User objects</returns>
        public static List<User> GetUsers(int RestaurantID) {
            var UserIDs = Restaurant.GetReviews(RestaurantID)
                .Select(r => r.UserID)
                .Distinct().ToArray();

            using (var db = new RestaurantReviewsEntities()) {
                var ReturnList = db.Users.Where(r => UserIDs.Contains(r.Id)).ToList();
                return ReturnList;
            }
        }

        /// <summary>
        /// Validate the restuarant. This method could/should be much more robust, but for now we'll just ensure
        /// that the restaurant requested by the ID is present. If so, we'll consider it valid.
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public static bool IsValid(int RestaurantID) {
            var restaurant = GetByID(RestaurantID);
            return restaurant != null;
        }
    }
}
