using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourseList;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourses
    {
        [Test]
        public async Task Then_Calls_Standards_And_Courses_Search_Api_And_Returns_Combined_Results()
        {
            var query = new GetTrainingCoursesQuery();
            var standardsResponse = new GetStandardsListResponse
            {
                Standards = new List<GetStandardsListItem>
                {
                    new() { LarsCode = 1, Title = "Standard 1", Level = 4, StandardUId = "ST0001_1.0", ApprenticeshipType = "Apprenticeship", LearningType = "Standard" }
                }
            };
            var searchResponse = new GetCoursesSearchApiResponse
            {
                Courses = new List<CourseSearchItemDto>
                {
                    new()
                    {
                        StandardUId = "ZSC00002_1.0",
                        LarsCode = "ZSC00002",
                        Title = "Teacher Assistant - Apprenticeship Unit",
                        Level = 4,
                        CourseDates = new CourseDatesDto { EffectiveFrom = new DateTime(2026, 1, 1), EffectiveTo = null }
                    }
                },
                Total = 1,
                TotalFiltered = 1
            };
            var mockApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(standardsResponse);
            mockApiClient
                .Setup(client => client.Get<GetCoursesSearchApiResponse>(It.IsAny<GetCoursesSearchRequest>()))
                .ReturnsAsync(searchResponse);
            var handler = new GetTrainingCoursesQueryHandler(mockApiClient.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().HaveCount(2);
            result.Courses.Should().ContainSingle(c => c.LarsCode == "1" && c.Title == "Standard 1" && c.Level == "4" && c.LearningType == "Apprenticeship");
            result.Courses.Should().ContainSingle(c => c.LarsCode == "ZSC00002" && c.Title == "Teacher Assistant - Apprenticeship Unit" && c.LearningType == "ApprenticeshipUnit");
            mockApiClient.Verify(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()), Times.Once);
            mockApiClient.Verify(client => client.Get<GetCoursesSearchApiResponse>(It.IsAny<GetCoursesSearchRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Combines_Standards_From_Api_With_Apprenticeship_Units_From_Courses_Search(
            GetTrainingCoursesQuery query,
            GetStandardsListItem standardItem,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingCoursesQueryHandler handler)
        {
            var standardsResponse = new GetStandardsListResponse { Standards = new List<GetStandardsListItem> { standardItem } };
            var searchResponse = new GetCoursesSearchApiResponse
            {
                Courses = new List<CourseSearchItemDto>
                {
                    new() { StandardUId = "ZSC00002_1.0", LarsCode = "ZSC00002", Title = "An App Unit", Level = 4, CourseDates = new CourseDatesDto { EffectiveFrom = DateTime.UtcNow } }
                }
            };

            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(standardsResponse);
            mockApiClient
                .Setup(client => client.Get<GetCoursesSearchApiResponse>(It.IsAny<GetCoursesSearchRequest>()))
                .ReturnsAsync(searchResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().HaveCount(2);
            result.Courses.Should().ContainSingle(c => c.LarsCode == standardItem.LarsCode.ToString());
            result.Courses.Should().ContainSingle(c => c.LarsCode == "ZSC00002");
        }
    }
}