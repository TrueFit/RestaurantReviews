using AutoFixture;
using Moq;
using RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate;
using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;
using RestaurantReviews.Domain.AggregatesModel.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Tests.Domain
{
    [TestClass]
    public class RestaurantReviewRepositoryTests
    {
        private IFixture _fixture;

        private Mock<IRestaurantRepository> _mockRestaurantRepo;
        private Mock<IReviewRepository> _mockReviewRepo;
        private Mock<IUserRepository> _mockUserRepo;

        private RestaurantReviewRepository _repo;

        [TestInitialize]
        public void Initialize()
        {
            _fixture = new Fixture();

            _mockRestaurantRepo = new Mock<IRestaurantRepository>();
            _mockReviewRepo = new Mock<IReviewRepository>();
            _mockUserRepo = new Mock<IUserRepository>();

            _repo = new RestaurantReviewRepository(_mockRestaurantRepo.Object, _mockReviewRepo.Object, _mockUserRepo.Object);
        }

        [TestMethod]
        public void Validate_RestaurantIdDoesNotExist_ThrowsException()
        {
            // Arrange
            _mockRestaurantRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns((Restaurant) null);
            var review = _fixture.Create<Review>();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _repo.Validate(review));
        }

        [TestMethod]
        public void ValidateUser_UserIdDoesNotExist_ThrowsException()
        {
            // Arrange
            _mockRestaurantRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(_fixture.Create<Restaurant>());
            _mockUserRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns((User) null);
            var review = _fixture.Create<Review>();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _repo.Validate(review));
        }

        [TestMethod]
        public void Validate_UserIsSuspended_ThrowsException()
        {
            // Arrange
            var suspendedUser = _fixture.Build<User>()
                .With(x => x.IsSuspended, true).Create();
            _mockRestaurantRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(_fixture.Create<Restaurant>());
            _mockUserRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(suspendedUser);
            var review = _fixture.Create<Review>();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() => _repo.Validate(review));
        }

        [TestMethod]
        public void AddReview_ValidArguments_ReviewInserted()
        {
            // Arrange
            var validUser = _fixture.Build<User>()
                .With(x => x.IsSuspended, false).Create();
            var review = _fixture.Create<Review>();
            _mockRestaurantRepo.Setup(x => x.GetById(review.RestaurantId)).Returns(_fixture.Create<Restaurant>());
            _mockUserRepo.Setup(x => x.GetById(review.UserId)).Returns(validUser);

            // Act
            _repo.AddReview(review);

            // Assert
            _mockReviewRepo.Verify(x => x.Insert(review), Times.Once);
        }

        [TestMethod]
        public void UpdateReview_ValidArguments_ReviewUpdated()
        {
            // Arrange
            var validUser = _fixture.Build<User>()
                .With(x => x.IsSuspended, false).Create();
            var review = _fixture.Create<Review>();
            _mockRestaurantRepo.Setup(x => x.GetById(review.RestaurantId)).Returns(_fixture.Create<Restaurant>());
            _mockUserRepo.Setup(x => x.GetById(review.UserId)).Returns(validUser);

            // Act
            _repo.UpdateReview(review.Id, review);

            // Assert
            _mockReviewRepo.Verify(x => x.Update(review.Id, review), Times.Once);
        }

        [TestMethod]
        public void DeleteReview_ValidArguments_ReviewDeleted()
        {
            // Arrange
            var validUser = _fixture.Build<User>()
                .With(x => x.IsSuspended, false).Create();
            var review = _fixture.Create<Review>();
            _mockRestaurantRepo.Setup(x => x.GetById(review.RestaurantId)).Returns(_fixture.Create<Restaurant>());
            _mockUserRepo.Setup(x => x.GetById(review.UserId)).Returns(validUser);

            // Act
            _repo.DeleteReview(review.Id, review.UserId);

            // Assert
            _mockReviewRepo.Verify(x => x.Delete(review.Id), Times.Once);
        }
    }
}
