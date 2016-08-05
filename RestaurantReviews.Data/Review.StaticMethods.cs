using System.Linq;
using System.Collections.Generic;

namespace RestaurantReviews.Data {

    public partial class Review {

        public static Review AddReview(Review NewReview) {
            using (var db = new RestaurantReviewsEntities()) {
                db.Reviews.Add(NewReview);
                db.SaveChanges();
                return NewReview;
            }
        }

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

        public static List<Review> GetByRestaurant(int RestaurantID, bool SortReverseByDate = true) {
            return Restaurant.GetReviews(RestaurantID, SortReverseByDate);
        }

        public static Review Remove(Review ReviewToRemove) {
            using (var db = new RestaurantReviewsEntities()) {
                db.Reviews.Remove(ReviewToRemove);
                db.SaveChanges();
                return ReviewToRemove;
            }
        }
    }
}
