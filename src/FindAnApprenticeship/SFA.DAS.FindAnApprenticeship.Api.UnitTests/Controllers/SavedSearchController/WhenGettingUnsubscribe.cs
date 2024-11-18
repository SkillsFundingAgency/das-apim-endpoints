using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.SavedSearches;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SavedSearchController;

public class WhenGettingUnsubscribe
{
    [Test, MoqAutoData]
    public async Task Then_The_Id_Is_Passed_To_Mediator_And_Response_Returned(
        Guid id,
        GetUnsubscribeSavedSearchQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SavedSearchesController savedSearchController)
    {
        mediator.Setup(x => x.Send(It.Is<GetUnsubscribeSavedSearchQuery>(c => c.SavedSearchId == id),
            It.IsAny<CancellationToken>())).ReturnsAsync(queryResult);

        var actual = await savedSearchController.GetUnsubscribeSavedSearch(id) as OkObjectResult;

        actual.Should().NotBeNull();
        actual!.Value.Should().Be(queryResult);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Id_Is_Passed_To_Mediator_And_No_Search_Returned_If_Null_From_Mediator(
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SavedSearchesController savedSearchController)
    {
        mediator.Setup(x => x.Send(It.Is<GetUnsubscribeSavedSearchQuery>(c => c.SavedSearchId == id),
            It.IsAny<CancellationToken>())).ReturnsAsync(new GetUnsubscribeSavedSearchQueryResult(null, new List<GetRoutesListItem>()));

        var actual = await savedSearchController.GetUnsubscribeSavedSearch(id) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualModel = actual!.Value as GetUnsubscribeSavedSearchApiResponse;
        actualModel?.SavedSearch.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Is_Thrown_InternalServerError_Response_Returned(
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SavedSearchesController savedSearchController)
    {
        mediator.Setup(x => x.Send(It.Is<GetUnsubscribeSavedSearchQuery>(c => c.SavedSearchId == id),
            It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        
        var actual = await savedSearchController.GetUnsubscribeSavedSearch(id) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}