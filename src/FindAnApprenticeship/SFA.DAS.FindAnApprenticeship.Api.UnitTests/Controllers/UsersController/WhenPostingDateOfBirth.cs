using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using System.Net;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DateOfBirth;
using System.Threading;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;
public class WhenPostingDateOfBirth
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Put_Response(
        Guid candidateId,
        CandidatesDateOfBirthModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        var actual = await controller.DateOfBirth(candidateId, model) as OkObjectResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mediator.Verify(x => x.Send(It.Is<UpsertDateOfBirthCommand>(
            c => c.CandidateId.Equals(candidateId)
            && c.DateOfBirth.Equals(model.DateOfBirth)
            && c.Email.Equals(model.Email)
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_Exception(
        Guid candidateId,
        CandidatesDateOfBirthModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<UpsertDateOfBirthCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var actual = await controller.DateOfBirth(candidateId, model) as StatusCodeResult;

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
    }
}
