using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.InvalidIlrChanges.Commands;
using SFA.DAS.Approvals.Application.InvalidIlrChanges.Queries;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices;

public class WhenGettingAndPostingInvalidIlrChanges
{
    [Test, MoqAutoData]
    public async Task Get_ThenReturnsTheAggregatedPageModel(
        long providerId,
        long apprenticeshipId,
        GetInvalidIlrChangesResponse mediatorResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApprenticesController controller)
    {
        mediator.Setup(m => m.Send(
                It.Is<GetInvalidIlrChangesQuery>(query =>
                    query.ProviderId == providerId &&
                    query.ApprenticeshipId == apprenticeshipId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var result = await controller.GetInvalidIlrChanges(providerId, apprenticeshipId) as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task Get_ThenReturnsNotFoundWhenTheQueryReturnsNull(
        long providerId,
        long apprenticeshipId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApprenticesController controller)
    {
        mediator.Setup(m => m.Send(It.IsAny<GetInvalidIlrChangesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetInvalidIlrChangesResponse)null);

        var result = await controller.GetInvalidIlrChanges(providerId, apprenticeshipId);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task Post_ThenSendsTheAcknowledgeCommand(
        long providerId,
        long apprenticeshipId,
        AcknowledgeInvalidIlrChangesApiRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApprenticesController controller)
    {
        mediator.Setup(m => m.Send(It.IsAny<AcknowledgeInvalidIlrChangesCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await controller.AcknowledgeInvalidIlrChanges(providerId, apprenticeshipId, request);

        result.Should().BeOfType<OkResult>();
        mediator.Verify(m => m.Send(
            It.Is<AcknowledgeInvalidIlrChangesCommand>(command =>
                command.ProviderId == providerId &&
                command.ApprenticeshipId == apprenticeshipId &&
                command.UserInfo == request.UserInfo &&
                command.Acknowledgements == request.Acknowledgements),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
