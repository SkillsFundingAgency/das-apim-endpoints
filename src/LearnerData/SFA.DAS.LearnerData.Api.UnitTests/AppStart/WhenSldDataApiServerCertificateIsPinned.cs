using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.AppStart;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Requests.SldDataApi;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.Api.UnitTests.AppStart;

[TestFixture]
public class WhenSldDataApiServerCertificateIsPinned
{
    private const string UnrelatedThumbprint = "0123456789ABCDEF0123456789ABCDEF01234567";

    private X509Certificate2 _serverCertificate = null!;
    private WebApplication _sldServer = null!;
    private int _port;

    [OneTimeSetUp]
    public async Task StartSldServer()
    {
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var request = new CertificateRequest("CN=sld-data-api-test", key, HashAlgorithmName.SHA256);
        using var selfSigned = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));
        _serverCertificate = X509CertificateLoader.LoadPkcs12(selfSigned.Export(X509ContentType.Pfx), null);

        var builder = WebApplication.CreateBuilder();
        builder.WebHost.ConfigureKestrel(options =>
            options.Listen(IPAddress.Loopback, 0, listen => listen.UseHttps(_serverCertificate)));
        _sldServer = builder.Build();
        _sldServer.MapGet("/api/lars/get-em-value/{learnAimRef}/{startDate}", () => Results.Json(123.45m));
        await _sldServer.StartAsync();

        _port = new Uri(_sldServer.Urls.First()).Port;
    }

    [OneTimeTearDown]
    public async Task StopSldServer()
    {
        await _sldServer.DisposeAsync();
        _serverCertificate.Dispose();
    }

    [Test]
    public async Task Then_a_server_certificate_matching_the_configured_thumbprint_is_accepted_even_though_it_is_self_signed()
    {
        using var provider = BuildProvider(_serverCertificate.Thumbprint);
        var client = provider.GetRequiredService<ISldDataApiClient<SLDDataApiConfiguration>>();

        var response = await client.GetWithResponseCode<decimal?>(NewRequest());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Body.Should().Be(123.45m);
    }

    [Test]
    public async Task Then_a_server_certificate_not_matching_any_configured_thumbprint_is_rejected()
    {
        using var provider = BuildProvider(UnrelatedThumbprint);
        var client = provider.GetRequiredService<ISldDataApiClient<SLDDataApiConfiguration>>();

        await FluentActions.Awaiting(() => client.GetWithResponseCode<decimal?>(NewRequest()))
            .Should().ThrowAsync<HttpRequestException>();
    }

    [Test]
    public async Task Then_a_server_certificate_is_rejected_when_no_thumbprints_are_configured()
    {
        using var provider = BuildProvider();
        var client = provider.GetRequiredService<ISldDataApiClient<SLDDataApiConfiguration>>();

        await FluentActions.Awaiting(() => client.GetWithResponseCode<decimal?>(NewRequest()))
            .Should().ThrowAsync<HttpRequestException>();
    }

    private ServiceProvider BuildProvider(params string[] thumbprints)
    {
        var settings = new Dictionary<string, string?>
        {
            ["SLDDataApiConfiguration:Url"] = $"https://127.0.0.1:{_port}/",
            ["SLDDataApiConfiguration:TokenSettings:Url"] = "https://login.example.test/",
            ["SLDDataApiConfiguration:TokenSettings:ShouldSkipForLocal"] = "true"
        };

        for (var i = 0; i < thumbprints.Length; i++)
        {
            settings[$"SLDDataApiConfiguration:CertificateThumbprints:{i}"] = thumbprints[i];
        }

        var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddConfigurationOptions(configuration);
        services.AddServices();

        return services.BuildServiceProvider();
    }

    private static GetEnglishAndMathsValueRequest NewRequest() => new("ZPROG001", new DateOnly(2025, 8, 1));
}
