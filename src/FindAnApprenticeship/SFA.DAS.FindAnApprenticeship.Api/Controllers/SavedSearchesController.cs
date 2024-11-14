using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.SavedSearches;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("saved-searches/")]
public class SavedSearchesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("{id:guid}/unsubscribe")]
    public async Task<IActionResult> GetUnsubscribeSavedSearch([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("unsubscribe")]
    public async Task<IActionResult> PostUnsubscribeSavedSearch([FromBody]PostUnsubscribeSavedSearchApiRequest request)
    {
        throw new NotImplementedException();
    }
}