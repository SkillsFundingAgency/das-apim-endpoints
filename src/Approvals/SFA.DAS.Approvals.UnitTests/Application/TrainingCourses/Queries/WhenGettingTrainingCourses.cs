using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourses
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Courses_From_Courses_Api(
            GetCoursesQuery query,
            GetCoursesListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetCoursesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetCoursesExportRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(apiResponse.Courses);
        }
    }
}