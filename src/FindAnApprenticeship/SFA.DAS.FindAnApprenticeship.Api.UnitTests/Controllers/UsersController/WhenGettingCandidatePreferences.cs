using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePreferences;
using System.Threading;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;
public class WhenGettingCandidatePreferences
{
    [Test, MoqAutoData]
    public async Task And_An_Exception_Is_Thrown_Then_Returns_InternalServerError(
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidatePreferencesQuery>(x => x.CandidateId == candidateId), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetCandidatePreferences(candidateId) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Ok_Result(
        Guid candidateId,
        GetCandidatePreferencesQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidatePreferencesQuery>(x => x.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetCandidatePreferences(candidateId);

        actual.Should().BeOfType<OkObjectResult>();
    }
}
