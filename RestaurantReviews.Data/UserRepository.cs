using RestaurantReviews.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantReviews.Data
{
    public class UserRepository : GenericRepositoryEF<User>
    {
        public IEnumerable<RestaurantReview> GetUserReviews(int userId)
        {
            List<RestaurantReview> userReviews = null;
            using (RRContext ctx = Context)
            {
                userReviews = ctx.RestaurantReviews.Where(r => r.CreatorId == userId).OrderByDescending(d => d.Created).ToList();
            }
            return userReviews;
        }

        //placed this here becuase of the assumed restriction that a user can only delete their own reviews
        public void DeleteUserReview(int reviewId)
        {
            using (RRContext ctx = Context)
            {
                RestaurantReview restReview = ctx.RestaurantReviews.Find(reviewId);
                if (!(restReview == null))
                {
                    ctx.RestaurantReviews.Remove(restReview);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Review with suppled ID does not exist");
                }
            }
        }

        //override the base delete entity function. Do not want any users to be deleted from the system.
        public override void DeleteEntity(User entity)
        {
            throw new ArgumentException("Users cannot be deleted from the system");
        }
    }
}
