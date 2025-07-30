using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourse;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.TrainingCourses.Queries;
public class WhenGettingTrainingCourseDetails
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Standards_From_Courses_Api(
        GetTrainingCourseQuery query,
        StandardDetailResponse apiResponse,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
        GetTrainingCourseQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<StandardDetailResponse>(It.Is<GetStandardDetailsByIdRequest>(p=>p.Id == query.Id)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(apiResponse);
    }
}