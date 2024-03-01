using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.Standards;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.TrainingCourses
{
    public class WhenGettingTrainingCourses
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Sector_Looked_Up_And_Returns_Standards(
            int routeId,
            GetStandardsQuery query,
            GetStandardsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICourseService> courseService,
            GetStandardsQueryHandler handler
        )
        {
            //Arrange
            courseService
                .Setup(x => x.GetRoutes())
                .ReturnsAsync(new GetRoutesListResponse
                {
                    Routes = new List<GetRoutesListItem>
                    {
                        new GetRoutesListItem
                        {
                            Id = routeId,
                            Name = query.Sector
                        }
                    }
                });
            apiClient.Setup(x => x.Get<GetStandardsListResponse>(It.Is<GetAvailableToStartStandardsListRequest>(c=>c.RouteIds.Contains(routeId)))).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Standards.Should().BeEquivalentTo(apiResponse.Standards);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Sector_Does_Not_Exist_Then_No_Standards_Are_Returned(
            GetStandardsQuery query,
            GetStandardsListResponse apiResponse,
            GetRoutesListResponse routesApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICourseService> courseService,
            GetStandardsQueryHandler handler)
        {
            //Arrange
            courseService
                .Setup(x => x.GetRoutes())
                .ReturnsAsync(routesApiResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Standards.Should().BeNull();
        }
    }
}