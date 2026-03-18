using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseRoutes;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseRoutes;

public sealed class WhenGettingCourseRoutes
{
    [Test]
    [MoqAutoData]
    public async Task Then_Gets_Routes_From_Courses_Api(
        GetCourseRoutesQuery query,
        GetRoutesListResponse response,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> _mockCoursesApiClient,
        GetCourseRoutesQueryHandler sut
    )
    {
        _mockCoursesApiClient.Setup(client =>
            client.Get<GetRoutesListResponse>(
                It.IsAny<GetRoutesListRequest>()
            )
        )
        .ReturnsAsync(response);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Routes.Should().Equal(response.Routes);
    }
}
