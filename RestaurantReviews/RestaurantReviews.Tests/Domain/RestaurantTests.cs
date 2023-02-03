using AutoFixture;
using RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate;

namespace RestaurantReviews.Tests.Domain
{
    [TestClass]
    public class RestaurantTests
    {
        private readonly IFixture _fixture = new Fixture();

        [TestMethod]
        public void Constructor_AuditDateTimesSet()
        {
            // Arrange
            // Act
            var result = new Restaurant(1, "RestaurantName", "CityTown");

            // Assert
            Assert.IsNotNull(result.CreatedOn);
            Assert.IsNotNull(result.LastUpdatedOn);
            Assert.AreEqual(result.CreatedOn, result.LastUpdatedOn);
        }

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

        [TestMethod]
        public void Update_SetsLastUpdatedOn()
        {
            // Arrange
            var place = new Restaurant(1, "RestaurantName", "CityTown");
            var expectedCreate = place.CreatedOn;

            // Act
            place.Update(_fixture.Create<Restaurant>());

            // Assert
            Assert.AreEqual(expectedCreate, place.CreatedOn);
            Assert.IsTrue(place.CreatedOn < place.LastUpdatedOn);
        }
    }
}
