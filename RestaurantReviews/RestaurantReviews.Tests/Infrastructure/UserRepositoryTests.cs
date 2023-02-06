using AutoFixture;
using RestaurantReviews.Domain.AggregatesModel.UserAggregate;
using RestaurantReviews.Infrastructure.Repositories;

namespace RestaurantReviews.Tests.Infrastructure
{
    [TestClass]
    public class UserRepositoryTests : BaseInfrastructureTests
    {
        private UserRepository _repo;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _repo = new UserRepository(_context);
        }

        [TestMethod]
        public void GetById_ValidId_ReturnsExpectedRecord()
        {
            // Arrange
            // Act
            var result = _repo.GetById(1);

            // Assert
            Assert.AreEqual(_context.Users.First(), result);
        }

        [TestMethod]
        public void GetById_InvalidId_ReturnsNull()
        {
            // Arrange
            // Act
            var result = _repo.GetById(_context.Users.Count() + 1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Insert_AddsRecordToDb()
        {
            // Arrange
            var user = _fixture.Create<User>();

            // Act
            var result = _repo.Insert(user);

            // Assert
            var addedRecord = _context.Users.Last();
            Assert.AreEqual(addedRecord.Id, result);
            Assert.AreEqual(addedRecord.FirstName, user.FirstName);
            Assert.AreEqual(addedRecord.LastName, user.LastName);
            Assert.AreEqual(addedRecord.IsSuspended, user.IsSuspended);
        }

        [TestMethod]
        public void Update_ValidId_DbRecordUpdated()
        {
            // Arrange
            var user = _fixture.Create<User>();

            // Act
            var result = _repo.Update(1, user);

            // Assert
            var updatedRecord = _context.Users.First();
            Assert.AreEqual(updatedRecord.Id, result);
            Assert.AreEqual(updatedRecord.FirstName, user.FirstName);
            Assert.AreEqual(updatedRecord.LastName, user.LastName);
            Assert.AreEqual(updatedRecord.IsSuspended, user.IsSuspended);
        }

        [TestMethod]
        public void Update_InvalidId_ThrowsException()
        {
            // Arrange
            var user = _fixture.Create<User>();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _repo.Update(_context.Users.Count() + 1, user));
        }
    }
}
