using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DeleteCandidate;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DeleteSavedSearch;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearch;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;

public class WhenDeletingCandidateSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Search_Is_Deleted(
        Guid candidateId,
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController sut
    )
    {
        // arrange
        DeleteSavedSearchCommand passedCommand = null;
        mediator
            .Setup(x => x.Send(It.IsAny<DeleteSavedSearchCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest, CancellationToken>((x, _) => passedCommand = x as DeleteSavedSearchCommand);
        
        // act
        var response = await sut.DeleteUserSavedSearch(candidateId, id, default) as OkResult;
        
        // assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        passedCommand.CandidateId.Should().Be(candidateId);
        passedCommand.Id.Should().Be(id);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Exceptions_Are_Handled(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController sut
    )
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.IsAny<DeleteSavedSearchCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());
        
        // act
        var response = await sut.DeleteUserSavedSearch(Guid.NewGuid(), Guid.NewGuid(), default) as StatusCodeResult;
        
        // assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}