﻿using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.AppStart;
public class WhenAddingServicesToTheContainer
{

    [TestCase(typeof(IAzureClientCredentialHelper))]
    [TestCase(typeof(IRecruitApiClient<RecruitApiConfiguration>))]
    [TestCase(typeof(ICoursesApiClient<CoursesApiConfiguration>))]
    [TestCase(typeof(ILocationApiClient<LocationApiConfiguration>))]
    [TestCase(typeof(ICandidateApiClient<CandidateApiConfiguration>))]
    [TestCase(typeof(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>))]
    [TestCase(typeof(INhsJobsApiClient))]
    [TestCase(typeof(ICourseService))]
    [TestCase(typeof(EmailEnvironmentHelper))]
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
                new("RecruitApiConfiguration:url", "http://localhost:1"),
                new("CoursesApiConfiguration:url", "http://localhost:2"),
                new("LocationApiConfiguration:url", "http://localhost:3"),
                new("CandidateApiConfiguration:url", "http://localhost:4"),
                new("FindApprenticeshipApiConfiguration:url", "http://localhost:5"),
                new("ResourceEnvironmentName", "TEST"),
            }!
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}
