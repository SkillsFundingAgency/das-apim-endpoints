using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseLevels;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseLevels;

public sealed class WhenGettingCourseLevels
{
    [Test]
    [MoqAutoData]
    public async Task Then_Gets_Levels_From_Courses_Api(
        GetCourseLevelsQuery query,
        GetCourseLevelsListResponse response,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> _mockCoursesApiClient,
        GetCourseLevelsQueryHandler sut
    )
    {
        _mockCoursesApiClient.Setup(client => 
            client.Get<GetCourseLevelsListResponse>(
                It.IsAny<GetCourseLevelsListRequest>()
            )
        )
        .ReturnsAsync(response);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Levels.Should().Equal(response.Levels);
    }
}
