using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Courses;

public sealed class WhenCallingGetCourseProvider
{
    [Test]
    [MoqAutoData]
    public async Task Then_Get_Course_Provider_Calls_Mediator_With_Expected_Properties(
        GetCourseProviderRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] CoursesController sut
    )
    {
        int larsCode = 123;
        long ukprn = 10000002;

        mockMediator
            .Setup(m => m.Send(
                It.Is<GetCourseProviderQuery>(q =>
                    q.LarsCode == larsCode &&
                    q.Ukprn == ukprn &&
                    q.ShortlistUserId == request.ShortlistUserId &&
                    q.Location == request.Location &&
                    q.Distance == request.Distance
                ),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new GetCourseProviderQueryResult());

        await sut.GetCourseProvider(larsCode, ukprn, request, CancellationToken.None);

        mockMediator.Verify(m => m.Send(
                It.Is<GetCourseProviderQuery>(q =>
                    q.LarsCode == larsCode &&
                    q.Ukprn == ukprn &&
                    q.ShortlistUserId == request.ShortlistUserId &&
                    q.Location == request.Location &&
                    q.Distance == request.Distance
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}
