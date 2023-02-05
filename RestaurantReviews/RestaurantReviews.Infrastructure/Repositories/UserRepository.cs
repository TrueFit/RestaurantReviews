using RestaurantReviews.Domain.AggregatesModel.UserAggregate;

namespace RestaurantReviews.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ReviewsContext _context;

        public UserRepository(ReviewsContext context) { _context = context; }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public int Insert(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }

        public int Update(int id, User user)
        {
            var existing = GetById(id);
            if (existing is null)
                throw new ArgumentOutOfRangeException("Not a valid user identity.");

            existing.Update(user);
            _context.SaveChanges();
            return existing.Id;
        }
    }
}
