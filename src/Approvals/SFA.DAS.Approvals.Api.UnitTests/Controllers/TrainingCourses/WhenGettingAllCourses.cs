using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.TrainingCourses;

public class WhenGettingAllCourses
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Courses_From_Mediator(
        GetCoursesResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] TrainingCoursesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetCoursesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetCourses() as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetCoursesListResponse;
        Assert.That(model, Is.Not.Null);
        model.Courses.Should().BeEquivalentTo(mediatorResult.Courses.Select(item => (GetCourseResponse)item));
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] TrainingCoursesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetCoursesQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetCourses() as BadRequestResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}