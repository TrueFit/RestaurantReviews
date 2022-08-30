using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NoREST.Api.Auth;
using NoREST.Api.Controllers;
using NoREST.DataAccess.Entities;
using NoREST.DataAccess.Repositories;
using NoREST.Domain;
using NoREST.Models.DomainModels;
using NoREST.Models.ViewModels.Creation;
using NoREST.Models.ViewModels.Outgoing;
using NoREST.Models.ViewModels.Profile;
using NoREST.Tests.Utilities;
using System.Net;
using Xunit;

namespace NoREST.Tests
{
    public class UserTests
    {
        private static readonly IFixture _fixture = new Fixture();


        [Fact]
        public async Task Create_ReturnsInternalServerError_WhenIdentityProviderCall_Returns_AnUnsuccessfulResponse()
        {
            var userCreation = _fixture.Create<UserCreation>();
            var expectedIdpResponse = new UserCreationResponse
            {
                IsSuccess = false,
                Error = _fixture.Create<string>()
            };

            using (var testFacility = new UserTestFacility())
            {
                testFacility.ExpectIdentityProviderCreateUser(userCreation, expectedIdpResponse);

                var result = await testFacility.Sut.Create(userCreation);
                var objectResult = result.Should().BeAssignableTo<ObjectResult>()
                    .Which;
                objectResult.Value.Should().BeAssignableTo<string>().Which.Should().Contain(expectedIdpResponse.Error);
                objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            }
        }

        [Fact]
        public async Task Create_ReturnsInternalServerError_WhenIdentityProviderCall_ThrowsAnUnhandledException()
        {
            var userCreation = _fixture.Create<UserCreation>();
            
            using (var testFacility = new UserTestFacility())
            {
                testFacility.ExpectIdentityProviderException(new ApplicationException("oops"));

                var result = await testFacility.Sut.Create(userCreation);
                var objectResult = result.Should().BeAssignableTo<StatusCodeResult>()
                    .Which.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            }
        }

        [Fact]
        public async Task CreateUser_ReturnsConflictStatus_WhenUserIsCreatedInIdp_But_CannotBeCreatedLocally()
        {
            var userCreation = _fixture.Create<UserCreation>();
            var expectedIdpResponse = new UserCreationResponse
            {
                IsSuccess = true
            };

            using (var testFacility = new UserTestFacility())
            {
                testFacility.ExpectIdentityProviderCreateUser(userCreation, expectedIdpResponse);
                testFacility.ExpectUserSaveFailure(userCreation);

                var result = await testFacility.Sut.Create(userCreation);

                var objectResult = result.Should().BeAssignableTo<ObjectResult>()
                    .Which.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
            }
        }

        [Fact]
        public async Task CreateUser_MapsIdFromIdp_ToUserEntity_WhenCreatedSuccessfully()
        {
            var userCreation = _fixture.Create<UserCreation>();
            var expectedIdpResponse = new UserCreationResponse
            {
                IsSuccess = true,
                IdentityProviderId = _fixture.Create<string>()
            };

            var expectedUser = _fixture.Build<UserProfile>().With(u => u.UserName, userCreation.Username).Create();

            using (var testFacility = new UserTestFacility())
            {
                testFacility.ExpectIdentityProviderCreateUser(userCreation, expectedIdpResponse);
                testFacility.ExpectUserSaveSuccess(expectedIdpResponse.IdentityProviderId, userCreation.Username, expectedUser);

                var result = await testFacility.Sut.Create(userCreation);

                var objectResult = result.Should().BeAssignableTo<CreatedResult>()
                    .Which;
                objectResult.Location.Should().Contain(expectedUser.Id.ToString());
                objectResult.Value.Should().BeAssignableTo<UserProfile>().Which.UserName
                    .Should().Be(userCreation.Username);
            }
        }

        [Fact]
        public async Task BanUser_ReturnsNotFound_WhenRestaurantDoesNotExist()
        {
            using (var testFacility = new UserTestFacility())
            {
                var targetUserId = _fixture.Create<int>();
                var restaurantId = _fixture.Create<int>();
                var reason = _fixture.Create<string>();
                testFacility.ExpectGetRestaurantResult(restaurantId, null);
                var expectedCurrentUser = _fixture.Create<UserProfile>();
                testFacility.ExpectGetCurrentUser(expectedCurrentUser);

                var result = await testFacility.Sut.BanUserFromRestaurant(targetUserId, restaurantId, reason);

                result.Should().BeAssignableTo<ObjectResult>()
                    .Which.Value.As<string>().Should().Contain(restaurantId.ToString());
            }
        }

        [Fact]
        public async Task BanUser_ReturnsNotFound_WhenTargetUserDoesNotExist()
        {
            using (var testFacility = new UserTestFacility())
            {
                var targetUserId = _fixture.Create<int>();
                var restaurantId = _fixture.Create<int>();
                var reason = _fixture.Create<string>();
                var expectedCurrentUser = _fixture.Create<UserProfile>();
                testFacility.ExpectGetCurrentUser(expectedCurrentUser);
                var expectedRestaurant = _fixture.Create<RestaurantProfile>();                
                testFacility.ExpectGetRestaurantResult(restaurantId, expectedRestaurant);
                testFacility.ExpectGetUserResult(targetUserId, null);

                var result = await testFacility.Sut.BanUserFromRestaurant(targetUserId, restaurantId, reason);

                result.Should().BeAssignableTo<ObjectResult>()
                    .Which.Value.As<string>().Should().Contain(targetUserId.ToString());
            }
        }

        [Fact]
        public async Task BanUser_ReturnsUnauthorized_IfUserDoesNotHavePermission_ToImposeBan()
        {
            using (var testFacility = new UserTestFacility())
            {
                var expectedTargetUser = _fixture.Create<UserProfile>();
                var reason = _fixture.Create<string>();
                var notMyUserIdGenerator = _fixture.Create<Generator<int>>().Where(i => i != expectedTargetUser.Id);
                var expectedRestaurant = _fixture.Build<RestaurantProfile>()
                    .With(r => r.BannedUsers, _fixture.Build<UserRestaurantBanModel>().With(b => b.UserId, notMyUserIdGenerator.First()).CreateMany(12).ToList())
                    .Create();
                var expectedCurrentUser = _fixture.Create<UserProfile>();
                testFacility.ExpectGetRestaurantResult(expectedRestaurant.Id, expectedRestaurant);
                testFacility.ExpectGetUserResult(expectedTargetUser.Id, expectedTargetUser);
                testFacility.ExpectGetCurrentUser(expectedCurrentUser);
                testFacility.ExpectPermissionsCheck(expectedRestaurant, expectedCurrentUser, false);
                
                var result = await testFacility.Sut.BanUserFromRestaurant(expectedTargetUser.Id, expectedRestaurant.Id, reason);

                result.Should().BeAssignableTo<UnauthorizedObjectResult>();
            }
        }

        [Fact]
        public async Task BanUser_ReturnsOk_ButDoesNotInvokeRepoSaveMethod_OrCheckPermissions_WhenUserIsAlreadyBanned()
        {
            using (var testFacility = new UserTestFacility())
            {
                var expectedTargetUser = _fixture.Create<UserProfile>();
                var reason = _fixture.Create<string>();
                var expectedRestaurant = _fixture.Build<RestaurantProfile>()
                    .With(r => r.BannedUsers, _fixture.Build<UserRestaurantBanModel>().With(b => b.UserId, expectedTargetUser.Id).CreateMany(1).ToList())
                    .Create();
                var expectedCurrentUser = _fixture.Create<UserProfile>();
                testFacility.ExpectGetRestaurantResult(expectedRestaurant.Id, expectedRestaurant);
                testFacility.ExpectGetUserResult(expectedTargetUser.Id, expectedTargetUser);
                testFacility.ExpectGetCurrentUser(expectedCurrentUser);

                var result = await testFacility.Sut.BanUserFromRestaurant(expectedTargetUser.Id, expectedRestaurant.Id, reason);

                result.Should().BeAssignableTo<OkResult>();
            }
        }

        [Fact]
        public async Task BanUser_ReturnsOk_WhenUserHasPermission_ToImposeBan()
        {
            using (var testFacility = new UserTestFacility())
            {
                var expectedTargetUser = _fixture.Create<UserProfile>();
                var reason = _fixture.Create<string>();
                var notMyUserIdGenerator = _fixture.Create<Generator<int>>().Where(i => i != expectedTargetUser.Id);
                var expectedRestaurant = _fixture.Build<RestaurantProfile>()
                    .With(r => r.BannedUsers, _fixture.Build<UserRestaurantBanModel>().With(b => b.UserId, notMyUserIdGenerator.First()).CreateMany(12).ToList())
                    .Create();
                var expectedCurrentUser = _fixture.Create<UserProfile>();
                testFacility.ExpectGetRestaurantResult(expectedRestaurant.Id, expectedRestaurant);
                testFacility.ExpectGetUserResult(expectedTargetUser.Id, expectedTargetUser);
                testFacility.ExpectGetCurrentUser(expectedCurrentUser);
                testFacility.ExpectPermissionsCheck(expectedRestaurant, expectedCurrentUser, true);
                testFacility.AuditLogicMock.Setup(m => m.SetAuditValues(
                    It.Is<UserRestaurantBanModel>(
                    b => b.RestaurantId == expectedRestaurant.Id
                    && b.UserId == expectedTargetUser.Id),
                    false, null));
                testFacility.ExpectBanSave(expectedTargetUser.Id, expectedRestaurant.Id, reason, expectedCurrentUser.Id);

                var result = await testFacility.Sut.BanUserFromRestaurant(expectedTargetUser.Id, expectedRestaurant.Id, reason);

                result.Should().BeAssignableTo<OkResult>();
            }
        }

        private class UserTestFacility : IDisposable
        {
            private readonly MockRepository _mockRepo = new MockRepository(MockBehavior.Strict);

            public UserController Sut { get; set; }
            public Mock<IUserRepository> UserRepoMock { get; set; }
            public Mock<IIdentityProviderService> IdentityProviderServiceMock { get; set; }
            public Mock<IPermissionLogic> PermissionLogicMock { get; set; }
            public Mock<IAuditLogic> AuditLogicMock { get; set; }
            public Mock<IAuthService> AuthServiceMock { get; set; }
            public Mock<IRestaurantLogic> RestaurantLogicMock { get; set; }
            public LoggingAssertionHelper<UserController> LoggingAssertionHelper { get; set; }

            public UserTestFacility()
            {
                UserRepoMock = _mockRepo.Create<IUserRepository>();
                IdentityProviderServiceMock = _mockRepo.Create<IIdentityProviderService>();
                PermissionLogicMock = _mockRepo.Create<IPermissionLogic>();
                AuditLogicMock = _mockRepo.Create<IAuditLogic>();
                AuthServiceMock = _mockRepo.Create<IAuthService>();
                RestaurantLogicMock = _mockRepo.Create<IRestaurantLogic>();
                var logger = _mockRepo.Create<ILogger<UserController>>();
                LoggingAssertionHelper = new LoggingAssertionHelper<UserController>(logger);
                var mapper = TestServiceRegistry.GetAutoMapperMapper();
                var userLogic = new UserLogic(
                    mapper,
                    UserRepoMock.Object,
                    IdentityProviderServiceMock.Object,
                    PermissionLogicMock.Object,
                    AuditLogicMock.Object);

                var banner = new BanManager(RestaurantLogicMock.Object, userLogic, PermissionLogicMock.Object, mapper, UserRepoMock.Object, AuditLogicMock.Object);

                Sut = new UserController(userLogic, AuthServiceMock.Object, RestaurantLogicMock.Object, logger.Object, banner);
            }

            public void ExpectIdentityProviderException(Exception theUnexpected)
            {
                IdentityProviderServiceMock.Setup(m => m.CreateUser(It.IsAny<UserCreation>()))
                    .ThrowsAsync(theUnexpected);
            }

            public void ExpectIdentityProviderCreateUser(UserCreation expectedUser, UserCreationResponse expectedResponse)
            {
                IdentityProviderServiceMock.Setup(m => m.CreateUser(expectedUser))
                    .ReturnsAsync(expectedResponse);
            }

            public void ExpectUserSaveSuccess(string idpId, string userName, UserProfile expectedUser)
            {
                UserRepoMock
                    .Setup(m => m.CreateUser(It.Is<User>(u => 
                        u.UserName == userName 
                        && u.IdentityProviderId == idpId)))
                    .ReturnsAsync(expectedUser);
            }

            public void ExpectUserSaveFailure(UserCreation user)
            {
                UserRepoMock.Setup(m => m.CreateUser(It.IsAny<User>()))
                    .Returns<Task<User>>(null);
                IdentityProviderServiceMock.Setup(m => m.RemoveUser(user))
                    .ReturnsAsync(false);
            }

            public void ExpectGetRestaurantResult(int restaurantId, RestaurantProfile expected)
            {
                RestaurantLogicMock.Setup(m => m.GetRestaurant(restaurantId))
                    .ReturnsAsync(expected);
            }

            public void ExpectGetUserResult(int userId, UserProfile expected)
            {
                UserRepoMock.Setup(m => m.GetUser(userId))
                    .ReturnsAsync(expected);
            }

            public void ExpectGetCurrentUser(UserProfile expected) 
            {
                AuthServiceMock.Setup(m => m.GetCurrentlyAuthenticatedUser())
                    .Returns(expected);
            }

            public void ExpectPermissionsCheck(RestaurantProfile restaurantProfile, UserProfile userProfile, bool expected)
            {
                PermissionLogicMock.Setup(m => m.CanModify(restaurantProfile, userProfile))
                    .Returns(expected);
            }

            public void ExpectBanSave(int expectedUserId, int expectedRestaurantId, string reason, int expectedCreatedUserId)
            {
                UserRepoMock.Setup(m => m.BanUserFromRestaurant(It.Is<UserRestaurantBan>(
                    u => u.RestaurantId == expectedRestaurantId
                    && u.Reason == reason
                    && u.UserId == expectedUserId)))
                    .ReturnsAsync(true);
            }

            public void Dispose()
            {
                _mockRepo.VerifyAll();
            }
        }
    }
}
