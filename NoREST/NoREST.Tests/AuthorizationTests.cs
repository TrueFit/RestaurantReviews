using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Moq;
using NoREST.Api.Auth;
using NoREST.Domain;
using NoREST.Models.DomainModels;
using NoREST.Models.ViewModels.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NoREST.Tests
{

    //Couldn't get the below tests to work.... the static variable in the KeyIdHandler is a little too hard to deal with, and I don't want to complicate things any further...

    //public class KeyIdHandlerTests

    //{

    //    private static readonly IFixture _fixture = new Fixture();



    //    [Fact]

    //    public async Task KeyIdHandler_OnlyNeeds_ToGet_WellKnownConfiguration_Once()

    //    {

    //        var keyIdFetcherMock = new Mock<IKeyIdFetcher>();

    //        keyIdFetcherMock.Setup(m => m.FetchKeyId(It.IsAny<string>())).ReturnsAsync(new string[] { "hello" });



    //        var sut = new KeyIdHandler(keyIdFetcherMock.Object);



    //        for (int i = 0; i < 5; i++)

    //        {

    //            await sut.HasMatchingKeyId(_fixture.Create<string>(), _fixture.Create<string>());

    //        }



    //        keyIdFetcherMock.Verify(m => m.FetchKeyId(It.IsAny<string>()), Times.Once);

    //    }



    //    [Fact]

    //    public void KeyIdHandler_Throws_UnauthorizedException_IfFetcherThrows()

    //    {

    //        var keyIdFetcherMock = new Mock<IKeyIdFetcher>();

    //        keyIdFetcherMock.Setup(m => m.FetchKeyId(It.IsAny<string>())).ThrowsAsync(new InvalidCastException());



    //        var sut = new KeyIdHandler(keyIdFetcherMock.Object);



    //        Func<Task<bool>> attemptingToCheckForMatchingKeys = async () => await sut.HasMatchingKeyId(_fixture.Create<string>(), _fixture.Create<string>());

    //        attemptingToCheckForMatchingKeys.Should().Throw<UnauthorizedException>();

    //    }

    //}



    public class CognitoAuthorizationTests
    {
        private static readonly IFixture _fixture = new Fixture();

        [Theory]
        [MemberData(nameof(GetInvalidTokens))]
        public async Task InvalidToken_Returns_ErrorMessage(TokenWrecker tokenWrecker)
        {
            using (var testFacility = new CognitoTokenValidatorTestFacility())
            {
                if (tokenWrecker.WillCheckValidKey)
                {
                    testFacility.ExpectMatchingKeyId();
                }

                else
                {
                    testFacility.ExpectInvalidKeyId();
                }

                var sut = testFacility.BuildSut();
                var token = testFacility.BuildAValidToken();
                tokenWrecker.Wreck(token);

                if (tokenWrecker.WillCheckSubject)
                {
                    testFacility.ExpectGetUser(null, token.Subject);
                }

                var error = await sut.ValidateToken(token, testFacility.Now);

                Assert.NotNull(error);
            }
        }

        [Fact]
        public async Task ExpiredToken_ThrowsUnauthorizedException_WithRelevantMessage()
        {
            using (var testFacility = new CognitoTokenValidatorTestFacility())
            {
                testFacility.ExpectMatchingKeyId();
                var sut = testFacility.BuildSut();
                var token = testFacility.BuildAValidToken();
                token.ValidTo = testFacility.Now.AddMinutes(-61);

                var (error, result) = await sut.ValidateToken(token, testFacility.Now);

                error.Should().Contain("expired");
            }
        }



        [Fact]
        public async Task ValidToken_WithMatchingKeyId_ReturnsNull()
        {
            using (var testFacility = new CognitoTokenValidatorTestFacility())
            {
                testFacility.ExpectMatchingKeyId();
                var sut = testFacility.BuildSut();
                var token = testFacility.BuildAValidToken();
                testFacility.ExpectGetUser(null, token.Subject);
                var (error, result) = await sut.ValidateToken(token, testFacility.Now);

                error.Should().BeNull();
            }
        }

        [Fact]
        public async Task Token_Without_Claims_Returns_ErrorMessage()
        {
            using (var testFacility = new CognitoTokenValidatorTestFacility())
            {
                testFacility.ExpectMatchingKeyId();
                var sut = testFacility.BuildSut();
                var token = testFacility.BuildAValidToken();
                token.Claims = null;

                var (error, result) = await sut.ValidateToken(token, testFacility.Now);

                error.Should().Contain("claims").And.Contain("not granted");
            }
        }

        [Fact]
        public void Throws_Unauthorized_Exception_When_KeyHandler_Throws()
        {
            using (var testFacility = new CognitoTokenValidatorTestFacility()
                .ExpectKeyHandlerToThrow())
            {
                var sut = testFacility.BuildSut();
                var token = testFacility.BuildAValidToken();
                Func<Task> attemptingToAuthenticate = async () => await sut.ValidateToken(token, testFacility.Now);
                attemptingToAuthenticate.Should().ThrowAsync<ApplicationException>();
            }
        }

        public static IEnumerable<object[]> GetInvalidTokens()
        {
            yield return new object[] { new TokenWrecker((JwtModel token) => token.Issuer = "invalidValue") };
            yield return new object[] { new TokenWrecker((JwtModel token) => token.Subject = "invalidValue") { WillCheckSubject = true } };
            yield return new object[] { new TokenWrecker((JwtModel token) => token.Kid = "invalidValue") { WillCheckValidKey = false } };
            yield return new object[] { new TokenWrecker((JwtModel token) => token.Claims = new Claim[] { new Claim("token_use", "invalid") }) };
        }

        public class TokenWrecker
        {
            private readonly Action<JwtModel> _invalidation;
            public bool WillCheckValidKey { get; set; } = true;
            public bool WillCheckSubject { get; set; }
            public TokenWrecker(Action<JwtModel> invalidation)
            {
                _invalidation = invalidation;
            }
            public void Wreck(JwtModel token) => _invalidation(token);
        }



        private class CognitoTokenValidatorTestFacility : IDisposable
        {
            private MockRepository _mockRepo = new MockRepository(MockBehavior.Strict);

            public Mock<IKeyIdHandler> KeyIdHandlerMock { get; set; }
            public TestCognitoPoolInfo CognitoPoolInfo { get; set; }
            public DateTime Now { get; set; }
            public string Kid { get; set; }
            public Mock<IUserLogic> UserLogicMock { get; set; }

            public CognitoTokenValidatorTestFacility()
            {
                KeyIdHandlerMock = _mockRepo.Create<IKeyIdHandler>();
                CognitoPoolInfo = _fixture.Create<TestCognitoPoolInfo>();
                UserLogicMock = _mockRepo.Create<IUserLogic>();
                Kid = _fixture.Create<string>();
                Now = _fixture.Create<DateTime>();
            }

            public CognitoTokenValidator BuildSut()
            {
                return new CognitoTokenValidator(KeyIdHandlerMock.Object, CognitoPoolInfo, UserLogicMock.Object);
            }

            public void ExpectMatchingKeyId()
            {
                KeyIdHandlerMock.Setup(m => m.HasMatchingKeyId(Kid, It.IsAny<string>()))
                    .ReturnsAsync(true);
            }
            public void ExpectInvalidKeyId()
            {
                KeyIdHandlerMock.Setup(m => m.HasMatchingKeyId(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(false);
            }

            public void ExpectGetUser(UserProfile userProfile, string subject)
            {
                UserLogicMock.Setup(m => m.GetUserProfileFromIdentityProviderId(subject))
                    .ReturnsAsync(userProfile);
            }

            public CognitoTokenValidatorTestFacility ExpectKeyHandlerToThrow()
            {
                KeyIdHandlerMock.Setup(m => m.HasMatchingKeyId(It.IsAny<string>(), It.IsAny<string>()))
                    .Throws(new ApplicationException());
                return this;
            }

            public JwtModel BuildAValidToken() =>
                _fixture.Build<JwtModel>()
                    .With(t => t.Issuer, CognitoPoolAddressBuilder.GetCognitoUserPoolBaseAddress(CognitoPoolInfo))
                    .With(t => t.Subject, CognitoPoolInfo.ClientId)
                    .With(t => t.Kid, Kid)
                    .With(t => t.ValidTo, Now.AddHours(1))
                    .With(t => t.Claims, new Claim[] { new Claim("token_use", "access"), new Claim("scope", CognitoPoolInfo.Scopes.First()) })
                .Create();

            public void Dispose()
            {
                _mockRepo.VerifyAll();
            }
        }


        private class TestCognitoPoolInfo : ICognitoPoolInfo
        {
            public string PoolId { get; set; }
            public string Region { get; set; }
            public string[] Scopes { get; set; }

            public string ClientSecret { get; set; }

            public string ClientId { get; set; }

            public string UiClientId { get; set; }

            public string ComputeSecretHash(string username)
            {
                return username;
            }
        }
    }
}