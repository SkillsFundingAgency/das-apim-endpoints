using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.HealthChecks
{
    public class WhenCheckingCommitmentsHealth
    {
        [Test, MoqAutoData]
        public async Task Then_Then_The_Service_Is_Called_And_Healthy_Returned_If_True(
            [Frozen] Mock<ICommitmentsApiClient<CommitmentsConfiguration>> commitmentsApiClient,
            HealthCheckContext context,
            CommitmentsHealthCheck healthCheck)
        {
            commitmentsApiClient.Setup(x => x.GetResponseCode(It.IsAny<GetCommitmentsPingRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);
            
            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Healthy);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Then_The_Service_Is_Called_And_UnHealthy_Returned_If_False(
            [Frozen] Mock<ICommitmentsApiClient<CommitmentsConfiguration>> commitmentsApiClient,
            HealthCheckContext context,
            CommitmentsHealthCheck healthCheck)
        {
            commitmentsApiClient.Setup(x => x.GetResponseCode(It.IsAny<GetPingRequest>()))
                .ReturnsAsync(HttpStatusCode.InternalServerError);
            
            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}