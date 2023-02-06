using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        /// <summary>
        /// Gets Restaurant record by identity
        /// </summary>
        /// <param name="id">Identity of Restaurant</param>
        /// <returns></returns>
        Restaurant GetById(int id);

        /// <summary>
        /// Inserts Restaurant record
        /// </summary>
        /// <param name="restaurant">Restaurant to be inserted</param>
        /// <returns>Created identity of inserted record</returns>
        int Insert(Restaurant restaurant);

        /// <summary>
        /// Updates Restaurant record
        /// </summary>
        /// <param name="id">Identity of the Restaurant to be updated</param>
        /// <param name="restaurant">Updated Restaurant attributes</param>
        /// <returns>Identity of updated record</returns>
        int Update(int id, Restaurant restaurant);

        /// <summary>
        /// Searches for matching Restaurant records
        /// </summary>
        /// <param name="city">City to match</param>
        /// <returns>Collection of Restaurants that match search criteria</returns>
        IEnumerable<Restaurant> Get(string city);
    }
}
