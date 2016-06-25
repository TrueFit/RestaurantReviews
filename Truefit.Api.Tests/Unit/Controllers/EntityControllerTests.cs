using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Truefit.Api.Controllers;
using Truefit.Entities;
using Truefit.Reviews;
using Truefit.Reviews.Models;

namespace Truefit.Api.Tests.Unit.Controllers
{
    [TestFixture]
    public class EntityControllerTests : IDisposable
    {
        private Mock<IEntityService> _entityService;
        private Mock<IReviewService> _reviewService;
        private EntityController _controller;

        [SetUp]
        public void Setup()
        {
            this._entityService = new Mock<IEntityService>();
            this._reviewService = new Mock<IReviewService>();
            this._controller = new EntityController(this._entityService.Object, this._reviewService.Object);
        }

        [Test]
        public async Task PostReview_Should_Set_EntityId()
        {
            var entityId = Guid.NewGuid();
            var review = new ReviewModel();

            await this._controller.PostReview(entityId, review);

            this._reviewService.Verify(x => x.AddUserReview(It.Is<ReviewModel>(r => r.EntityGuid == entityId)));
        }

        public void Dispose()
        {
            this._controller.Dispose();
        }
    }
}
