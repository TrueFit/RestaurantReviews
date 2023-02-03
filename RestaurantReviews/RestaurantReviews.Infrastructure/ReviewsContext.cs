using Microsoft.EntityFrameworkCore;
using RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate;
using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;
using RestaurantReviews.Domain.AggregatesModel.UserAggregate;

namespace RestaurantReviews.Infrastructure
{
    public class ReviewsContext : DbContext
    {
        public ReviewsContext(DbContextOptions<ReviewsContext> options) : base(options) { }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
