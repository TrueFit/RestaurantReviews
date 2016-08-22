using System.Linq;

namespace RestaurantReviews.Data {
    using System.Collections.Generic;
    
    public partial class User {
        /// <summary>
        /// Add a new user to the DB
        /// </summary>
        /// <param name="NewUser"></param>
        /// <returns>the newly-created User object</returns>
        public static User AddUser(User NewUser) {
            using (var db = new RestaurantReviewsEntities()) {
                db.Users.Add(NewUser);
                db.SaveChanges();
                return NewUser;
            }
        }

        /// <summary>
        /// Get a user from the DB
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>the found user or null if not found</returns>
        public static User GetByID(int UserID) {
            using (var db = new RestaurantReviewsEntities()) {
                return db.Users.Find(UserID);
            }
        }

        /// <summary>
        /// Get all users from the DB
        /// </summary>
        /// <returns>a list of User objects</returns>
        public static List<User> GetAll() {
            using (var db = new RestaurantReviewsEntities()) {
                return db.Users.OrderBy(u => u.Username).ToList();
            }
        }

        /// <summary>
        /// Get alist of reviews for a given user (UserID) from the DB
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SortReverseByDate"></param>
        /// <returns></returns>
        public static List<Review> GetReviews(int UserID, bool SortReverseByDate = true) {
            return Review.GetByUser(UserID, SortReverseByDate);
        }

        /// <summary>
        /// Verify that the user (UserID) is valid.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>true if the user is valid, false otherwise</returns>
        public static bool IsValid(int UserID) {
            var user = GetByID(UserID);
            return user != null;
        }
    }
}
