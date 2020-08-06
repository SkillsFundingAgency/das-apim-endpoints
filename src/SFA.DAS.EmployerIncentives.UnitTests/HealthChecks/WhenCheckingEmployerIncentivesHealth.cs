using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.HealthChecks
{
    public class WhenCheckingEmployerIncentivesHealth
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Healthy_Returned_If_True(
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            HealthCheckContext context,
            EmployerIncentivesHealthCheck healthCheck)
        {
            employerIncentivesService.Setup(x => x.IsHealthy()).ReturnsAsync(true);
            
            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Healthy);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_UnHealthy_Returned_If_False(
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            HealthCheckContext context,
            EmployerIncentivesHealthCheck healthCheck)
        {
            employerIncentivesService.Setup(x => x.IsHealthy()).ReturnsAsync(false);
            
            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}