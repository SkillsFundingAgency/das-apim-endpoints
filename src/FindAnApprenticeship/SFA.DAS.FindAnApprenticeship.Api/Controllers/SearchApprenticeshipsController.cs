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
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class SearchApprenticeshipsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SearchApprenticeshipsController> _logger;

        public SearchApprenticeshipsController(IMediator mediator, ILogger<SearchApprenticeshipsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _mediator.Send(new SearchIndexQuery());
                var viewModel = (SearchIndexApiResponse)result;
                return Ok(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error calling Browse By Interests Index");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

        }

        [HttpGet]
        [Route("browsebyinterests")]
        public async Task<IActionResult> BrowseByInterests()
        {
            try
            {
                var result = await _mediator.Send(new BrowseByInterestsQuery());
                var viewModel = (BrowseByInterestsApiResponse)result;
                return Ok(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error calling Browse By Interests");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("browsebyinterestslocation")]
        public async Task<IActionResult> BrowseByInterestsLocation([FromQuery] string locationSearchTerm)
        {
            try
            {
                var result = await _mediator.Send(new BrowseByInterestsLocationQuery
                    { LocationSearchTerm = locationSearchTerm });
                return Ok((BrowseByInterestsLocationApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error calling Browse By Interests Location");
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
                var result = await _mediator.Send((SearchApprenticeshipsQuery)model);

                return Ok((SearchApprenticeshipsApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting search results");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}