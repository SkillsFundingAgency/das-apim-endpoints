using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    [TestFixture] 
    public class WhenCallingHealthCheck
    {
        [Test]
        public async Task Then_Health_Check_Should_Return_False()
        {
            var sut = new EmployerIncentivesService(null);

            var result = await sut.IsHealthy();
            result.Should().Be(false);
        }

        [Test, MoqAutoData]
        public async Task And_Api_Is_Available_And_Returns_Ok_Then_Health_Check_Should_Return_True(
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            [Greedy] EmployerIncentivesService sut)
        {
            client.Setup(x => x.GetResponseCode(It.Is<IGetApiRequest>(p => p.GetUrl == "ping")))
                .ReturnsAsync(HttpStatusCode.OK);

            var result = await sut.IsHealthy();
            result.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task And_Api_Is_Available_And_Does_Not_Returns_Ok_Then_Health_Check_Should_Return_False(
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            [Greedy] EmployerIncentivesService sut)
        {
            client.Setup(x => x.GetResponseCode(It.Is<IGetApiRequest>(p => p.GetUrl == "ping")))
                .ReturnsAsync(HttpStatusCode.NotFound);

            var result = await sut.IsHealthy();
            result.Should().BeFalse();
        }
    }
}
