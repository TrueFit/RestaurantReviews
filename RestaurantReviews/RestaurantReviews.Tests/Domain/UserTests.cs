using AutoFixture;
using RestaurantReviews.Domain.AggregatesModel.UserAggregate;

namespace RestaurantReviews.Tests.Domain
{
    [TestClass]
    public class UserTests
    {
        private readonly IFixture _fixture = new Fixture();

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
    }
}
