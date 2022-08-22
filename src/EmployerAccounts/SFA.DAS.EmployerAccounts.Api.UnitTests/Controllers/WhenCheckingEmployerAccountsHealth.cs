using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Api.Controllers;
using SFA.DAS.EmployerAccounts.Application.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers
{
    public class WhenCheckingEmployerAccountsHealth
    {
        private Mock<IEmployerAccountsService> _employerAccountsService;
        private HealthCheckController _pingController;

        [SetUp]
        public void Arrange()
        {
            _employerAccountsService = new Mock<IEmployerAccountsService>();
            _pingController = new HealthCheckController(_employerAccountsService.Object);
        }

        [Test]
        public async Task Then_The_Service_Is_Called_And_Healthy_Returned_If_True()
        {
            //Arrange
            _employerAccountsService.Setup(x => x.IsHealthy()).ReturnsAsync(true);

            //Act
            var actual = await _pingController.CheckHealth();

            //Assert
            actual.Status.Should().Be(HealthStatus.Healthy);
        }

        [Test]
        public async Task Then_The_Service_Is_Called_And_UnHealthy_Returned_If_False()         
        {
            //Arrange
            _employerAccountsService.Setup(x => x.IsHealthy()).ReturnsAsync(false);

            //Act
            var actual = await _pingController.CheckHealth();

            //Assert
            actual.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}
