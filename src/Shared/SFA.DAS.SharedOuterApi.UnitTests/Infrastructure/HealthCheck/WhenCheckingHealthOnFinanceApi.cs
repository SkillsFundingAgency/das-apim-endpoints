using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.HealthCheck
{
    public class WhenCheckingHealthOnFinanceApi
    {
        [Test, MoqAutoData]
        public async Task Then_Then_The_Service_Is_Called_And_Healthy_Returned_If_True(
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> apiClient,
            HealthCheckContext context,
            FinanceApiHealthCheck healthCheck)
        {
            apiClient.Setup(x => x.GetResponseCode(It.IsAny<GetPingRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);

            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Healthy);
        }

        [Test, MoqAutoData]
        public async Task Then_Then_The_Service_Is_Called_And_UnHealthy_Returned_If_False(
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> apiClient,
            HealthCheckContext context,
            FinanceApiHealthCheck healthCheck)
        {
            apiClient.Setup(x => x.GetResponseCode(It.IsAny<GetPingRequest>()))
                .ReturnsAsync(HttpStatusCode.InternalServerError);

            var actual = await healthCheck.CheckHealthAsync(context, CancellationToken.None);

            actual.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}