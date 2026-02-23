using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;
using SFA.DAS.SharedOuterApi.InnerApi;
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
        queryResult.AvailableCourses.Count.Should().Be(response.AvailableCourses.Count);
    }
}