using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        User GetById(int id);
        int Insert(User user);
        int Update(int id, User user);
    }
}
