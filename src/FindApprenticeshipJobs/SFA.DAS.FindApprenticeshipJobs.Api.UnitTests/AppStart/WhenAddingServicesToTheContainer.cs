using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using FluentAssertions;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.AppStart;
public class WhenAddingServicesToTheContainer
{

    [TestCase(typeof(IAzureClientCredentialHelper))]
    [TestCase(typeof(IRecruitApiClient<RecruitApiConfiguration>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved(Type toResolve)
    {
        var hostEnvironment = new Mock<IWebHostEnvironment>();
        var serviceCollection = new ServiceCollection();

        var configuration = GenerateConfiguration();
        serviceCollection.AddSingleton(hostEnvironment.Object);
        serviceCollection.AddSingleton(Mock.Of<IConfiguration>());
        Api.AppStart.AddConfigurationOptionsExtension.AddConfigurationOptions(serviceCollection, configuration);
        serviceCollection.AddDistributedMemoryCache();
        Api.AppStart.ServiceCollectionExtensions.AddServiceRegistration(serviceCollection, configuration);

        var provider = serviceCollection.BuildServiceProvider();
        var type = provider.GetService(toResolve);

        type.Should().NotBeNull();
    }

    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("RecruitApiConfiguration:url", "http://localhost:1"),
                }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}
