using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingIsHealthy
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Client_Is_Sent_A_Ping_Request_And_True_Returned_If_OK_Status_Code(
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service)
        {
            //Arrange
            client.Setup(x => 
                x.GetResponseCode(It.IsAny<GetHealthRequest>())).ReturnsAsync(HttpStatusCode.OK);
            
            //Act
            var actual = await service.IsHealthy();
            
            //Assert
            actual.Should().BeTrue();
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Api_Client_Is_Sent_A_Ping_Request_And_False_Returned_If_Not_OK_Status_Code(
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service)
        {
            //Arrange
            client.Setup(x => 
                x.GetResponseCode(It.IsAny<GetPingRequest>())).ReturnsAsync(HttpStatusCode.InternalServerError);
            
            //Act
            var actual = await service.IsHealthy();
            
            //Assert
            actual.Should().BeFalse();
        }
    }
}