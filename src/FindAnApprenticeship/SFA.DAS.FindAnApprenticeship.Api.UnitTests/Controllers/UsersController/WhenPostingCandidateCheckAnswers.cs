using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Threading;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.CheckAnswers;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;
public class WhenPostingCandidateCheckAnswers
{
    [Test, MoqAutoData]
    public async Task And_An_Exception_Is_Thrown_Then_Returns_InternalServerError(
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<UpdateCheckAnswersCommand>(x => x.CandidateId == candidateId), CancellationToken.None)
            ).Throws<Exception>();

        var actual = await controller.PostCheckAnswers(candidateId) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Ok_Result(
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<UpdateCheckAnswersCommand>(x => x.CandidateId == candidateId), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var actual = await controller.PostCheckAnswers(candidateId);

        actual.Should().BeOfType<OkResult>();
    }
}
