using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.FindAnApprenticeship.Api.AppStart;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.AppStart
{
    public class WhenAddingServicesToTheContainer
    {
        [TestCase(typeof(IAzureClientCredentialHelper))]
        [TestCase(typeof(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>))]
        [TestCase(typeof(ILocationApiClient<LocationApiConfiguration>))]
        [TestCase(typeof(ICoursesApiClient<CoursesApiConfiguration>))]
        [TestCase(typeof(IRecruitApiClient<RecruitApiV2Configuration>))]
        [TestCase(typeof(ICourseService))]
        [TestCase(typeof(ILocationLookupService))]
        [TestCase(typeof(IVacancyService))]
        [TestCase(typeof(EmailEnvironmentHelper))]
        public void Then_The_Dependencies_Are_Correctly_Resolved(Type toResolve)
        {
            var hostEnvironment = new Mock<IWebHostEnvironment>();
            var serviceCollection = new ServiceCollection();
            
            var configuration = GenerateConfiguration();
            serviceCollection.AddSingleton(hostEnvironment.Object);
            serviceCollection.AddSingleton(Mock.Of<IConfiguration>());
            serviceCollection.AddConfigurationOptions(configuration);
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddServiceRegistration(configuration);
            
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
                    new KeyValuePair<string, string>("LocationApiConfiguration:url", "http://localhost:3"),
                    new KeyValuePair<string, string>("RecruitAltApiConfiguration:url", "http://localhost:5"),
                    new KeyValuePair<string, string>("ResourceEnvironmentName", "DEV")
                }
            };

            var provider = new MemoryConfigurationProvider(configSource);

            return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
        }
    }
}