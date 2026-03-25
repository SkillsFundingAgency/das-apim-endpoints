using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models.Cohorts;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAssignAllowEmployerAdd;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Cohorts;

public class WhenGettingAssignAllowEmployerAdd
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_AllowEmployerAdd_From_Mediator(
        string accountHashedId,
        Guid reservationId,
        [Frozen] Mock<ISender> mediator,
        [Greedy] CohortController controller)
    {
        var mediatorResult = new GetAssignAllowEmployerAddQueryResult
        {
            LearningType = 2,
            AllowEmployerAdd = false
        };

        mediator.Setup(x => x.Send(
                It.Is<GetAssignAllowEmployerAddQuery>(q => q.ReservationId == reservationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetAssignAllowEmployerAdd(accountHashedId, reservationId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetAssignAllowEmployerAddResponse;
        model.Should().NotBeNull();
        model!.LearningType.Should().Be(2);
        model.AllowEmployerAdd.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task And_Result_Is_Null_Then_Returns_NotFound(
        string accountHashedId,
        Guid reservationId,
        [Frozen] Mock<ISender> mediator,
        [Greedy] CohortController controller)
    {
        GetAssignAllowEmployerAddQueryResult? nullResult = null;
        mediator.Setup(x => x.Send(
                It.IsAny<GetAssignAllowEmployerAddQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullResult);

        var controllerResult = await controller.GetAssignAllowEmployerAdd(accountHashedId, reservationId);

        controllerResult.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_BadRequest(
        string accountHashedId,
        Guid reservationId,
        [Frozen] Mock<ISender> mediator,
        [Frozen] Mock<ILogger<DraftApprenticeshipController>> logger,
        [Greedy] CohortController controller)
    {
        mediator.Setup(x => x.Send(
                It.IsAny<GetAssignAllowEmployerAddQuery>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        var controllerResult = await controller.GetAssignAllowEmployerAdd(accountHashedId, reservationId);

        controllerResult.Should().BeOfType<BadRequestResult>();
    }

    [Test, MoqAutoData]
    public async Task Then_Sends_Query_With_ReservationId(
        string accountHashedId,
        Guid reservationId,
        [Frozen] Mock<ISender> mediator,
        [Greedy] CohortController controller)
    {
        var mediatorResult = new GetAssignAllowEmployerAddQueryResult { AllowEmployerAdd = true };
        mediator.Setup(x => x.Send(
                It.IsAny<GetAssignAllowEmployerAddQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        await controller.GetAssignAllowEmployerAdd(accountHashedId, reservationId);

        mediator.Verify(x => x.Send(
                It.Is<GetAssignAllowEmployerAddQuery>(q => q.ReservationId == reservationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
