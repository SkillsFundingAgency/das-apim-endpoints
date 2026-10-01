using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.AppStart;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.Api.UnitTests.AppStart;

[TestFixture]
public class WhenRegisteringSldDataApiServices
{
    private ServiceProvider _provider = null!;

    [SetUp]
    public void SetUp()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SLDDataApiConfiguration:Url"] = "https://sld-data-api.example.test/",
                ["SLDDataApiConfiguration:TokenSettings:Url"] = "https://login.example.test/",
                ["SLDDataApiConfiguration:TokenSettings:Tenant"] = "tenant-id/oauth2/v2.0/token",
                ["SLDDataApiConfiguration:TokenSettings:ClientId"] = "client-id",
                ["SLDDataApiConfiguration:TokenSettings:ClientSecret"] = "client-secret",
                ["SLDDataApiConfiguration:TokenSettings:Scope"] = "api://sld-id/.default",
                ["SLDDataApiConfiguration:TokenSettings:ShouldSkipForLocal"] = "true",
                ["SLDDataApiConfiguration:CertificateThumbprints:0"] = "0123456789ABCDEF0123456789ABCDEF01234567",
                ["SLDDataApiConfiguration:CertificateThumbprints:1"] = "76543210FEDCBA9876543210FEDCBA9876543210"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddConfigurationOptions(configuration);
        services.AddServices();

        _provider = services.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown() => _provider.Dispose();

    [Test]
    public void Then_the_configuration_is_bound_from_the_SLDDataApiConfiguration_section()
    {
        var config = _provider.GetRequiredService<SLDDataApiConfiguration>();

        config.Url.Should().Be("https://sld-data-api.example.test/");
        config.TokenSettings.Url.Should().Be("https://login.example.test/");
        config.TokenSettings.Tenant.Should().Be("tenant-id/oauth2/v2.0/token");
        config.TokenSettings.ClientId.Should().Be("client-id");
        config.TokenSettings.ClientSecret.Should().Be("client-secret");
        config.TokenSettings.Scope.Should().Be("api://sld-id/.default");
        config.TokenSettings.ShouldSkipForLocal.Should().BeTrue();
        config.CertificateThumbprints.Should().Equal(
            "0123456789ABCDEF0123456789ABCDEF01234567",
            "76543210FEDCBA9876543210FEDCBA9876543210");
    }

    [Test]
    public void Then_the_sld_data_api_client_can_be_resolved()
    {
        var client = _provider.GetService<ISldDataApiClient<SLDDataApiConfiguration>>();

        client.Should().NotBeNull();
    }
}
