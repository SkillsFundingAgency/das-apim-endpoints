using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Services
{
    [TestFixture]
    public class WhenCallingHealthCheck
    {
        [Test]
        public async Task Then_Health_Check_Should_Return_False()
        {
            //Arrange
            var sut = new EmployerAccountsService(null);

            //Act            
            var result = await sut.IsHealthy();
            
            //Assert
            result.Should().Be(false);
        }

        [Test, MoqAutoData]
        public async Task And_Api_Is_Available_And_Returns_Ok_Then_Health_Check_Should_Return_True(
          [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> client,
          [Greedy] EmployerAccountsService sut)
        {
            //Arrange
            client.Setup(x => x.GetResponseCode(It.Is<IGetApiRequest>(p => p.GetUrl == "ping")))
                .ReturnsAsync(HttpStatusCode.OK);

            //Act
            var result = await sut.IsHealthy();
            
            //Assert
            result.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task And_Api_Is_Available_And_Does_Not_Returns_Ok_Then_Health_Check_Should_Return_False(
          [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> client,
          [Greedy] EmployerAccountsService sut)
        {
            //Arrange
            client.Setup(x => x.GetResponseCode(It.Is<IGetApiRequest>(p => p.GetUrl == "ping")))
                .ReturnsAsync(HttpStatusCode.NotFound);

            //Act
            var result = await sut.IsHealthy();

            //Assert
            result.Should().BeFalse();
        }
    }
}
