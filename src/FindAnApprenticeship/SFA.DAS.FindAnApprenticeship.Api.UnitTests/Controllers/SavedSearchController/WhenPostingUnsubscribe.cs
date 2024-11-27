using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.SavedSearches;
using SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SavedSearchesController;

public class WhenPostingUnsubscribe
{
    [Test, MoqAutoData]
    public async Task Then_The_Id_Is_Passed_To_Mediator_And_Ok_Response_Returned(
        PostUnsubscribeSavedSearchApiRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SavedSearchesController savedSearchController)
    {
        var actual = await savedSearchController.PostUnsubscribeSavedSearch(request) as OkResult;

        actual.Should().NotBeNull();
        mediator.Verify(
            x => x.Send(It.Is<UnsubscribeSavedSearchCommand>(c => c.Id == request.SavedSearchId),
                It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Exception_Is_Thrown_InternalServerError_Response_Returned(
        PostUnsubscribeSavedSearchApiRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SavedSearchesController savedSearchController)
    {
        mediator.Setup(
            x => x.Send(It.Is<UnsubscribeSavedSearchCommand>(c => c.Id == request.SavedSearchId),
                It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        
        var actual = await savedSearchController.PostUnsubscribeSavedSearch(request) as StatusCodeResult;
        
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}