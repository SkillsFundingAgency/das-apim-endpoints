using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.Application.TrainingCourses;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.TrainingCourses;

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

        Assert.IsNotNull(controllerResult);
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetStandardResponse;
        Assert.IsNotNull(model);
        model.Should().BeEquivalentTo((GetStandardResponse)mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task No_Standard_Is_Returned_From_Mediator_Then_Should_Return_NotFound(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] TrainingCoursesController controller,
        string courseCode)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetStandardQuery>(x => x.CourseCode == courseCode),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetStandardsListItem?)null);


        var controllerResult = await controller.GetStandard("courseCode") as NotFoundResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
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

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}