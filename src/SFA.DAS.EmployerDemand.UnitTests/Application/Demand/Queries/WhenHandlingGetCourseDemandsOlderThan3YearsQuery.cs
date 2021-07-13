using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCourseDemandsOlderThan3Years;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Queries
{
    public class WhenHandlingGetCourseDemandsOlderThan3YearsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_The_CourseDemand_Ids_From_The_Api(
            GetEmployerDemandsOlderThan3YearsResponse oldDemandsResponse,
            GetCourseDemandsOlderThan3YearsQuery query,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            GetCourseDemandsOlderThan3YearsQueryHandler handler)
        {
            //Arrange
            mockEmployerDemandApiClient
                .Setup(client => client.Get<GetEmployerDemandsOlderThan3YearsResponse>(
                    It.IsAny<GetEmployerDemandsOlderThan3YearsRequest>()))
                .ReturnsAsync(oldDemandsResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.EmployerDemandIds.Should().BeEquivalentTo(oldDemandsResponse.EmployerDemandIds);
        }
    }
}