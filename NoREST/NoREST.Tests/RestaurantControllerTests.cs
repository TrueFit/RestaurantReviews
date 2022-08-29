using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NoREST.Api.Auth;
using NoREST.Api.Controllers;
using NoREST.DataAccess.Entities;
using NoREST.DataAccess.Repositories;
using NoREST.Domain;
using NoREST.Models;
using NoREST.Models.ViewModels;
using NoREST.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NoREST.Tests
{
    public class RestaurantControllerTests
    {
        private static readonly IFixture _fixture = NoRestFixture.Create();

        [Fact]
        public async Task Create_SetsAuditValues_And_ReturnsCreated_OnSuccess()
        {
            using (var testFacility = new RestaurantControllerTestFacility())
            {
                var creation = _fixture.Create<RestaurantCreation>();
                var expectedEntity = _fixture.Create<Restaurant>();
                var expectedUser = _fixture.Create<UserProfile>();
                testFacility.ExpectRestaurantSave(creation, expectedEntity.Id);
                testFacility.ExpectGetUser(expectedUser);
                testFacility.AuditLogicMock.Setup(m => m.SetAuditAndOwnershipValues<RestaurantProfile, UserProfile, int>(
                    It.IsAny<RestaurantProfile>(), expectedUser, true, null));

                var result = await testFacility.Sut.Create(creation);

                var created = result.Should().BeAssignableTo<CreatedResult>().Which;
                created.Location.Should().Contain(expectedEntity.Id.ToString());
                created.Value.Should().BeAssignableTo<RestaurantProfile>().Which.Id
                    .Should().Be(expectedEntity.Id);
            }
        }

        [Fact]
        public async Task Create_ReturnsConflict_WhenWeCannotSaveToDatabase()
        {
            using (var testFacility = new RestaurantControllerTestFacility())
            {
                var creation = _fixture.Create<RestaurantCreation>();
                var expectedEntity = _fixture.Create<Restaurant>();
                var expectedException = _fixture.Create<ApplicationException>();
                var expectedUser = _fixture.Create<UserProfile>();
                testFacility.ExpectRestaurantSaveFailure(expectedException);
                testFacility.LogicLoggingAssertionHelper.SetupLogging(LogLevel.Error);
                testFacility.ExpectGetUser(expectedUser);
                testFacility.AuditLogicMock.Setup(m => m.SetAuditAndOwnershipValues<RestaurantProfile, UserProfile, int>(
                    It.IsAny<RestaurantProfile>(), expectedUser, true, null));

                var result = await testFacility.Sut.Create(creation);
                
                result.Should().BeAssignableTo<ConflictResult>();
            }
        }

        [Fact]
        public async Task CreateReview_ReturnsInternalServerError_AndLogsException_ForUnexpectedErrors()
        {
            using (var testFacility = new RestaurantControllerTestFacility())
            {
                var creation = _fixture.Create<ReviewCreation>();
                var expectedUser = _fixture.Create<UserProfile>();
                var expectedException = _fixture.Create<ApplicationException>();
                testFacility.ExpectGetUser(expectedUser);
                testFacility.ExpectExceptionFromGetRestaurant(expectedException);
                testFacility.LogicLoggingAssertionHelper.SetupLogging(LogLevel.Error);

                var response = await testFacility.Sut.Review(creation);

                response.Should().BeAssignableTo<IStatusCodeActionResult>().Which.StatusCode
                    .Should().Be((int)HttpStatusCode.InternalServerError);
            }
        }

        [Fact]
        public async Task CreateReview_ReturnsNotFound_WhenSpecifiedRestaurantId_DoesNotExist_AndIndicateThatId()
        {
            using (var testFacility = new RestaurantControllerTestFacility())
            {
                var creation = _fixture.Create<ReviewCreation>();
                var expectedUser = _fixture.Create<UserProfile>();
                testFacility.ExpectGetUser(expectedUser);
                testFacility.ExpectRestaurantFetchFailure();                

                var response = await testFacility.Sut.Review(creation);

                response.Should().BeAssignableTo<NotFoundObjectResult>()
                    .Which.Value.As<string>().Should().Contain(creation.RestaurantId.ToString());
            }
        }

        [Fact]
        public async Task CreateReview_ReturnsUnauthorized_WhenAuthenticatedUser_HasBeenBannedFromReviewingSpecified_Restaurant()
        {
            using (var testFacility = new RestaurantControllerTestFacility())
            {
                var creation = _fixture.Create<ReviewCreation>();
                var expectedUser = _fixture.Create<UserProfile>();
                var expectedRestaurant = _fixture.Build<RestaurantProfile>()
                    .With(r => r.BannedUsers, _fixture.Build<UserRestaurantBanModel>().With(urb => urb.UserId, expectedUser.Id).CreateMany(1).ToList())
                    .Create();
                testFacility.ExpectGetUser(expectedUser);
                testFacility.ExpectRestaurantFetch(expectedRestaurant, creation.RestaurantId);

                var response = await testFacility.Sut.Review(creation);

                var responseObject = response.Should().BeAssignableTo<ObjectResult>().Which;
                responseObject.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
                responseObject.Value.As<string>().Should().Contain("blocked");
            }
        }

        [Fact]
        public async Task CreateReview_ReturnsCreatedResponse_AndSetsAuditValues_ForNonBannedUser_MakingGoodRequest()
        {
            using (var testFacility = new RestaurantControllerTestFacility())
            {
                var creation = _fixture.Create<ReviewCreation>();
                var expectedCreatedId = _fixture.Create<int>();
                var expectedUser = _fixture.Create<UserProfile>();
                var notMyUserIdGenerator = _fixture.Create<Generator<int>>().Where(i => i != expectedUser.Id);
                var expectedRestaurant = _fixture.Build<RestaurantProfile>()
                    .With(r => r.BannedUsers, _fixture.Build<UserRestaurantBanModel>().With(urb => urb.UserId, notMyUserIdGenerator.First()).CreateMany(12).ToList())
                    .Create();
                testFacility.ExpectGetUser(expectedUser);
                testFacility.ExpectRestaurantFetch(expectedRestaurant, creation.RestaurantId);
                testFacility.AuditLogicMock.Setup(m => m.SetAuditAndOwnershipValues<ReviewProfile, UserProfile, int>(
                    It.Is<ReviewProfile>(rp => rp.Content == creation.Content && rp.RestaurantId == creation.RestaurantId),
                    expectedUser, true, null));
                testFacility.ExpectSaveReview(creation, expectedCreatedId);

                var response = await testFacility.Sut.Review(creation);

                var responseObject = response.Should().BeAssignableTo<CreatedResult>().Which;
                responseObject.Location.Should().Contain(expectedCreatedId.ToString());
                responseObject.Value.As<ReviewProfile>().Content.Should().Be(creation.Content);
            }
        }

        [Fact]
        public async Task DeleteReview_ReturnsOk_OnSuccess()
        {
            using (var testFacility = new RestaurantControllerTestFacility())
            {
                var reviewId = _fixture.Create<int>();
                var expectedUser = _fixture.Create<UserProfile>();
                var expectedReview = _fixture.Build<ReviewProfile>()
                    .With(r => r.Id, reviewId).Create();
                testFacility.ExpectGetUser(expectedUser);
                testFacility.ExpectGetReview(reviewId, expectedReview);
                testFacility.PermissionLogicMock.Setup(m => m.CanModify(
                    It.Is<ReviewProfile>(rp => rp.Id == reviewId),
                    expectedUser)).Returns(true);
                testFacility.ExpectDeleteReview(reviewId);
                testFacility.AuditLogicMock.Setup(m => m.SetAuditValues(expectedReview, false, null));

                var result = await testFacility.Sut.DeleteReview(reviewId);

                result.Should().BeAssignableTo<OkResult>();                
            }
        }

        [Fact]
        public async Task DeleteReview_ReturnsUnauthorized_IfUserIsNotAuthorizedToDeleteReview()
        {
            using (var testFacility = new RestaurantControllerTestFacility())
            {
                var reviewId = _fixture.Create<int>();
                var expectedUser = _fixture.Create<UserProfile>();
                var expectedReview = _fixture.Build<ReviewProfile>()
                    .With(r => r.Id, reviewId).Create();
                testFacility.ExpectGetUser(expectedUser);
                testFacility.ExpectGetReview(reviewId, expectedReview);
                testFacility.PermissionLogicMock.Setup(m => m.CanModify(
                    It.Is<ReviewProfile>(rp => rp.Id == reviewId),
                    expectedUser)).Returns(false);

                var result = await testFacility.Sut.DeleteReview(reviewId);

                result.Should().BeAssignableTo<UnauthorizedObjectResult>();
            }
        }


        private class RestaurantControllerTestFacility : IDisposable
        {
            private readonly MockRepository _mockRepo = new MockRepository(MockBehavior.Strict);
            public Mock<IRestaurantRepository> RestaurantRepoMock { get; set; }
            public Mock<IReviewRepository> ReviewRepoMock { get; set; }
            public RestaurantController Sut { get; set; }
            public LoggingAssertionHelper<IRestaurantLogic> LogicLoggingAssertionHelper { get; set; }
            public Mock<IAuthService> AuthServiceMock { get; set; }
            public Mock<IPermissionLogic> PermissionLogicMock { get; set; }
            public Mock<IAuditLogic> AuditLogicMock { get; set; }

            public RestaurantControllerTestFacility()
            {
                AuthServiceMock = _mockRepo.Create<IAuthService>();
                ReviewRepoMock = _mockRepo.Create<IReviewRepository>();
                PermissionLogicMock = _mockRepo.Create<IPermissionLogic>();
                var loggerMock = _mockRepo.Create<ILogger<RestaurantController>>();
                var logicLoggerMock = _mockRepo.Create<ILogger<IRestaurantLogic>>();
                LogicLoggingAssertionHelper = new LoggingAssertionHelper<IRestaurantLogic>(logicLoggerMock);
                RestaurantRepoMock = _mockRepo.Create<IRestaurantRepository>();
                var mapper = TestServiceRegistry.GetAutoMapperMapper();
                AuditLogicMock = _mockRepo.Create<IAuditLogic>();

                var logic = new RestaurantLogic(
                    RestaurantRepoMock.Object,
                    mapper,
                    logicLoggerMock.Object,
                    ReviewRepoMock.Object,
                    PermissionLogicMock.Object,
                    AuditLogicMock.Object);
                Sut = new RestaurantController(logic, loggerMock.Object, AuthServiceMock.Object);
            }

            public void ExpectRestaurantSave(RestaurantCreation creation, int? expectedId)
            {
                RestaurantRepoMock.Setup(m => m.Create(It.Is<Restaurant>(r => r.Name == creation.Name)))
                    .ReturnsAsync(expectedId);
            }

            public void ExpectGetUser(UserProfile expected)
            {
                AuthServiceMock.Setup(m => m.GetCurrentlyAuthenticatedUser())
                    .Returns(expected);
            }

            public void ExpectRestaurantSaveFailure(Exception expected)
            {
                RestaurantRepoMock.Setup(m => m.Create(It.IsAny<Restaurant>()))
                    .Throws(expected);
            }

            public void ExpectExceptionFromGetRestaurant(Exception expected)
            {
                RestaurantRepoMock.Setup(m => m.GetRestaurant(It.IsAny<int>()))
                    .ThrowsAsync(expected);
            }

            public void ExpectRestaurantFetchFailure()
            {
                RestaurantRepoMock.Setup(m => m.GetRestaurant(It.IsAny<int>()))
                    .Returns(Task.FromResult<RestaurantProfile>(null));
            }

            public void ExpectRestaurantFetch(RestaurantProfile expected, int id)
            {
                RestaurantRepoMock.Setup(m => m.GetRestaurant(id))
                    .ReturnsAsync(expected);
            }

            public void ExpectSaveReview(ReviewCreation review, int expectedId)
            {
                ReviewRepoMock.Setup(m => m.Create(It.Is<Review>(r => r.RestaurantId == review.RestaurantId && r.Content == review.Content)))
                    .ReturnsAsync(expectedId);
            }

            public void ExpectGetReview(int reviewId, ReviewProfile expected)
            {
                ReviewRepoMock.Setup(m => m.GetReview(reviewId))
                    .ReturnsAsync(expected);
            }

            public void ExpectDeleteReview(int reviewId)
            {
                ReviewRepoMock.Setup(m => m.DeleteReview(reviewId))
                    .ReturnsAsync(true);
            }


            public void Dispose()
            {
                _mockRepo.VerifyAll();
            }
        }




        //ToDo: move this to some kind of db tests class...
        [Fact]
        public void UsingNameOf_LikeThat_Works()
        {
            nameof(Restaurant.Name).Should().Be("Name", "Otherwise I have to hardcode strings in the FK attribute and thats a mess");
        }


    }
}
