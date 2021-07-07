using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.HealthChecks
{
    public class WhenCheckingAccountsApiHealth
    {
        [Test, MoqAutoData]
        public async Task Then_Then_The_Service_Is_Called_And_Healthy_Returned_If_True(
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> apiClient,
            HealthCheckContext context,
            AccountsApiHealthCheck healthCheck)
        {
            apiClient.Setup(x => x.GetResponseCode(It.IsAny<GetAccountsApiPingRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);

            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Healthy);
        }

        [Test, MoqAutoData]
        public async Task Then_Then_The_Service_Is_Called_And_UnHealthy_Returned_If_False(
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> apiClient,
            HealthCheckContext context,
            AccountsApiHealthCheck healthCheck)
        {
            apiClient.Setup(x => x.GetResponseCode(It.IsAny<GetAccountsApiPingRequest>()))
                .ReturnsAsync(HttpStatusCode.InternalServerError);

            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}