using AutoFixture;
using Microsoft.EntityFrameworkCore;
using RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate;
using RestaurantReviews.Domain.AggregatesModel.UserAggregate;
using RestaurantReviews.Infrastructure;

namespace RestaurantReviews.Tests.Infrastructure
{
    [TestClass]
    public abstract class BaseInfrastructureTests
    {
        protected IFixture _fixture;
        protected ReviewsContext _context;

        [TestInitialize]
        public virtual void Initialize()
        {
            _fixture = new Fixture();

            var optionsBuilder = new DbContextOptionsBuilder<ReviewsContext>()
                .UseInMemoryDatabase(databaseName: "Reviews");
            _context = new ReviewsContext(optionsBuilder.Options);

            if(_context.Users.Count() == 0)
                _context.Users.AddRange(_fixture.CreateMany<User>(5));
            if(_context.Restaurants.Count() == 0)
                _context.Restaurants.AddRange(_fixture.CreateMany<Restaurant>(5));
            _context.SaveChanges();
        }
    }
}
