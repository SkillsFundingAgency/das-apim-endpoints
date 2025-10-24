﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Learning.Api.Controllers;
using SFA.DAS.Learning.Api.Models;
using SFA.DAS.Learning.Application.TrainingCourses;
using SFA.DAS.Learning.InnerApi;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Learning.Api.UnitTests.Controllers.TrainingCourses;

[TestFixture]
public class WhenGettingAStandard
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Standard_From_Mediator(
        string courseCode,
        GetStandardsListItem mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] TrainingCoursesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetStandardQuery>(x => x.CourseCode == courseCode),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetStandard(courseCode) as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetStandardResponse;
        Assert.That(model, Is.Not.Null);
        model.Should().BeEquivalentTo((GetStandardResponse)mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task No_Standard_Is_Returned_From_Mediator_Then_Should_Return_NotFound(string courseCode)
    {
        var controller = new TrainingCoursesController(Mock.Of<ILogger<TrainingCoursesController>>(), Mock.Of<IMediator>());
        var controllerResult = await controller.GetStandard("courseCode") as NotFoundResult;
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        string courseCode,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] TrainingCoursesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetStandardQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetStandard(courseCode) as BadRequestResult;

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}