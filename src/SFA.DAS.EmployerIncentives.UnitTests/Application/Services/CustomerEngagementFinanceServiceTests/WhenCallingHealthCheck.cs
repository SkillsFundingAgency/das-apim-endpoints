using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.CustomerEngagementFinanceServiceTests
{
    [TestFixture]
    public class WhenCallingHealthCheck
    {
        [Test]
        public async Task And_Api_Is_Unavailable_Then_Health_Check_Should_Return_False()
        {
            ICustomerEngagementFinanceService sut = new CustomerEngagementFinanceService(null);

            var result = await sut.IsHealthy();
            result.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task And_Api_Is_Available_And_Returns_Ok_Then_Health_Check_Should_Return_True(
            [Frozen] Mock<ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration>> client,
            [Greedy] CustomerEngagementFinanceService sut)
        {
            client.Setup(x => x.GetResponseCode(It.Is<IGetApiRequest>(p => p.GetUrl == "finance/heartbeat")))
                .ReturnsAsync(HttpStatusCode.OK);

            var result = await sut.IsHealthy();
            result.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task And_Api_Is_Available_And_Does_Not_Returns_Ok_Then_Health_Check_Should_Return_False(
            [Frozen] Mock<ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration>> client,
            [Greedy] CustomerEngagementFinanceService sut)
        {
            client.Setup(x => x.GetResponseCode(It.Is<IGetApiRequest>(p => p.GetUrl == "finance/heartbeat")))
                .ReturnsAsync(HttpStatusCode.NotFound);

            var result = await sut.IsHealthy();
            result.Should().BeFalse();
        }
    }
}
