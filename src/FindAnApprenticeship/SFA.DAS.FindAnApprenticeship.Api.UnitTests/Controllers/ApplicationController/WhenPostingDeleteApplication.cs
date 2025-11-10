using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteApplication;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationController;

public class WhenPostingDeleteApplication
{
    [Test, MoqAutoData]
    public async Task ConfirmDelete_Returns_Not_Found(
        Guid candidateId,
        Guid applicationId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.ApplicationController sut)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.Is<DeleteApplicationCommand>(c => c.ApplicationId == applicationId && c.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(DeleteApplicationCommandResult.None);

        // act
        var result = await sut.DeleteApplication(applicationId, candidateId, CancellationToken.None);

        // assert
        result.Should().BeOfType<NotFoundResult>();
    }
    
    [Test, MoqAutoData]
    public async Task ConfirmDelete_Returns_Ok(
        Guid candidateId,
        Guid applicationId,
        DeleteApplicationCommandResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.ApplicationController sut)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.Is<DeleteApplicationCommand>(c => c.ApplicationId == applicationId && c.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        // act
        var result = await sut.DeleteApplication(applicationId, candidateId, CancellationToken.None) as OkObjectResult;
        
        // assert
        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(queryResult);
    }
}