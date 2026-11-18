using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Assessor.Api.AppStart;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.Assessor.Api.UnitTests.AppStart
{
    public class WhenRegisteringAssessorServices
    {
        private ServiceCollection _sut;
        private IConfiguration _configuration;

        [SetUp]
        public void SetUp()
        {
            _sut = new ServiceCollection();

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["AssessorApiConfiguration:Url"] =
                        "https://assessor-inner.example/",

                    ["AssessorApiConfiguration:Identifier"] =
                        "api://assessor-inner"
                })
                .Build();
        }

        [Test]
        public void Then_The_Inner_Api_Configuration_Is_Bound()
        {
            _sut.AddConfigurationOptions(_configuration);

            using var provider = _sut.BuildServiceProvider();

            var actual = provider
                .GetRequiredService<AssessorApiConfiguration>();

            actual.Url.Should().Be("https://assessor-inner.example/");
            actual.Identifier.Should().Be("api://assessor-inner");
        }

        [Test]
        public void Then_The_Assessor_Client_And_Its_Dependencies_Can_Be_Resolved()
        {
            _sut.AddConfigurationOptions(_configuration);
            _sut.AddServiceRegistration();

            _sut.AddSingleton(
                Mock.Of<IAzureClientCredentialHelper>());

            using var provider = _sut.BuildServiceProvider();

            provider
                .GetRequiredService<
                    IAssessorApiClient<AssessorApiConfiguration>>()
                .Should()
                .BeOfType<AssessorApiClient>();

            provider
                .GetRequiredService<
                    IInternalApiClient<AssessorApiConfiguration>>()
                .Should()
                .BeOfType<InternalApiClient<AssessorApiConfiguration>>();
        }
    }
}