using AutoFixture;
using RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate;
using RestaurantReviews.Infrastructure.Repositories;

namespace RestaurantReviews.Tests.Infrastructure
{
    [TestClass]
    public class RestaurantRepositoryTests : BaseInfrastructureTests
    {
        private RestaurantRepository _repo;
        private const string cityName = "Search City";

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            if (!_context.Restaurants.Any(x => x.City == cityName))
            {
                _context.Restaurants.AddRange(_fixture.Build<Restaurant>().With(x => x.City, cityName).CreateMany(3));
                _context.SaveChanges();
            }
            _repo = new RestaurantRepository(_context);
        }

        [TestMethod]
        public void GetById_ValidId_ReturnsExpectedRecord()
        {
            // Arrange
            // Act
            var result = _repo.GetById(1);

            // Assert
            Assert.AreEqual(_context.Restaurants.First(), result);
        }

        [TestMethod]
        public void GetById_InvalidId_ReturnsNull()
        {
            // Arrange
            // Act
            var result = _repo.GetById(_context.Restaurants.Count() + 1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Insert_AddsRecordToDb()
        {
            // Arrange
            var restaurant = _fixture.Create<Restaurant>();

            // Act
            var result = _repo.Insert(restaurant);

            // Assert
            var addedRecord = _context.Restaurants.Last();
            Assert.AreEqual(addedRecord.Id, result);
            Assert.AreEqual(addedRecord.Name, restaurant.Name);
            Assert.AreEqual(addedRecord.City, restaurant.City);
        }

        [TestMethod]
        public void Update_ValidId_DbRecordUpdated()
        {
            // Arrange
            var restaurant = _fixture.Create<Restaurant>();

            // Act
            var result = _repo.Update(1, restaurant);

            // Assert
            var updatedRecord = _context.Restaurants.First();
            Assert.AreEqual(updatedRecord.Id, result);
            Assert.AreEqual(updatedRecord.Name, restaurant.Name);
            Assert.AreEqual(updatedRecord.City, restaurant.City);
        }

        [TestMethod]
        public void Update_InvalidId_ThrowsException()
        {
            // Arrange
            var restaurant = _fixture.Create<Restaurant>();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _repo.Update(_context.Restaurants.Count() + 1, restaurant));
        }

        [TestMethod]
        public void Get_CitySearch_ReturnsCollectionOfRestaurants()
        {
            // Arrange
            // Act
            var results = _repo.Get(cityName);

            // Assert
            Assert.AreEqual(3, results.Count());
            foreach (var result in results)
                Assert.AreEqual(cityName, result.City);
        }
    }
}
