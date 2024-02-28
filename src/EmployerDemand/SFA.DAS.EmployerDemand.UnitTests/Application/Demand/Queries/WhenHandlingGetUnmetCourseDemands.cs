using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnmetCourseDemands;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Queries
{
    public class WhenHandlingGetUnmetCourseDemands
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_The_CourseDemand_Ids_From_The_Api(
            GetUnmetCourseDemandsResponse response,
            GetUnmetCourseDemandsQuery query,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            GetUnmetCourseDemandsQueryHandler handler)
        {
            //Arrange
            apiClient.Setup(x => x.Get<GetUnmetCourseDemandsResponse>(
                    It.Is<GetUnmetEmployerDemandsRequest>(c => c.GetUrl.Contains($"unmet?ageOfDemandInDays={query.AgeOfDemandInDays}"))))
                .ReturnsAsync(response);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.EmployerDemandIds.Should().BeEquivalentTo(response.UnmetCourseDemands.Select(demand => demand.Id));
        }
    }
}