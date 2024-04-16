using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.CandidatesController;
public class WhenPostingCandidate
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Post_Response(
        string govIdentifier,
        CandidatesModel model,
        CreateCandidateCommandResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.CandidatesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<CreateCandidateCommand>(x => (x.GovUkIdentifier == govIdentifier) && (x.Email == model.Email)),
        It.IsAny<CancellationToken>()))
        .ReturnsAsync(result);

        var actual = await controller.Index(govIdentifier, model);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            actual.As<OkObjectResult>().Value.Should().BeOfType<CandidateResponse>();
            actual.As<OkObjectResult>().Value.As<CandidateResponse>().Should().BeEquivalentTo(result);
        }
    }
}
