using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ProviderRelationships.Api.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.AppStart;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(IAzureClientCredentialHelper))]
    [TestCase(typeof(IAccountsApiClient<AccountsConfiguration>))]
    [TestCase(typeof(IEmployerUsersApiClient<EmployerUsersApiConfiguration>))]
    [TestCase(typeof(IEmployerAccountsService))]
    public void Then_The_Dependencies_Are_Correctly_Resolved(Type toResolve)
    {
        var hostEnvironment = new Mock<IWebHostEnvironment>();
        var serviceCollection = new ServiceCollection();

        var configuration = GenerateConfiguration();
        serviceCollection.AddSingleton(hostEnvironment.Object);
        serviceCollection.AddSingleton(Mock.Of<IConfiguration>());
        serviceCollection.AddConfigurationOptions(configuration);
        serviceCollection.AddDistributedMemoryCache();
        serviceCollection.AddServiceRegistration();

        var provider = serviceCollection.BuildServiceProvider();

        var type = provider.GetService(toResolve);
        Assert.IsNotNull(type);
    }

    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("AccountsInnerApiConfiguration:url", "http://localhost:1"),
                new KeyValuePair<string, string>("EmployerUsersApiConfiguration:url", "http://localhost:3")
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}