using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.Api.AppStart;

namespace SFA.DAS.Vacancies.Api.UnitTests.AppStart
{
    public class WhenAddingServicesToTheContainer
    {
        [TestCase(typeof(IAzureClientCredentialHelper))]
        [TestCase(typeof(ICoursesApiClient<CoursesApiConfiguration>))]
        [TestCase(typeof(ICacheStorageService))]
        [TestCase(typeof(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>))]
        [TestCase(typeof(IAccountsApiClient<AccountsConfiguration>))]
        [TestCase(typeof(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>))]
        [TestCase(typeof(IAccountLegalEntityPermissionService))]
        [TestCase(typeof(ICourseService))]
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
            Assert.That(type, Is.Not.Null);
        }
        
        private static IConfigurationRoot GenerateConfiguration()
        {
            var configSource = new MemoryConfigurationSource
            {
                InitialData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("CoursesApiConfiguration:url", "http://localhost:1"),
                    new KeyValuePair<string, string>("FindApprenticeshipApiConfiguration:url", "http://localhost:2"),
                    new KeyValuePair<string, string>("AccountsInnerApi:url", "http://localhost:3"),
                    new KeyValuePair<string, string>("ProviderRelationshipsApi:url", "http://localhost:4")
                }
            };

            var provider = new MemoryConfigurationProvider(configSource);

            return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
        }
    }
}