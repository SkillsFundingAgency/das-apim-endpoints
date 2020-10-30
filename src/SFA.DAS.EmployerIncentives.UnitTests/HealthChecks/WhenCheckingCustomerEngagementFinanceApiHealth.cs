using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CustomerEngagementFinance;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.HealthChecks
{
    public class WhenCheckingCustomerEngagementFinanceApiHealth
    {
        [Test, MoqAutoData]
        public async Task Then_Then_The_Service_Is_Called_And_Healthy_Returned_If_True(
            [Frozen] Mock<ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration>> apiClient,
            HealthCheckContext context,
            CustomerEngagementFinanceApiHealthCheck healthCheck)
        {
            apiClient.Setup(x => x.GetResponseCode(It.IsAny<GetCustomerEngagementFinanceHeartbeatRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);

            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Healthy);
        }

        [Test, MoqAutoData]
        public async Task Then_Then_The_Service_Is_Called_And_UnHealthy_Returned_If_False(
            [Frozen] Mock<ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration>> apiClient,
            HealthCheckContext context,
            CustomerEngagementFinanceApiHealthCheck healthCheck)
        {
            apiClient.Setup(x => x.GetResponseCode(It.IsAny<GetCustomerEngagementFinanceHeartbeatRequest>()))
                .ReturnsAsync(HttpStatusCode.InternalServerError);

            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}