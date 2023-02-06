using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Gets User record by identity
        /// </summary>
        /// <param name="id">Identity of User</param>
        /// <returns></returns>
        User GetById(int id);

        /// <summary>
        /// Inserts User record
        /// </summary>
        /// <param name="user">User to be inserted</param>
        /// <returns>Created identity of inserted record</returns>
        int Insert(User user);

        /// <summary>
        /// Updates User record
        /// </summary>
        /// <param name="id">Identity of the User to be updated</param>
        /// <param name="user">Updated User attributes</param>
        /// <returns>Identity of updated record</returns>
        int Update(int id, User user);
    }
}
