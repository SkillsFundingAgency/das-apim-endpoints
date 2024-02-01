﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Api.Models;
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
        [Route("")]
        public async Task<IActionResult> SearchOrganisations([FromQuery] string searchTerm, [FromQuery] int maximumResults = 500)
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

                var model = (SearchOrganisationsResponse)result;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Searching for Organisations");
                return BadRequest();
            }
        }
    }
}
