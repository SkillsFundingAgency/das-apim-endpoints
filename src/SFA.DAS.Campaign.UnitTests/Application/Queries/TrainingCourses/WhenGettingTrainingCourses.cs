using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.Standards;
using SFA.DAS.Campaign.InnerApi.Requests;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
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
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetStandardsQueryHandler handler
        )
        {
            //Arrange
            cacheStorageService
                .Setup(x => x.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
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
        public async Task Then_If_The_Sectors_Cache_Is_Empty_They_Are_Looked_Up_And_Added(
            int routeId,
            GetStandardsQuery query,
            GetStandardsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetStandardsQueryHandler handler)
        {
            //Arrange
            var sectorsApiResponse = new GetRoutesListResponse
            {
                Routes = new List<GetRoutesListItem>
                {
                    new GetRoutesListItem
                    {
                        Id = routeId,
                        Name = query.Sector
                    }
                }
            };
            cacheStorageService
                .Setup(x => x.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync((GetRoutesListResponse) default);
            apiClient.Setup(x => x.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>())).ReturnsAsync(sectorsApiResponse);
            apiClient.Setup(x => x.Get<GetStandardsListResponse>(It.Is<GetAvailableToStartStandardsListRequest>(c=>c.RouteIds.Contains(routeId)))).ReturnsAsync(apiResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Standards.Should().BeEquivalentTo(apiResponse.Standards);
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetRoutesListResponse),sectorsApiResponse, 23));

        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Sector_Does_Not_Exist_Then_No_Standards_Are_Returned(
            GetStandardsQuery query,
            GetStandardsListResponse apiResponse,
            GetRoutesListResponse routesApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetStandardsQueryHandler handler)
        {
            //Arrange
            cacheStorageService
                .Setup(x => x.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync((GetRoutesListResponse) default);
            apiClient.Setup(x => x.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>())).ReturnsAsync(routesApiResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Standards.Should().BeNull();
        }
    }
}