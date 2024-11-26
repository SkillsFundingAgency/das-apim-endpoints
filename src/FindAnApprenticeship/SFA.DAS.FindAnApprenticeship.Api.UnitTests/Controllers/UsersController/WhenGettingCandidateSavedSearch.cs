using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearch;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;

public class WhenGettingCandidateSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Search_Is_Returned(
        Guid candidateId,
        Guid id,
        GetCandidateSavedSearchQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController sut
        )
    {
        // arrange
        GetCandidateSavedSearchQuery passedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetCandidateSavedSearchQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetCandidateSavedSearchQueryResult>, CancellationToken>((x, _) => passedQuery = x as GetCandidateSavedSearchQuery)
            .ReturnsAsync(queryResult);
        
        // act
        var response = await sut.GetUserSavedSearch(candidateId, id, default) as OkObjectResult;
        
        // assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Value.Should().BeEquivalentTo(queryResult);
        passedQuery.CandidateId.Should().Be(candidateId);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Exceptions_Are_Handled(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController sut
    )
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetCandidateSavedSearchQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());
        
        // act
        var response = await sut.GetUserSavedSearch(Guid.NewGuid(), Guid.NewGuid(), default) as StatusCodeResult;
        
        // assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}