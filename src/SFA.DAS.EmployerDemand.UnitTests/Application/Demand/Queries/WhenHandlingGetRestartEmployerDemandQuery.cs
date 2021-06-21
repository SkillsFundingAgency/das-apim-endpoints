using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRestartEmployerDemand;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Queries
{
    public class WhenHandlingGetRestartEmployerDemandQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Checked_To_See_If_A_New_Demand_Is_Already_Created_And_That_Id_Returned(
            GetRestartEmployerDemandQuery query,
            GetEmployerDemandResponse getDemandResponse,
            GetEmployerDemandResponse getExpiredDemandResponse,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            GetRestartEmployerDemandQueryHandler handler)
        {
            //Arrange
            apiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandByExpiredDemandRequest>(c => c.GetUrl.Contains($"demand?expiredCourseDemandId={query.Id}"))))
                .ReturnsAsync(getExpiredDemandResponse);
            apiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{query.Id}"))))
                .ReturnsAsync(getDemandResponse);   
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.EmployerDemand.Should().BeEquivalentTo(getExpiredDemandResponse);
            actual.RestartDemandExists.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Demand_Data_Is_Returned_From_The_Api(
            GetRestartEmployerDemandQuery query,
            GetEmployerDemandResponse getDemandResponse,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            GetRestartEmployerDemandQueryHandler handler)
        {
            //Arrange
            apiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandByExpiredDemandRequest>(c => c.GetUrl.Contains($"demand?expiredCourseDemandId={query.Id}"))))
                .ReturnsAsync((GetEmployerDemandResponse)null);
            apiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{query.Id}"))))
                .ReturnsAsync(getDemandResponse);   
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.EmployerDemand.Should().BeEquivalentTo(getDemandResponse);
            actual.RestartDemandExists.Should().BeFalse();
        }
    }
}