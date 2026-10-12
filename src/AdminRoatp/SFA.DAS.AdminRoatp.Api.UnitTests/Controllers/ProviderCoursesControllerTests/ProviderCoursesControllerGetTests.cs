using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderCourse;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ProviderCoursesControllerTests;

public class ProviderCoursesControllerGetTests
{
    [Test, MoqAutoData]
    public async Task WhenGetProviderCourseIsInvoked_ThenReturnsOkResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderCoursesController sut,
        GetProviderCourseResponse expected,
        int ukprn,
        string larsCode)
    {
        mediatorMock
            .Setup(m => m.Send(It.Is<GetProviderCourseQuery>(q => q.Ukprn == ukprn && q.LarsCode == larsCode), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await sut.GetProviderCourse(ukprn, larsCode, CancellationToken.None);

        using (new AssertionScope())
        {
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(expected);
            mediatorMock.Verify(
                m => m.Send(It.Is<GetProviderCourseQuery>(q => q.Ukprn == ukprn && q.LarsCode == larsCode), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    [Test, MoqAutoData]
    public async Task WhenGetProviderCourseIsInvoked_AndResultIsNull_ThenReturnsNotFoundResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderCoursesController sut,
        int ukprn,
        string larsCode)
    {
        mediatorMock
            .Setup(m => m.Send(It.Is<GetProviderCourseQuery>(q => q.Ukprn == ukprn && q.LarsCode == larsCode), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var result = await sut.GetProviderCourse(ukprn, larsCode, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundResult>();
            mediatorMock.Verify(
                m => m.Send(It.Is<GetProviderCourseQuery>(q => q.Ukprn == ukprn && q.LarsCode == larsCode), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
