using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnmetDemandsWithStoppedCourse;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Queries
{
    public class WhenHandlingGetUnmetDemandsWithStoppedCourseQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_The_CourseDemand_Ids_From_The_Api(
            GetStandardsListResponse closedStandardsApiResponse,
            GetUnmetCourseDemandsResponse unmetDemandResponse,
            GetUnmetDemandsWithStoppedCourseQuery query,
            [Frozen]Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            GetUnmetDemandsWithStoppedCourseQueryHandler handler)
        {
            //Arrange
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.IsAny<GetStandardsClosedToNewStartsRequest>()))
                .ReturnsAsync(closedStandardsApiResponse);
            var expectedApiRequest = new GetUnmetEmployerDemandsRequest(0);
            mockEmployerDemandApiClient
                .Setup(client => client.Get<GetUnmetCourseDemandsResponse>(
                    It.Is<GetUnmetEmployerDemandsRequest>(c => c.GetUrl == expectedApiRequest.GetUrl)))
                .ReturnsAsync(unmetDemandResponse);
            unmetDemandResponse.UnmetCourseDemands[0].CourseId = closedStandardsApiResponse.Standards[0].LarsCode;
            var expectedDemandIds = new List<Guid> {unmetDemandResponse.UnmetCourseDemands[0].Id};
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.EmployerDemandIds.Should().BeEquivalentTo(expectedDemandIds);
        }
    }
}