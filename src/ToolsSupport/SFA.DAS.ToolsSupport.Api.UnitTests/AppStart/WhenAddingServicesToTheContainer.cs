using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.AppStart;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(IAzureClientCredentialHelper))]
    [TestCase(typeof(IInternalApiClient<CommitmentsV2ApiConfiguration>))]
    [TestCase(typeof(IInternalApiClient<AccountsConfiguration>))]
    [TestCase(typeof(IInternalApiClient<EmployerProfilesApiConfiguration>))]
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

    private static ConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
                {
                    new("CommitmentsV2InnerApi:url", "http://localhost:1"),
                    new("AccountsInnerApi:url", "http://localhost:2"),
                    new("EmployerProfilesInnerApi:url", "http://localhost:3"),
                    new("ResourceEnvironmentName", "TEST"),
                }!
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}