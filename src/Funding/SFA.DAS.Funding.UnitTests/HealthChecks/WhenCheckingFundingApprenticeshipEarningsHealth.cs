using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Infrastructure;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.UnitTests.HealthChecks
{
    public class WhenCheckingFundingApprenticeshipEarningsHealth
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Healthy_Returned_If_True(
            [Frozen] Mock<IFundingApprenticeshipEarningsService> fundingService,
            HealthCheckContext context,
            FundingApprenticeshipEarningsHealthCheck healthCheck)
        {
            fundingService.Setup(x => x.IsHealthy()).ReturnsAsync(true);
            
            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Healthy);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_UnHealthy_Returned_If_False(
            [Frozen] Mock<IFundingApprenticeshipEarningsService> fundingService,
            HealthCheckContext context,
            FundingApprenticeshipEarningsHealthCheck healthCheck)
        {
            fundingService.Setup(x => x.IsHealthy()).ReturnsAsync(false);
            
            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}