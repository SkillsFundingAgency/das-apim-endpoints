﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAvailableProviderLocations;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetProviderLocationDetails;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class GetProviderLocationsController : ControllerBase
    {
        private readonly ILogger<GetProviderLocationsController> _logger;
        private readonly IMediator _mediator;

        public GetProviderLocationsController(ILogger<GetProviderLocationsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("providers/{ukprn}/locations")]
        public async Task<IActionResult> GetAllProviderLocations([FromRoute] int ukprn)
        {
            _logger.LogInformation("Request received for all locations for ukprn: {ukprn}", ukprn);
            var query = new GetAllProviderLocationsQuery(ukprn);
            var result = await _mediator.Send(query);
            if (result.ProviderLocations == null)
            {
                _logger.LogInformation("Invalid ukprn: {ukprn}", ukprn);
                return BadRequest();
            }
            _logger.LogInformation($"Found {result.ProviderLocations.Count} locations for ukprn: {ukprn}");
            return Ok(result.ProviderLocations);
        }

        [HttpGet]
        [Route("providers/{ukprn}/locations/{id}")]
        public async Task<IActionResult> GetProviderLocation([FromRoute] int ukprn, [FromRoute] Guid id)
        {
            _logger.LogInformation("Request received for get provider location details for ukprn: {ukprn} and {id}", ukprn, id);
            var query = new GetProviderLocationDetailsQuery(ukprn, id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogInformation("Provider Location Details not found for {ukprn} and navigation id {id}", ukprn, id);
                return NotFound();
            }

            if (result.ProviderLocation == null)
            {
                _logger.LogInformation("Provider Location Details not found for {ukprn} and {id}", ukprn, id);
                return BadRequest();
            }

            _logger.LogInformation($"Found provider location details for ukprn: {ukprn} and {id}");
            return Ok(result.ProviderLocation);
        }

        [HttpGet]
        [Route("providers/{ukprn}/locations/{larsCode}/available-providerlocations")]
        public async Task<IActionResult> GetAvailableProviderLocations([FromRoute] int ukprn, [FromRoute] int larsCode)
        {
            var result = await _mediator.Send(new GetAvailableProviderLocationsQuery(ukprn, larsCode));
            _logger.LogInformation($"Total {result.AvailableProviderLocations.Count} provider locations are available for ukprn: {ukprn} larsCode: {larsCode}");
            return Ok(result);
        }
    }
}
