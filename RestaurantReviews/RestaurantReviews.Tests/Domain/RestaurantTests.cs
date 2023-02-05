using AutoFixture;
using RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate;

namespace RestaurantReviews.Tests.Domain
{
    [TestClass]
    public class RestaurantTests
    {
        private readonly IFixture _fixture = new Fixture();

        [TestMethod]
        public void Update_UpdatesNameAndCity()
        {
            // Arrange
            var org = _fixture.Create<Restaurant>();
            var update = _fixture.Create<Restaurant>();

            // Act
            org.Update(update);

            // Assert
            Assert.AreEqual(update.Name, org.Name);
            Assert.AreEqual(update.City, org.City);
        }
    }
}
