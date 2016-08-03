using System.Linq;

namespace RestaurantReviews.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Restaurant")]
    public partial class Restaurant
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Address1 { get; set; }

        [Required]
        [StringLength(100)]
        public string Address2 { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(5)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(11)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string WebsiteURL { get; set; }

        /// <summary>
        /// Get a list of restaurants by current city
        /// </summary>
        /// <param name="City"></param>
        /// <returns></returns>
        public static List<Restaurant> GetByCity(string City) {
            using (var db = new RestaurantReviewsEntities()) {
                var ReturnList = db.Restaurants
                    .Where(r => r.City.IndexOf(City, StringComparison.OrdinalIgnoreCase) >= 0)
                    .OrderBy(r => r.Name);

                return ReturnList.ToList();
            }
        }

        /// <summary>
        /// Get a list of restaurants by current city
        /// </summary>
        /// <param name="City"></param>
        /// <returns></returns>
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
    }
}
