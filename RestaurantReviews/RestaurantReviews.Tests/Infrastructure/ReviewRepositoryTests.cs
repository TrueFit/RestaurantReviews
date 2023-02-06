using AutoFixture;
using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;
using RestaurantReviews.Infrastructure.Repositories;

namespace RestaurantReviews.Tests.Infrastructure
{
    [TestClass]
    public class ReviewRepositoryTests : BaseInfrastructureTests
    {
        private ReviewRepository _repo;
        [TestInitialize]
        public void Initialize()
        {
            base.Initialize();
            if(_context.Reviews.Count() == 0)
            {
                _context.Reviews.AddRange(
                    _fixture.Build<Review>().With(x => x.RestaurantId, 1).With(x => x.UserId, 1).With(x => x.Active, true)
                    .CreateMany(4));
                _context.Reviews.AddRange(
                    _fixture.Build<Review>().With(x => x.RestaurantId, 2).With(x => x.UserId, 2).With(x => x.Active, true)
                    .CreateMany(3));
                _context.Reviews.AddRange(
                    _fixture.Build<Review>().With(x => x.RestaurantId, 2).With(x => x.UserId, 3).With(x => x.Active, true)
                    .CreateMany(2));
                _context.Reviews.AddRange(
                    _fixture.Build<Review>().With(x => x.RestaurantId, 3).With(x => x.UserId, 3).With(x => x.Active, true)
                    .CreateMany(1));
                _context.SaveChanges();
            }
            _repo = new ReviewRepository(_context);
        }

        [TestMethod]
        public void GetById_ValidId_ReturnsExpectedRecord()
        {
            // Arrange
            // Act
            var result = _repo.GetById(1);

            // Assert
            Assert.AreEqual(_context.Reviews.First(), result);
        }

        [TestMethod]
        public void GetById_InvalidId_ReturnsNull()
        {
            // Arrange
            // Act
            var result = _repo.GetById(_context.Reviews.Count() + 1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Insert_AddsRecordToDb()
        {
            // Arrange
            var review = _fixture.Build<Review>()
                .With(x => x.RestaurantId, _context.Restaurants.Last().Id)
                .With(x => x.UserId, _context.Users.Last().Id)
                .Create();

            // Act
            var result = _repo.Insert(review);

            // Assert
            var addedRecord = _context.Reviews.Last();
            Assert.AreEqual(addedRecord.Id, result);
            Assert.AreEqual(addedRecord.RestaurantId, review.RestaurantId);
            Assert.AreEqual(addedRecord.UserId, review.UserId);
            Assert.AreEqual(addedRecord.Rating, review.Rating);
            Assert.AreEqual(addedRecord.Text, review.Text);
        }

        [TestMethod]
        public void Update_ValidId_DbRecordUpdated()
        {
            // Arrange
            var review = _fixture.Create<Review>();

            // Act
            var result = _repo.Update(1, review);

            // Assert
            var updatedRecord = _context.Reviews.First();
            Assert.AreEqual(updatedRecord.Id, result);
            Assert.AreEqual(updatedRecord.Rating, review.Rating);
            Assert.AreEqual(updatedRecord.Text, review.Text);
        }

        [TestMethod]
        public void Update_InvalidId_ThrowsException()
        {
            // Arrange
            var review = _fixture.Create<Review>();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _repo.Update(_context.Reviews.Count() + 1, review));
        }

        [TestMethod]
        public void Delete_ValidId_DbRecordActiveIsFalse()
        {
            // Arrange
            // Act
            _repo.Delete(1);

            // Assert
            Assert.IsFalse(_context.Reviews.First().Active);
        }

        [TestMethod]
        public void Delete_InvalidId_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _repo.Delete(_context.Reviews.Count() + 1));
        }

        [DataTestMethod]
        [DataRow(null, 3, 3)]
        [DataRow(2, null, 5)]
        [DataRow(2, 3, 2)]
        public void Get_GivenParameters_ReturnsExpectedCounts(int? restaurantId, int? userId, int expectedCount)
        {
            // Arrange
            // Act
            var result = _repo.Get(restaurantId, userId);

            // Assert
            Assert.AreEqual(expectedCount, result.Count());
        }
    }
}
