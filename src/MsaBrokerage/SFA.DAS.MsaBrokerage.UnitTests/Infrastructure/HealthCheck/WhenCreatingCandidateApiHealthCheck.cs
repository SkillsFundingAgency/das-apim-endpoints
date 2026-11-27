using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.MsaBrokerage.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.MsaBrokerage.UnitTests.Infrastructure.HealthCheck;

public class WhenCreatingCandidateApiHealthCheck
{
    [Test]
    public void Then_The_Health_Check_Is_Created()
    {
        var actual = new CandidateApiHealthCheck(
            Mock.Of<ICandidateApiClient<CandidateApiConfiguration>>(),
            Mock.Of<ILogger<CandidateApiHealthCheck>>());

        actual.Should().NotBeNull();
    }

    [Test]
    public void Then_The_Descriptions_Are_Set()
    {
        CandidateApiHealthCheck.HealthCheckDescription.Should().Be("Candidate Account API");
        CandidateApiHealthCheck.HealthCheckResultDescription.Should().Be("Candidate Account API check");
    }
}
