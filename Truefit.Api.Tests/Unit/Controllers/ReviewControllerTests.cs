using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using Truefit.Api.Controllers;
using Truefit.Reviews;
using Truefit.Users;
using Truefit.Users.Models;

namespace Truefit.Api.Tests.Unit.Controllers
{
    [TestFixture]
    public class ReviewControllerTests : IDisposable
    {
        private Mock<IUserService> _userService;
        private Mock<IReviewService> _reviewService;
        private ReviewController _controller;

        [SetUp]
        public void Setup()
        {
            this._userService = new Mock<IUserService>();
            this._reviewService = new Mock<IReviewService>();
            this._controller = new ReviewController(this._userService.Object, this._reviewService.Object);
        }

        [Test]
        public async Task DeleteReview_Should_OkResult_For_Success()
        {
            this._userService.Setup(x => x.Authenticate(It.IsAny<string>())).ReturnsAsync(new UserModel());
            this._reviewService.Setup(x => x.RemoveUserReview(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            var result = await this._controller.DeleteReview(Guid.NewGuid(), string.Empty);
            Assert.IsInstanceOf<OkResult>( result);
        }

        [Test]
        public async Task DeleteReview_Should_NotFoundResult_For_Failure()
        {
            this._userService.Setup(x => x.Authenticate(It.IsAny<string>())).ReturnsAsync(new UserModel());
            this._reviewService.Setup(x => x.RemoveUserReview(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var result = await this._controller.DeleteReview(Guid.NewGuid(), string.Empty);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._controller == null) return;
            this._controller.Dispose();
            this._controller = null;
        }
    }
}
