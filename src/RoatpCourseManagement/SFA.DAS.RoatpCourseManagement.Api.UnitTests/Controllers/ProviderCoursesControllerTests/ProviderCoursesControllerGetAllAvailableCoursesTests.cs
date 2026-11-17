using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCourseForProvider;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderCoursesControllerTests;

[TestFixture]
public class ProviderCoursesControllerGetAllAvailableCoursesTests
{
    [Test]
    [MoqInlineAutoData(CourseType.Apprenticeship)]
    [MoqInlineAutoData(CourseType.ShortCourse)]
    public async Task GetAllAvailableCourses_ReturnsCourses(
        CourseType courseType,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderCoursesController sut,
        GetAvailableCoursesForProviderQueryResult response,
        int ukprn
        )
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetAvailableCoursesForProviderQuery>(q => q.Ukprn == ukprn && q.CourseType == courseType), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.GetAllAvailableCourses(ukprn, courseType);

        var okObjectResult = result as OkObjectResult;
        okObjectResult.Should().NotBeNull();
        var queryResult = okObjectResult.Value as GetAvailableCoursesForProviderQueryResult;
        queryResult.AvailableCourses.Should().HaveCount(response.AvailableCourses.Count());
    }

    [Test]
    [MoqInlineAutoData(CourseType.Apprenticeship)]
    [MoqInlineAutoData(CourseType.ShortCourse)]
    public async Task GetAvailableCourse_CourseAvailable_ReturnsCourseDetails(
        CourseType courseType,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderCoursesController sut,
        GetAvailableCourseForProviderQueryResult response,
        int ukprn,
        string larsCode
        )
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetAvailableCourseForProviderQuery>(q => q.Ukprn == ukprn && q.CourseType == courseType && q.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.GetAvailableCourse(ukprn, courseType, larsCode);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okObjectResult = result as OkObjectResult;
        okObjectResult.Value.Should().BeEquivalentTo(response);
    }

    [Test]
    [MoqInlineAutoData(CourseType.Apprenticeship)]
    [MoqInlineAutoData(CourseType.ShortCourse)]
    public async Task GetAvailableCourse_CourseNotAvailable_ReturnsNotFound(
        CourseType courseType,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderCoursesController sut,
        int ukprn,
        string larsCode
        )
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAvailableCourseForProviderQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((GetAvailableCourseForProviderQueryResult)null);

        var result = await sut.GetAvailableCourse(ukprn, courseType, larsCode);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}
