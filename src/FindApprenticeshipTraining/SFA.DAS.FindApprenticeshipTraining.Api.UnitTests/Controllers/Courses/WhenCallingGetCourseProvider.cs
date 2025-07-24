using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;
using SFA.DAS.Testing.AutoFixture;

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

    [Test]
    [MoqAutoData]
    public async Task And_The_Handler_Returns_Null_Then_Returns_NotFound(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] CoursesController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetCourseProviderQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetCourseProviderQueryResult)null);

        var result = await sut.GetCourseProvider(1, 1, new GetCourseProviderRequest(), CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
        var notFoundResult = result as NotFoundResult;
        notFoundResult!.StatusCode.Should().Be(404);
    }
}