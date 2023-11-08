using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterestsLocation;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;

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
                var result = await _mediator.Send(new SearchApprenticeshipsQuery());
                var viewModel = (SearchApprenticeshipsApiResponse)result;
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
        public async Task<IActionResult> SearchResults([FromQuery] List<string>? routeIds, [FromQuery] string? location,
            [FromQuery] int? distance)
        {
            try
            {
                var result = await _mediator.Send(new SearchApprenticeshipsQuery
                    { Location = location, Distance = distance, SelectedRouteIds = routeIds });
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