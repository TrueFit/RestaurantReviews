namespace RestaurantReviews.Data {
    using System.Collections.Generic;
    
    public partial class User {
        public static User AddUser(User NewUser) {
            using (var db = new RestaurantReviewsEntities()) {
                db.Users.Add(NewUser);
                db.SaveChanges();
                return NewUser;
            }
        }

        public static List<Review> GetReviews(int UserID, bool SortReverseByDate = true) {
            return Review.GetByUser(UserID, SortReverseByDate);
        }
    }
}
