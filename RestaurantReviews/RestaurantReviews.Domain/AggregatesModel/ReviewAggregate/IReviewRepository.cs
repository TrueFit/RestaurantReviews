using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.ReviewAggregate
{
    public interface IReviewRepository : IRepository<Review>
    {
        /// <summary>
        /// Gets Review record by identity
        /// </summary>
        /// <param name="id">Identity of Review</param>
        /// <returns></returns>
        Review GetById(int id);

        /// <summary>
        /// Inserts Review record
        /// </summary>
        /// <param name="review">Review to be inserted</param>
        /// <returns>Identity of created record</returns>
        int Insert(Review review);

        /// <summary>
        /// Updates Review record
        /// </summary>
        /// <param name="id">Identity of Review to be updated</param>
        /// <param name="review">Updated Review attributes</param>
        /// <returns>Identity of updated record</returns>
        int Update(int id, Review review);

        /// <summary>
        /// Deletes Review record
        /// </summary>
        /// <param name="id">Identity of Review to be deleted</param>
        void Delete(int id);

        /// <summary>
        /// Seraches for matching Review records
        /// </summary>
        /// <param name="restaurantId">Identity of Restaurant to search by</param>
        /// <param name="userId">Identity of User to search by</param>
        /// <returns>Collection of reviews that match the search criteria</returns>
        IEnumerable<Review> Get(int? restaurantId = null, int? userId = null);
    }
}
