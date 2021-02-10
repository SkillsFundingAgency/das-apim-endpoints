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
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api(
            GetTrainingCoursesQuery query,
            GetStandardsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingCoursesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
        }
    }
}