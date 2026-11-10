using Microsoft.Extensions.Logging.Abstractions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace SFA.DAS.SharedOuterApi.Types.UnitTests.Infrastructure.AccessTokenApi;

[TestFixture]
public class WhenCallingGetWithAnAccessTokenClient
{
    private const string TokenUrl = "https://login.example.test/tenant-id/oauth2/v2.0/token";
    private const string AccessToken = "the-access-token";

    private RecordingHandler _handler = null!;
    private TestAccessTokenApiConfiguration _config = null!;
    private ManualTimeProvider _timeProvider = null!;
    private string _tokenResponseBody = null!;
    private AccessTokenApiClient<TestAccessTokenApiConfiguration> _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _timeProvider = new ManualTimeProvider(new DateTimeOffset(2026, 9, 29, 12, 0, 0, TimeSpan.Zero));
        _tokenResponseBody = $"{{\"access_token\":\"{AccessToken}\",\"expires_in\":3600}}";

        // The token cache is static, so each test uses its own client id to stay isolated.
        _config = new TestAccessTokenApiConfiguration
        {
            Url = "https://api.example.test/",
            TokenSettings = new AccessTokenProviderApiConfiguration
            {
                Url = "https://login.example.test/",
                Tenant = "tenant-id/oauth2/v2.0/token",
                ClientId = $"client-id-{Guid.NewGuid()}",
                ClientSecret = "client-secret",
                Scope = "api://api-id/.default"
            }
        };

        _handler = new RecordingHandler(request =>
            request.RequestUri!.AbsoluteUri == TokenUrl
                ? Json(HttpStatusCode.OK, _tokenResponseBody)
                : Json(HttpStatusCode.OK, "\"ok\""));

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(() => new HttpClient(_handler, disposeHandler: false));

        _sut = new AccessTokenApiClient<TestAccessTokenApiConfiguration>(
            NullLogger<AccessTokenApiClient<TestAccessTokenApiConfiguration>>.Instance,
            httpClientFactory.Object,
            _config,
            _timeProvider);
    }

    [Test]
    public async Task Then_the_token_is_requested_with_a_POST_to_the_token_endpoint()
    {
        await _sut.Get<string>(new TestGetRequest());

        var tokenRequest = TokenRequests.Single();
        tokenRequest.Method.Should().Be(HttpMethod.Post);
        tokenRequest.Uri.Should().Be(TokenUrl);
    }

    [Test]
    public async Task Then_the_token_request_is_a_client_credentials_form()
    {
        await _sut.Get<string>(new TestGetRequest());

        var form = TokenRequests.Single().Body;
        form.Should().Contain($"client_id={_config.TokenSettings.ClientId}");
        form.Should().Contain("client_secret=client-secret");
        form.Should().Contain("scope=api%3A%2F%2Fapi-id%2F.default");
        form.Should().Contain("grant_type=client_credentials");
    }

    [Test]
    public async Task Then_the_api_request_carries_the_bearer_token()
    {
        await _sut.Get<string>(new TestGetRequest());

        var apiRequest = ApiRequests.Single();
        apiRequest.Uri.Should().Be("https://api.example.test/some/path");
        apiRequest.Authorization.Should().Be($"Bearer {AccessToken}");
    }

    [Test]
    public async Task Then_the_token_is_reused_for_subsequent_calls_while_it_is_valid()
    {
        await _sut.Get<string>(new TestGetRequest());
        _timeProvider.Advance(TimeSpan.FromMinutes(30));
        await _sut.Get<string>(new TestGetRequest());

        TokenRequests.Should().HaveCount(1);
        ApiRequests.Should().HaveCount(2);
        ApiRequests.Should().OnlyContain(x => x.Authorization == $"Bearer {AccessToken}");
    }

    [Test]
    public async Task Then_a_new_token_is_requested_shortly_before_the_current_one_expires()
    {
        await _sut.Get<string>(new TestGetRequest());
        _timeProvider.Advance(TimeSpan.FromSeconds(3600 - 30)); // inside the 60 second safety margin
        await _sut.Get<string>(new TestGetRequest());

        TokenRequests.Should().HaveCount(2);
    }

    [Test]
    public async Task Then_the_token_is_not_cached_when_the_response_has_no_expiry()
    {
        _tokenResponseBody = $"{{\"access_token\":\"{AccessToken}\"}}";

        await _sut.Get<string>(new TestGetRequest());
        await _sut.Get<string>(new TestGetRequest());

        TokenRequests.Should().HaveCount(2);
    }

    private IEnumerable<RecordedRequest> TokenRequests => _handler.Requests.Where(x => x.Uri == TokenUrl);
    private IEnumerable<RecordedRequest> ApiRequests => _handler.Requests.Where(x => x.Uri != TokenUrl);

    private static HttpResponseMessage Json(HttpStatusCode status, string body) =>
        new(status) { Content = new StringContent(body, Encoding.UTF8, "application/json") };

    private class TestAccessTokenApiConfiguration : IAccessTokenApiConfiguration
    {
        public string Url { get; set; } = string.Empty;
        public AccessTokenProviderApiConfiguration TokenSettings { get; set; } = new();
    }

    private class TestGetRequest : IGetApiRequest
    {
        public string Version => "1.0";
        public string GetUrl => "some/path";
    }

    private class ManualTimeProvider(DateTimeOffset start) : TimeProvider
    {
        private DateTimeOffset _now = start;

        public override DateTimeOffset GetUtcNow() => _now;

        public void Advance(TimeSpan by) => _now += by;
    }

    private record RecordedRequest(HttpMethod Method, string Uri, string Body, string? Authorization);

    private class RecordingHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        public List<RecordedRequest> Requests { get; } = [];

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var body = request.Content == null ? string.Empty : await request.Content.ReadAsStringAsync(cancellationToken);
            Requests.Add(new RecordedRequest(request.Method, request.RequestUri!.AbsoluteUri, body, request.Headers.Authorization?.ToString()));
            return respond(request);
        }
    }
}
