using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmploymentCheck.Configuration;
using SFA.DAS.EmploymentCheck.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmploymentCheck.Api.UnitTests.HealthChecks
{
    public class WhenCheckingEmploymentCheckHealth
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Healthy_Returned_If_True(
            [Frozen] Mock<IInternalApiClient<EmploymentCheckConfiguration>> employmentCheckApiClient,
            HealthCheckContext context,
            EmploymentCheckApiHealthCheck healthCheck)
        {
            employmentCheckApiClient.Setup(x => 
                x.GetResponseCode(It.IsAny<GetPingRequest>())).ReturnsAsync(HttpStatusCode.OK);
            
            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Healthy);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_UnHealthy_Returned_If_False(
            [Frozen] Mock<IInternalApiClient<EmploymentCheckConfiguration>> employmentCheckApiClient,
            HealthCheckContext context,
            EmploymentCheckApiHealthCheck healthCheck)
        {
            employmentCheckApiClient.Setup(x =>
                x.GetResponseCode(It.IsAny<GetPingRequest>())).ReturnsAsync(HttpStatusCode.BadGateway);

            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}