using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Queries.DeleteApplication;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationController;

public class WhenGettingConfirmDeleteApplication
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
            .Setup(x => x.Send(It.Is<ConfirmDeleteApplicationQuery>(q => q.ApplicationId == applicationId && q.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ConfirmDeleteApplicationQueryResult.None);

        // act
        var result = await sut.ConfirmDeleteApplication(applicationId, candidateId, CancellationToken.None);

        // assert
        result.Should().BeOfType<NotFoundResult>();
    }
    
    [Test, MoqAutoData]
    public async Task ConfirmDelete_Returns_Ok(
        Guid candidateId,
        Guid applicationId,
        ConfirmDeleteApplicationQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.ApplicationController sut)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.Is<ConfirmDeleteApplicationQuery>(q => q.ApplicationId == applicationId && q.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        // act
        var result = await sut.ConfirmDeleteApplication(applicationId, candidateId, CancellationToken.None) as OkObjectResult;
        
        // assert
        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(queryResult);
    }
}