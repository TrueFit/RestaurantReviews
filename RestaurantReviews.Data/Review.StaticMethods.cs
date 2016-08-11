using System.Linq;
using System.Collections.Generic;

namespace RestaurantReviews.Data {

    public partial class Review {

        /// <summary>
        /// Add a new review to the DB
        /// </summary>
        /// <param name="NewReview"></param>
        /// <returns>the newly-created Review</returns>
        public static Review AddReview(Review NewReview) {
            using (var db = new RestaurantReviewsEntities()) {
                db.Reviews.Add(NewReview);
                db.SaveChanges();
                return NewReview;
            }
        }

        /// <summary>
        /// Get a list of reviews created by a given user (UserID) from the DB
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SortReverseByDate"></param>
        /// <returns>a list of Review objects</returns>
        public static List<Review> GetByUser(int UserID, bool SortReverseByDate = true) {
            using (var db = new RestaurantReviewsEntities()) {
                var ReturnList = db.Reviews
                    .Where(r => r.UserID == UserID);

                if (SortReverseByDate) {
                    return ReturnList.OrderByDescending(r => r.CreatedDateTime).ToList();
                }
                else {
                    return ReturnList.OrderBy(r => r.CreatedDateTime).ToList();
                }
            }
        }

        /// <summary>
        /// Get a list of reviews for a given restaurant (RestaurantID) frim the DB
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="SortReverseByDate"></param>
        /// <returns>a list of Review objects</returns>
        public static List<Review> GetByRestaurant(int RestaurantID, bool SortReverseByDate = true) {
            return Restaurant.GetReviews(RestaurantID, SortReverseByDate);
        }

        /// <summary>
        /// Remove a review from the DB
        /// </summary>
        /// <param name="ReviewToRemove"></param>
        /// <returns>the removed Review object</returns>
        public static Review Remove(Review ReviewToRemove) {
            using (var db = new RestaurantReviewsEntities()) {
                db.Reviews.Remove(ReviewToRemove);
                db.SaveChanges();
                return ReviewToRemove;
            }
        }
    }
}
