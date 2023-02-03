using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;

namespace RestaurantReviews.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ReviewsContext _context;

        public ReviewRepository(ReviewsContext context) { _context = context; }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null)
                throw new ArgumentOutOfRangeException("Not a valid review identity.");

            existing.Delete();
            _context.SaveChanges();
        }

        public IEnumerable<Review> Get(int? restaurantId = null, int? userId = null)
        {
            var query = _context.Reviews.Where(r => r.Active);
            if (restaurantId != null) query = query.Where(r => r.RestaurantId == restaurantId);
            if (userId != null) query = query.Where(r => r.UserId == userId);
            return query.ToList();
        }

        public Review GetById(int id)
        {
            return _context.Reviews.FirstOrDefault(r => r.Id == id);
        }

        public int Insert(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return review.Id;
        }

        public int Update(int id, Review review)
        {
            var existing = GetById(id);
            if (existing == null)
                throw new ArgumentOutOfRangeException("Not a valid review identity.");

            existing.Update(review);
            _context.SaveChanges();
            return existing.Id;
        }
    }
}
