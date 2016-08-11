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

        public static User GetByID(int UserID) {
            using (var db = new RestaurantReviewsEntities()) {
                return db.Users.Find(UserID);
            }
        }

        public static List<Review> GetReviews(int UserID, bool SortReverseByDate = true) {
            return Review.GetByUser(UserID, SortReverseByDate);
        }

        /// <summary>
        /// Validate the user object. This method could/should be much more robust, but for now we'll just ensure
        /// that the user requested by the ID is present. If so, we'll consider it valid. 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static bool IsValid(int UserID) {
            var user = GetByID(UserID);
            return user != null;
        }
    }
}
