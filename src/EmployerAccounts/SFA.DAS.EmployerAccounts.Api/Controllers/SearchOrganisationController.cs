using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;

namespace SFA.DAS.EmployerAccounts.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class SearchOrganisationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SearchOrganisationController> _logger;

        public SearchOrganisationController(IMediator mediator, ILogger<SearchOrganisationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{searchTerm}/levy/english-fraction-history")]
        public async Task<IActionResult> SearchOrganisations(string searchTerm, [FromQuery] int maximumResults = 500)
        {
            try
            {
                var result = await _mediator.Send(new SearchOrganisationsQuery()
                {
                   SearchTerm = searchTerm,
                   MaximumResults = maximumResults
                });

                if (result == null)
                {
                    return NotFound();
                }

                var model = (GetEnglishFractionResponse)result;

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting english fraction history");
                return BadRequest();
            }
        }

    }
}
