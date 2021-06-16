using System;
using System.Collections.Generic;
using System.Linq;
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
            List<GetUnmetCourseDemandsResponse> unmetDemandResponses,
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
            var standards = closedStandardsApiResponse.Standards.ToList();
            for (var x = 0; x < standards.Count; x++)
            {
                var i = x;//needed to extend lifetime of value of x
                var expectedApiRequest = new GetUnmetEmployerDemandsRequest(0, standards[i].LarsCode);
                mockEmployerDemandApiClient
                    .Setup(client => client.Get<GetUnmetCourseDemandsResponse>(
                        It.Is<GetUnmetEmployerDemandsRequest>(c => c.GetUrl == expectedApiRequest.GetUrl)))
                    .ReturnsAsync(unmetDemandResponses[i]);
            }
            var expectedDemandIds = new List<Guid>();
            foreach (var demandsResponse in unmetDemandResponses)
            {
                expectedDemandIds.AddRange(demandsResponse.EmployerDemandIds);
            }

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.EmployerDemandIds.Should().BeEquivalentTo(expectedDemandIds);
        }
    }
}