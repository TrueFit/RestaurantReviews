using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Truefit.Reviews.Models;
using Truefit.Reviews.Repositories;


namespace Truefit.Reviews.Tests
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private Mock<IReviewRepository> _reviewRepository;
        private IReviewService _reviewService;

        [SetUp]
        public void Setup()
        {
            this._reviewRepository = new Mock<IReviewRepository>();
            this._reviewService = new ReviewService(this._reviewRepository.Object);
        }

        [Test]
        public async Task GetByEntity_Is_Repo_Passthrough()
        {
            var guid = Guid.NewGuid();
            var expected = new [] { new ReviewModel() };

            this._reviewRepository.Setup(x => x.GetByEntity(guid)).ReturnsAsync(expected);
            var actual = await this._reviewService.GetByEntity(guid);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task AddUserReview_Sets_New_Guid()
        {
            var guid = Guid.NewGuid();
            var review = new ReviewModel {Guid = guid};

            await this._reviewService.AddUserReview(review);

            this._reviewRepository.Verify(x => x.Insert(It.Is<ReviewModel>(r => r.Guid != guid)));
        }

        [Test]
        public async Task RemoveUserReview_Returns_False_If_No_Review_Found()
        {
            var userId = Guid.NewGuid();
            var reviewId = Guid.NewGuid();

            this._reviewRepository.Setup(x => x.GetByGuid(reviewId)).ReturnsAsync(null);

            var actual = await this._reviewService.RemoveUserReview(reviewId, userId);
            Assert.IsFalse(actual);
        }

        [Test]
        public async Task RemoveUserReview_Returns_False_If_Not_By_User()
        {
            var userId = Guid.NewGuid();
            var reviewId = Guid.NewGuid();
            var review = new ReviewModel { UserGuid = Guid.NewGuid() };

            this._reviewRepository.Setup(x => x.GetByGuid(reviewId)).ReturnsAsync(review);

            var actual = await this._reviewService.RemoveUserReview(reviewId, userId);
            Assert.IsFalse(actual);
        }

        [Test]
        public async Task RemoveUserReview_Does_Not_Call_Delete_If_Not_By_User()
        {
            var userId = Guid.NewGuid();
            var reviewId = Guid.NewGuid();
            var review = new ReviewModel { UserGuid = Guid.NewGuid() };

            this._reviewRepository.Setup(x => x.GetByGuid(reviewId)).ReturnsAsync(review);

            await this._reviewService.RemoveUserReview(reviewId, userId);
            
            this._reviewRepository.Verify(x => x.Delete(reviewId), Times.Never);
        }

        [Test]
        public async Task RemoveUserReview_Returns_True_If_Match()
        {
            var userId = Guid.NewGuid();
            var reviewId = Guid.NewGuid();
            var review = new ReviewModel { UserGuid = userId };

            this._reviewRepository.Setup(x => x.GetByGuid(reviewId)).ReturnsAsync(review);

            var actual = await this._reviewService.RemoveUserReview(reviewId, userId);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task RemoveUserReview_Calls_Delete_If_Match()
        {
            var userId = Guid.NewGuid();
            var reviewId = Guid.NewGuid();
            var review = new ReviewModel { UserGuid = userId };

            this._reviewRepository.Setup(x => x.GetByGuid(reviewId)).ReturnsAsync(review);

            await this._reviewService.RemoveUserReview(reviewId, userId);

            this._reviewRepository.Verify(x => x.Delete(reviewId));
        }
    }
}
