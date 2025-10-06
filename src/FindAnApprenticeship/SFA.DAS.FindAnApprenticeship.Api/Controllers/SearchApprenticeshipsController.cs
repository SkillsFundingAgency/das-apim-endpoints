using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterestsLocation;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Api.Models.SavedSearches;
using SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch.CreateSaveSearch;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class SearchApprenticeshipsController(ILogger<SearchApprenticeshipsController> logger, IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index([FromQuery] string locationSearchTerm = null, [FromQuery] Guid? candidateId = null)
    {
        try
        {
            var result = await mediator.Send(new SearchIndexQuery
            {
                LocationSearchTerm = locationSearchTerm,
                CandidateId = candidateId
            });
            var viewModel = (SearchIndexApiResponse)result;
            return Ok(viewModel);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error calling Browse By Interests Index");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("browsebyinterests")]
    public async Task<IActionResult> BrowseByInterests()
    {
        try
        {
            var result = await mediator.Send(new BrowseByInterestsQuery());
            var viewModel = (BrowseByInterestsApiResponse)result;
            return Ok(viewModel);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error calling Browse By Interests");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("browsebyinterestslocation")]
    public async Task<IActionResult> BrowseByInterestsLocation([FromQuery] string locationSearchTerm)
    {
        try
        {
            var result = await mediator.Send(new BrowseByInterestsLocationQuery
                { LocationSearchTerm = locationSearchTerm });
            return Ok((BrowseByInterestsLocationApiResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error calling Browse By Interests Location");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("searchResults")]
    [ProducesResponseType(typeof(SearchApprenticeshipsApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchResults(
        [FromQuery] GetSearchApprenticeshipsModel model)
    {
        try
        {
            var result = await mediator.Send((SearchApprenticeshipsQuery)model);

            return Ok((SearchApprenticeshipsApiResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting search results");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
        
    [HttpPost]
    [Route("saved-search")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> PostSaveSearch([FromQuery, Required] Guid candidateId, [FromQuery, Required] Guid id, [FromBody] PostSaveSearchApiRequest apiRequest)
    {
        try
        {
            var response = await mediator.Send(new SaveSearchCommand(
                id,
                candidateId,
                apiRequest.DisabilityConfident,
                apiRequest.ExcludeNational,
                apiRequest.Distance,
                apiRequest.Location,
                apiRequest.SearchTerm,
                apiRequest.SelectedLevelIds,
                apiRequest.SelectedRouteIds,
                apiRequest.UnSubscribeToken,
                apiRequest.ApprenticeshipTypes
            ));

            return response == SaveSearchCommandResult.None
                ? new StatusCodeResult((int)HttpStatusCode.BadRequest)
                : Created();
        }
        catch (Exception e)
        {
            logger.LogError(e, "PostSaveSearch : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}