using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models.Responses;
using SFA.DAS.RecruitQa.Application.GetTrainingProgrammes;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.TrainingProgrammes;

public class WhenGettingAllTrainingProgrammes
{
    [Test, MoqAutoData]
    public async Task Then_Gets_TrainingProgrammes_From_Mediator(
        GetTrainingProgrammesQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] TrainingProgrammesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetTrainingProgrammesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetAll() as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetTrainingProgrammesListResponse;
        Assert.That(model, Is.Not.Null);
        model!.TrainingProgrammes.Should().BeEquivalentTo(mediatorResult._);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] TrainingProgrammesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetTrainingProgrammesQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetAll() as BadRequestResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}