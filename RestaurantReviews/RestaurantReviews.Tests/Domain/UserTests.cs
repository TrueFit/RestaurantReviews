using AutoFixture;
using RestaurantReviews.Domain.AggregatesModel.UserAggregate;

namespace RestaurantReviews.Tests.Domain
{
    [TestClass]
    public class UserTests
    {
        private readonly IFixture _fixture = new Fixture();

        [TestMethod]
        public void Constructor_AuditDateTimesSet()
        {
            // Arrange
            // Act
            var result = new User("First", "Last", false);

            // Assert
            Assert.IsNotNull(result.CreatedOn);
            Assert.IsNotNull(result.LastUpdatedOn);
            Assert.AreEqual(result.CreatedOn, result.LastUpdatedOn);
        }

        [TestMethod]
        public void Update_UpdatesAttributes()
        {
            // Arrange
            var org = _fixture.Create<User>();
            var update = _fixture.Build<User>()
                .With(x => x.IsSuspended, !org.IsSuspended).Create();

            // Act
            org.Update(update);

            // Assert
            Assert.AreEqual(update.FirstName, org.FirstName);
            Assert.AreEqual(update.LastName, org.LastName);
            Assert.AreEqual(update.IsSuspended, org.IsSuspended);
        }

        [TestMethod]
        public void Update_SetsLastUpdatedOn()
        {
            // Arrange
            var user = new User("First", "Last", false);
            var expectedCreate = user.CreatedOn;

            // Act
            user.Update(_fixture.Create<User>());

            // Assert
            Assert.AreEqual(expectedCreate, user.CreatedOn);
            Assert.IsTrue(user.CreatedOn < user.LastUpdatedOn);
        }
    }
}
