using AutoFixture;
using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;

namespace RestaurantReviews.Tests.Domain
{
    [TestClass]
    public class ReviewTests
    {
        private readonly IFixture _fixture = new Fixture();

        [TestMethod]
        public void Constructor_AuditDateTimesSet()
        {
            // Arrange
            // Act
            var result = new Review(1, 1, "ReviewText", 0);

            // Assert
            Assert.IsNotNull(result.CreatedOn);
            Assert.IsNotNull(result.LastUpdatedOn);

        }

        [TestMethod]
        public void Update_UpdatesAttributes()
        {
            // Arrange
            var org = _fixture.Create<Review>();
            var update = _fixture.Create<Review>();

            // Act
            org.Update(update);

            // Assert
            Assert.AreEqual(update.Text, org.Text);
            Assert.AreEqual(update.Rating, org.Rating);
        }

        [TestMethod]
        public void Update_SetsLastUpdatedOn()
        {
            // Arrange
            var review = new Review(1, 1, "ReviewText", 0);
            var expectedCreate = review.CreatedOn;

            // Act
            review.Update(_fixture.Create<Review>());

            // Assert
            Assert.AreEqual(expectedCreate, review.CreatedOn);
            Assert.IsTrue(review.CreatedOn < review.LastUpdatedOn);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Delete_SetsActiveToFalse(bool activeValue)
        {
            // Arrange
            var review = _fixture.Build<Review>()
                .With(x => x.Active, activeValue)
                .Create();

            // Act
            review.Delete();

            // Assert
            Assert.IsFalse(review.Active);
        }

        [TestMethod]
        public void Delete_SetsLastUpdatedOn()
        {
            // Arrange
            var review = new Review(1, 1, "ReviewText", 0);
            var expectedCreate = review.CreatedOn;

            // Act
            review.Delete();

            // Assert
            Assert.AreEqual(expectedCreate, review.CreatedOn);
            Assert.IsTrue(review.CreatedOn < review.LastUpdatedOn);
        }
    }
}
