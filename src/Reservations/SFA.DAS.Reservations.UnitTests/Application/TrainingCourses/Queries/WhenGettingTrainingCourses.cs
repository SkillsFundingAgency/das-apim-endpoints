using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourseList;
using SFA.DAS.Reservations.InnerApi.Requests;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourses
    {
        private const string HardcodedAppUnitLarsCode = "ZSC00002";

        [Test]
        public async Task Then_Calls_Standards_Api_Only_And_Returns_Standards_Plus_Hardcoded_App_Unit()
        {
            var query = new GetTrainingCoursesQuery();
            var standardsResponse = new GetStandardsListResponse
            {
                Standards = new List<GetStandardsListItem>
                {
                    new() { LarsCode = 1, Title = "Standard 1", Level = 4, StandardUId = "ST0001_1.0", ApprenticeshipType = "Apprenticeship", LearningType = "Standard" }
                }
            };
            var mockApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(standardsResponse);
            var handler = new GetTrainingCoursesQueryHandler(mockApiClient.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().HaveCount(2);
            result.Courses.Should().ContainSingle(c => c.LarsCode == "1" && c.Title == "Standard 1" && c.Level == "4" && c.LearningType == "Apprenticeship");
            result.Courses.Should().ContainSingle(c => c.LarsCode == HardcodedAppUnitLarsCode);
            mockApiClient.Verify(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()), Times.Once);
            mockApiClient.Verify(client => client.Get<GetShortCoursesListResponse>(It.IsAny<GetShortCoursesListRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Combines_Standards_From_Api_With_Hardcoded_Example_App_Unit(
            GetTrainingCoursesQuery query,
            GetStandardsListItem standardItem,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingCoursesQueryHandler handler)
        {
            var standardsResponse = new GetStandardsListResponse { Standards = new List<GetStandardsListItem> { standardItem } };

            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(standardsResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().HaveCount(2);
            result.Courses.Should().ContainSingle(c => c.LarsCode == standardItem.LarsCode.ToString());
            result.Courses.Should().ContainSingle(c => c.LarsCode == HardcodedAppUnitLarsCode);
        }
    }
}