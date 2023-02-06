using RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate;

namespace RestaurantReviews.Infrastructure.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ReviewsContext _context;

        public RestaurantRepository(ReviewsContext context) { _context = context; }

        public IEnumerable<Restaurant> Get(string city)
        {
            var query = _context.Restaurants.AsQueryable();
            if(!string.IsNullOrWhiteSpace(city)) query = query.Where(r => r.City == city);
            return query.ToList();
        }

        public Restaurant GetById(int id)
        {
            return _context.Restaurants.FirstOrDefault(r => r.Id == id);
        }

        public int Insert(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();
            return restaurant.Id;
        }

        public int Update(int id, Restaurant restaurant)
        {
            var existing = GetById(id);
            if (existing is null)
                throw new ArgumentOutOfRangeException("Not a valid restaurant identity.");

            existing.Update(restaurant);
            _context.SaveChanges();
            return existing.Id;
        }
    }
}
