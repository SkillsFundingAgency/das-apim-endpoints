﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCoursesList;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourseList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_And_Sectors_And_Levels_From_Courses_Api_With_Request_Params(
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetSectorsListResponse sectorsApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingCoursesListQueryHandler handler)
        {
            
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword) 
                                                      && c.RouteIds.Equals(query.RouteIds)
                                                      && c.Levels.Equals(query.Levels)
                                                      )))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetSectorsListResponse>(It.IsAny<GetSectorsListRequest>()))
                .ReturnsAsync(sectorsApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
            result.Sectors.Should().BeEquivalentTo(sectorsApiResponse.Sectors);
            result.Levels.Should().BeEquivalentTo(levelsApiResponse.Levels);
            result.Total.Should().Be(apiResponse.Total);
            result.TotalFiltered.Should().Be(apiResponse.TotalFiltered);
            result.OrderBy.Should().Be(query.OrderBy);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Sectors_Are_Added_To_The_Cache_If_Not_Available(
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetSectorsListResponse sectorsApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword) && c.RouteIds.Equals(query.RouteIds))))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetSectorsListResponse>(It.IsAny<GetSectorsListRequest>()))
                .ReturnsAsync(sectorsApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            
            await handler.Handle(query, CancellationToken.None);
            
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetSectorsListResponse),sectorsApiResponse,1));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Sectors_Are_Returned_From_The_Cache_If_Available(
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetSectorsListResponse sectorsApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            //Arrange
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword) && c.RouteIds.Equals(query.RouteIds))))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            cacheStorageService
                .Setup(x =>
                    x.RetrieveFromCache<GetSectorsListResponse>(nameof(GetSectorsListResponse)))
                .ReturnsAsync(sectorsApiResponse);
            
            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
            result.Sectors.Should().BeEquivalentTo(sectorsApiResponse.Sectors);
            result.Total.Should().Be(apiResponse.Total);
            result.TotalFiltered.Should().Be(apiResponse.TotalFiltered);
            mockApiClient.Verify(x=>x.Get<GetSectorsListResponse>(It.IsAny<GetSectorsListRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Levels_Are_Added_To_The_Cache_If_Not_Available( GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetSectorsListResponse sectorsApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword) && c.RouteIds.Equals(query.RouteIds))))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetSectorsListResponse>(It.IsAny<GetSectorsListRequest>()))
                .ReturnsAsync(sectorsApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            
            await handler.Handle(query, CancellationToken.None);
            
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetLevelsListResponse),levelsApiResponse,1));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Levels_Are_Returned_From_The_Cache_If_Available(GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetSectorsListResponse sectorsApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            //Arrange
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword) && c.RouteIds.Equals(query.RouteIds))))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetSectorsListResponse>(It.IsAny<GetSectorsListRequest>()))
                .ReturnsAsync(sectorsApiResponse);
            cacheStorageService
                .Setup(x =>
                    x.RetrieveFromCache<GetLevelsListResponse>(nameof(GetLevelsListResponse)))
                .ReturnsAsync(levelsApiResponse);
            
            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
            result.Sectors.Should().BeEquivalentTo(sectorsApiResponse.Sectors);
            result.Levels.Should().BeEquivalentTo(levelsApiResponse.Levels);
            result.Total.Should().Be(apiResponse.Total);
            result.TotalFiltered.Should().Be(apiResponse.TotalFiltered);
            mockApiClient.Verify(x=>x.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()), Times.Never);
        }
        
        
    }
}

