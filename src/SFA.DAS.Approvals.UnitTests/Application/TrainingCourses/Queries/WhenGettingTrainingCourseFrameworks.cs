using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourseFrameworks
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Frameworks_From_Courses_Api(
            GetFrameworksQuery query,
            GetFrameworksListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetFrameworksQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetFrameworksListResponse>(It.IsAny<GetFrameworksRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Frameworks.Should().BeEquivalentTo(apiResponse.Frameworks);
        }
    }
}