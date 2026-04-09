using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure.Authentication;
using SFA.DAS.SharedOuterApi.Infrastructure.DfeSignIn;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.DfeSignIn
{
    [TestFixture]
    public class DfeSignInApiClientTests
    {
        [Test]
        public async Task Then_AddAuthenticationHeader_Sets_Bearer_Token_From_JwtProvider()
        {
            // arrange
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient()); 

            var cfg = new DfeSignInApiConfiguration
            {
                Url = "https://example.test",   
                Audience = "aud",
                ClientId = "iss",
                ApiSecret = "secret",
                TokenLifetimeMinutes = 5
            };

            var jwtProvider = new Mock<IDfeJwtProvider>();
            jwtProvider.Setup(x => x.CreateToken()).Returns("token-123");

            var sut = new TestDfeSignInApiClient(httpClientFactory.Object, cfg, jwtProvider.Object);

            var req = new HttpRequestMessage(HttpMethod.Get, "https://example.test/anything");

            // act
            await sut.CallAddAuthenticationHeader(req);

            // assert
            jwtProvider.Verify(x => x.CreateToken(), Times.Once);

            Assert.That(req.Headers.Authorization, Is.Not.Null);
            Assert.That(req.Headers.Authorization!.Scheme, Is.EqualTo("Bearer"));
            Assert.That(req.Headers.Authorization!.Parameter, Is.EqualTo("token-123"));
        }

        private sealed class TestDfeSignInApiClient : DfeSignInApiClient<DfeSignInApiConfiguration>
        {
            public TestDfeSignInApiClient(
                IHttpClientFactory httpClientFactory,
                DfeSignInApiConfiguration apiConfiguration,
                IDfeJwtProvider jwtProvider)
                : base(httpClientFactory, apiConfiguration, jwtProvider)
            {
            }

            public Task CallAddAuthenticationHeader(HttpRequestMessage request)
                => AddAuthenticationHeader(request);
        }
    }
}
