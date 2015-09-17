using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IReviewsDAL
    {
        int InsertRestarantReview(ReviewDataContainer rdc);

        int InsertReviewForNewRestaurant(ReviewDataContainer rdc);

        IList<ReviewDataContainer> GetAllReviewsByCity(string cityName);

        IList<ReviewDataContainer> GetAllReviewsByRestaurant(string restaurantName);

        IList<ReviewDataContainer> GetAllReviewsByUser(string userName);

        int DeleteReview(int reviewID, string userName);

        ReviewDataContainer GetReviewByID(int reviewID);
    }
}
