using System;
using System.Threading;
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
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword) 
                                                      && c.RouteIds.Equals(query.RouteIds)
                                                      && c.Levels.Equals(query.Levels)
                                                      )))
                .ReturnsAsync(apiResponse);
            var outFalse = false;
            cacheStorageService.Setup(cache => cache.GetRequest<GetSectorsListResponse>(mockApiClient.Object,
                    It.IsAny<GetSectorsListRequest>(), nameof(GetSectorsListResponse), out outFalse))
                .ReturnsAsync(sectorsApiResponse);
            cacheStorageService.Setup(cache => cache.GetRequest<GetLevelsListResponse>(mockApiClient.Object,
                    It.IsAny<GetLevelsListRequest>(), nameof(GetLevelsListResponse), out outFalse))
                .ReturnsAsync(levelsApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
            result.Sectors.Should().BeEquivalentTo(sectorsApiResponse.Sectors);
            result.Levels.Should().BeEquivalentTo(levelsApiResponse.Levels);
            result.Total.Should().Be(apiResponse.Total);
            result.TotalFiltered.Should().Be(apiResponse.TotalFiltered);
            result.OrderBy.Should().Be(query.OrderBy);
        }
    }
}

