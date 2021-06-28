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
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
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
            GetStandardsListItem getStandardResponse,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> demandApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            GetRestartEmployerDemandQueryHandler handler)
        {
            //Arrange
            demandApiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandByExpiredDemandRequest>(c => c.GetUrl.Contains($"demand?expiredCourseDemandId={query.Id}"))))
                .ReturnsAsync(getExpiredDemandResponse);
            demandApiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{query.Id}"))))
                .ReturnsAsync(getDemandResponse);
            coursesApiClient.Setup(x =>
                    x.Get<GetStandardsListItem>(
                        It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{getDemandResponse.Course.Id}"))))
                .ReturnsAsync(getStandardResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.EmployerDemand.Should().BeEquivalentTo(getExpiredDemandResponse);
            actual.RestartDemandExists.Should().BeTrue();
            actual.LastStartDate.Should().Be(getStandardResponse.StandardDates.LastDateStarts);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Demand_Data_Is_Returned_From_The_Api(
            GetRestartEmployerDemandQuery query,
            GetEmployerDemandResponse getDemandResponse,
            GetStandardsListItem getStandardResponse,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> demandApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            GetRestartEmployerDemandQueryHandler handler)
        {
            //Arrange
            demandApiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandByExpiredDemandRequest>(c => c.GetUrl.Contains($"demand?expiredCourseDemandId={query.Id}"))))
                .ReturnsAsync((GetEmployerDemandResponse)null);
            demandApiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{query.Id}"))))
                .ReturnsAsync(getDemandResponse);
            coursesApiClient.Setup(x =>
                    x.Get<GetStandardsListItem>(
                        It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{getDemandResponse.Course.Id}"))))
                .ReturnsAsync(getStandardResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.EmployerDemand.Should().BeEquivalentTo(getDemandResponse);
            actual.RestartDemandExists.Should().BeFalse();
            actual.LastStartDate.Should().Be(getStandardResponse.StandardDates.LastDateStarts);
        }
    }
}