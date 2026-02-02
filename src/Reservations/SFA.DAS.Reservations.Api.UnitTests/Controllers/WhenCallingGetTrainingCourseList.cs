using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Api.Controllers;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourseList;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.Api.UnitTests.Controllers;

public class WhenCallingGetTrainingCourseList
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Training_Courses_From_Mediator(
        int standardCode,
        GetTrainingCoursesResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy]TrainingCoursesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetTrainingCoursesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetList() as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetTrainingCoursesListResponse;
        model.Should().NotBeNull();
        model.Courses.Should().BeEquivalentTo(mediatorResult.Courses.Select(course => (GetTrainingCoursesListItem)course));
        model.Courses.Should().HaveCount(mediatorResult.Courses.Count());
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        int standardCode,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy]TrainingCoursesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetTrainingCoursesQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetList() as BadRequestResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}