using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCoursesList;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourseList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_And_Sectors_And_Levels_From_Courses_Api_And_Shortlist_Item_Count_With_Request_Params(
            int sectorId,
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetLevelsListResponse levelsApiResponse,
            int shortlistItemCount,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            GetTrainingCoursesListQueryHandler handler)
        {
            var sectorsApiResponse = new GetRoutesListResponse
            {
                Routes = new List<GetRoutesListItem>
                {
                    new GetRoutesListItem
                    {
                        Id = sectorId,
                        Name = query.RouteIds.First()
                    }
                }
            };
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetAvailableToStartStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword) 
                                                      && c.RouteIds.Contains(sectorId)
                                                      && c.Levels.Equals(query.Levels)
                                                      )))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()))
                .ReturnsAsync(sectorsApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            shortlistService.Setup(x => x.GetShortlistItemCount(query.ShortlistUserId))
                .ReturnsAsync(shortlistItemCount);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
            result.Sectors.Should().BeEquivalentTo(sectorsApiResponse.Routes);
            result.Levels.Should().BeEquivalentTo(levelsApiResponse.Levels);
            result.Total.Should().Be(apiResponse.Total);
            result.TotalFiltered.Should().Be(apiResponse.TotalFiltered);
            result.OrderBy.Should().Be(query.OrderBy);
            result.ShortlistItemCount.Should().Be(shortlistItemCount);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Caches_The_Courses_If_There_Are_No_Filters(
            GetStandardsListResponse apiResponse,
            GetRoutesListResponse routesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingCoursesListQueryHandler handler)
        {
            var query = new GetTrainingCoursesListQuery
            {
                Levels = new List<int>(),
                RouteIds = new List<string>()
            };
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()))
                .ReturnsAsync(routesApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            
            var result = await handler.Handle(query, CancellationToken.None);

            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetStandardsListResponse), apiResponse, TimeSpan.FromHours(2)));
            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
            result.Sectors.Should().BeEquivalentTo(routesApiResponse.Routes);
            result.Levels.Should().BeEquivalentTo(levelsApiResponse.Levels);
            result.Total.Should().Be(apiResponse.Total);
            result.TotalFiltered.Should().Be(apiResponse.TotalFiltered);
            result.OrderBy.Should().Be(query.OrderBy);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Sectors_Are_Added_To_The_Cache_If_Not_Available(
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetRoutesListResponse routesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetAvailableToStartStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword))))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()))
                .ReturnsAsync(routesApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            
            await handler.Handle(query, CancellationToken.None);
            
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetRoutesListResponse),routesApiResponse, TimeSpan.FromHours(2)));
        }

        [Test, MoqAutoData]
        public async Task Then_Sector_Ids_Are_Looked_Up_If_There_Is_A_Sector_Filter(
            int sectorId,
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            var sectorsApiResponse = new GetRoutesListResponse
            {
                Routes = new List<GetRoutesListItem>
                {
                    new GetRoutesListItem
                    {
                        Id = sectorId,
                        Name = query.RouteIds.First()
                    }
                }
            };
            mockApiClient
                .Setup(client => client.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()))
                .ReturnsAsync(sectorsApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetAvailableToStartStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword) && c.RouteIds.Contains(sectorId))))
                .ReturnsAsync(apiResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);
            
            Assert.IsNotNull(actual);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Sectors_Are_Returned_From_The_Cache_If_Available(
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetRoutesListResponse routesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            //Arrange
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetAvailableToStartStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword))))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            cacheStorageService
                .Setup(x =>
                    x.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync(routesApiResponse);
            
            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
            result.Sectors.Should().BeEquivalentTo(routesApiResponse.Routes);
            result.Total.Should().Be(apiResponse.Total);
            result.TotalFiltered.Should().Be(apiResponse.TotalFiltered);
            mockApiClient.Verify(x=>x.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Levels_Are_Added_To_The_Cache_If_Not_Available( GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetRoutesListResponse routesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetAvailableToStartStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword))))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()))
                .ReturnsAsync(routesApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            
            await handler.Handle(query, CancellationToken.None);
            
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetLevelsListResponse),levelsApiResponse, TimeSpan.FromHours(2)));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Levels_Are_Returned_From_The_Cache_If_Available(GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetRoutesListResponse routesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            //Arrange
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetAvailableToStartStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword))))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()))
                .ReturnsAsync(routesApiResponse);
            cacheStorageService
                .Setup(x =>
                    x.RetrieveFromCache<GetLevelsListResponse>(nameof(GetLevelsListResponse)))
                .ReturnsAsync(levelsApiResponse);
            
            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
            result.Sectors.Should().BeEquivalentTo(routesApiResponse.Routes);
            result.Levels.Should().BeEquivalentTo(levelsApiResponse.Levels);
            result.Total.Should().Be(apiResponse.Total);
            result.TotalFiltered.Should().Be(apiResponse.TotalFiltered);
            mockApiClient.Verify(x=>x.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Caches_The_Courses_If_The_Cache_Has_An_Empty_Course_List(
            GetStandardsListResponse cachedStandards,
            GetStandardsListResponse apiResponse,
            GetRoutesListResponse routesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingCoursesListQueryHandler handler)
        {
            cachedStandards.Total = 0;
            var query = new GetTrainingCoursesListQuery
            {
                Levels = new List<int>(),
                RouteIds = new List<string>()
            };
            cacheStorageService
                .Setup(cache => cache.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(cachedStandards);

            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()))
                .ReturnsAsync(routesApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            cacheStorageService.Verify(x => x.SaveToCache(nameof(GetStandardsListResponse), apiResponse, TimeSpan.FromHours(2)));
            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
        }

    }
}

