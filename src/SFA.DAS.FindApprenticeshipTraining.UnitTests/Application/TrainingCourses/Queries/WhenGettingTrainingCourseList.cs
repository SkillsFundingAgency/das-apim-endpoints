using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Application.Interfaces;
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
            [Frozen] Mock<IApiClient> mockApiClient,
            GetTrainingCoursesListQueryHandler handler)
        {
            ArrangeStandardsToHaveValidDates(apiResponse);
            
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
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
            result.Sectors.Should().BeEquivalentTo(sectorsApiResponse.Sectors);
            result.Levels.Should().BeEquivalentTo(levelsApiResponse.Levels);
            result.Total.Should().Be(apiResponse.Total);
            result.TotalFiltered.Should().Be(apiResponse.TotalFiltered);
        }


        [Test, MoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_Based_On_The_Available_Dates(
            GetTrainingCoursesListQuery query,
            GetSectorsListResponse sectorsApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<IApiClient> mockApiClient,
            GetTrainingCoursesListQueryHandler handler)
        {
            //Arrange
            var sameDate = DateTime.UtcNow.AddDays(10);
            var standardsListResponse = new GetStandardsListResponse
            {
                Standards = new List<GetStandardsListItem>
                {
                    new GetStandardsListItem
                    {
                        Title = "Available",
                        ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                        StandardDates = new List<StandardDate>
                        {
                            new StandardDate
                            {
                                EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                                LastDateStarts = null
                            }
                        }
                    },
                    new GetStandardsListItem
                    {
                        Title = "Not Available 1",
                        ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                        StandardDates = new List<StandardDate>
                        {
                            new StandardDate
                            {
                                EffectiveFrom = DateTime.UtcNow.AddMonths(1),
                                LastDateStarts = null
                            }
                        }
                    },
                    new GetStandardsListItem
                    {
                        Title = "Not Available 2",
                        ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                        StandardDates = new List<StandardDate>
                        {
                            new StandardDate
                            {
                                EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                                LastDateStarts = DateTime.UtcNow.AddDays(-1)
                            }
                        }
                    },
                    new GetStandardsListItem
                    {
                        Title = "Not Available 3",
                        ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                        StandardDates = new List<StandardDate>
                        {
                            new StandardDate
                            {
                                EffectiveFrom = sameDate,
                                LastDateStarts = sameDate
                            }
                        }
                    }
                }
            };
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword) && c.RouteIds.Equals(query.RouteIds))))
                .ReturnsAsync(standardsListResponse);
            mockApiClient
                .Setup(client => client.Get<GetSectorsListResponse>(It.IsAny<GetSectorsListRequest>()))
                .ReturnsAsync(sectorsApiResponse);
            mockApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            
            
            //Act
            var result = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            Assert.AreEqual(1,result.Courses.ToList().Count);
            Assert.IsTrue(result.Courses.ToList().TrueForAll(c=>c.Title.Equals("Available")));
        }

        
        

        [Test, MoqAutoData]
        public async Task Then_The_Sectors_Are_Added_To_The_Cache_If_Not_Available(
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetSectorsListResponse sectorsApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<IApiClient> mockApiClient,
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
            
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetSectorsListResponse),sectorsApiResponse,23));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Sectors_Are_Returned_From_The_Cache_If_Available(
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetSectorsListResponse sectorsApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<IApiClient> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            //Arrange
            ArrangeStandardsToHaveValidDates(apiResponse);
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
            [Frozen] Mock<IApiClient> mockApiClient,
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
            
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetLevelsListResponse),levelsApiResponse,23));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Levels_Are_Returned_From_The_Cache_If_Available(GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetSectorsListResponse sectorsApiResponse,
            GetLevelsListResponse levelsApiResponse,
            [Frozen] Mock<IApiClient> mockApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetTrainingCoursesListQueryHandler handler)
        {
            //Arrange
            ArrangeStandardsToHaveValidDates(apiResponse);
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
        
        private static void ArrangeStandardsToHaveValidDates(GetStandardsListResponse apiResponse)
        {
            apiResponse.Standards = apiResponse.Standards.Select(c =>
            {
                c.StandardDates.Select(d => { d.LastDateStarts = null; d.EffectiveFrom = DateTime.UtcNow.AddDays(-1);
                    return d;
                }).ToList(); return c;
            }).ToList();
        }
    }
}

