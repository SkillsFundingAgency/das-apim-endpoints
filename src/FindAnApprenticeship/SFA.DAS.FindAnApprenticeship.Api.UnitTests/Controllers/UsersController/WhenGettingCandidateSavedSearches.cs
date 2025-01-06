﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearches;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;

public class WhenGettingCandidateSavedSearches
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Searches_Are_Returned(
        Guid candidateId,
        GetCandidateSavedSearchesQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController sut
        )
    {
        // arrange
        GetCandidateSavedSearchesQuery passedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetCandidateSavedSearchesQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetCandidateSavedSearchesQueryResult>, CancellationToken>((x, _) => passedQuery = x as GetCandidateSavedSearchesQuery)
            .ReturnsAsync(queryResult);
        
        // act
        var response = await sut.GetUserSavedSearches(candidateId, default) as OkObjectResult;
        
        // assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Value.Should().BeEquivalentTo(queryResult);
        passedQuery.CandidateId.Should().Be(candidateId);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Exceptions_Are_Handled(
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController sut
    )
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetCandidateSavedSearchesQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());
        
        // act
        var response = await sut.GetUserSavedSearches(candidateId, default) as StatusCodeResult;
        
        // assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}