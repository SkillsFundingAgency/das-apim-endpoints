using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmploymentCheck.Application.Services;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmploymentCheck.Api.UnitTests.HealthChecks
{
    public class WhenCheckingEmploymentCheckHealth
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Healthy_Returned_If_True(
            [Frozen] Mock<IEmploymentCheckService> employmentCheckService,
            HealthCheckContext context,
            EmploymentCheckApiHealthCheck healthCheck)
        {
            employmentCheckService.Setup(x => x.IsHealthy()).ReturnsAsync(true);
            
            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Healthy);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_UnHealthy_Returned_If_False(
            [Frozen] Mock<IEmploymentCheckService> employmentCheckService,
            HealthCheckContext context,
            EmploymentCheckApiHealthCheck healthCheck)
        {
            employmentCheckService.Setup(x => x.IsHealthy()).ReturnsAsync(false);
            
            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}