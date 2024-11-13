using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.SavedSearches;
using SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SearchApprenticeshipsController;

public class WhenPostingASavedSearch
{
    [Test, MoqAutoData]
    public async Task If_The_Request_Is_Valid_Then_Created_Is_Returned(
        Guid candidateId,
        PostSaveSearchApiRequest apiRequest,   
        SaveSearchCommandResult saveSearchCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SFA.DAS.FindAnApprenticeship.Api.Controllers.SearchApprenticeshipsController sut
    )
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.Is<SaveSearchCommand>(cmd =>
                cmd.CandidateId == candidateId
                && cmd.SearchTerm == apiRequest.SearchTerm
                && cmd.DisabilityConfident == apiRequest.DisabilityConfident
                && cmd.Distance == apiRequest.Distance
                && cmd.Location == apiRequest.Location
                && cmd.SelectedLevelIds == apiRequest.SelectedLevelIds
                && cmd.SelectedRouteIds == apiRequest.SelectedRouteIds
            ), CancellationToken.None))
            .ReturnsAsync(saveSearchCommandResult);
        
        // act
        var response = await sut.PostSaveSearch(candidateId, apiRequest);
        
        // assert
        response.Should().BeOfType<CreatedResult>();
    }
    
    [Test, MoqAutoData]
    public async Task If_The_Request_Is_Invalid_Then_BadRequest_Is_Returned(
        Guid candidateId,
        PostSaveSearchApiRequest apiRequest,    
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SFA.DAS.FindAnApprenticeship.Api.Controllers.SearchApprenticeshipsController sut
    )
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.Is<SaveSearchCommand>(cmd =>
                cmd.CandidateId == candidateId
                && cmd.SearchTerm == apiRequest.SearchTerm
                && cmd.DisabilityConfident == apiRequest.DisabilityConfident
                && cmd.Distance == apiRequest.Distance
                && cmd.Location == apiRequest.Location
                && cmd.SelectedLevelIds == apiRequest.SelectedLevelIds
                && cmd.SelectedRouteIds == apiRequest.SelectedRouteIds
            ), CancellationToken.None))
            .ReturnsAsync(SaveSearchCommandResult.None);
        
        // act
        var response = await sut.PostSaveSearch(candidateId, apiRequest) as StatusCodeResult;
        
        // assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
    
    [Test, MoqAutoData]
    public async Task If_An_Exception_Occurs_Then_Internal_Server_Error_Is_Returned(
        Guid candidateId,
        PostSaveSearchApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SearchApprenticeshipsController sut
    )
    {
        // arrange
        var exception = new Exception();
        mediator
            .Setup(x => x.Send(It.IsAny<SaveSearchCommand>(), CancellationToken.None))
            .ThrowsAsync(exception);
        
        // act
        var response = await sut.PostSaveSearch(candidateId, apiRequest) as StatusCodeResult;
        
        // assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}