using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.AppStart;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

namespace SFA.DAS.LearnerData.Api.UnitTests.AppStart;

[TestFixture]
public class AddServicesTests
{
    private ServiceProvider _provider = null!;

    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{nameof(CommitmentsV2ApiConfiguration)}:Url"] = "https://commitments.example",
                [$"{nameof(CommitmentsV2ApiConfiguration)}:Identifier"] = "identifier"
            })
            .Build();

        services.AddLogging();
        services.AddConfigurationOptions(configuration);
        services.AddServices();

        _provider = services.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown() => _provider.Dispose();

    [Test]
    public void ThenTheApprovalsServiceCanBeResolved()
    {
        _provider.GetRequiredService<IApprovalsService>().Should().BeOfType<ApprovalsService>();
    }

    [Test]
    public void ThenTheApprovalsRequestBuilderCanBeResolved()
    {
        _provider.GetRequiredService<IApprovalsRequestBuilder>().Should().BeOfType<ApprovalsRequestBuilder>();
    }
}
