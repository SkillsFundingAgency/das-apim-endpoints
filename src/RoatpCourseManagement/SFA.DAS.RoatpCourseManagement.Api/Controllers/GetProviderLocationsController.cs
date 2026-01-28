using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAvailableProviderLocations;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetProviderLocationDetails;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Locations")]
[Route("providers/{ukprn}/locations")]
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
    public async Task<IActionResult> GetAllProviderLocations([FromRoute] int ukprn)
    {
        _logger.LogInformation("Request received for all locations for ukprn: {Ukprn}", ukprn);
        var query = new GetAllProviderLocationsQuery(ukprn);
        var result = await _mediator.Send(query);
        if (result.ProviderLocations == null)
        {
            _logger.LogInformation("Invalid ukprn: {Ukprn}", ukprn);
            return BadRequest();
        }
        _logger.LogInformation("Found {LocationCount} locations for ukprn: {Ukprn}", result.ProviderLocations.Count, ukprn);
        return Ok(result.ProviderLocations);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetProviderLocation([FromRoute] int ukprn, [FromRoute] Guid id)
    {
        _logger.LogInformation("Request received for get provider location details for ukprn: {Ukprn} and {ProviderId}", ukprn, id);
        var query = new GetProviderLocationDetailsQuery(ukprn, id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            _logger.LogInformation("Provider Location Details not found for {Ukprn} and navigation id {ProviderId}", ukprn, id);
            return NotFound();
        }

        _logger.LogInformation("Found provider location details for ukprn: {Ukprn} and {ProviderId}", ukprn, id);
        return Ok(result.ProviderLocation);
    }

    [HttpGet]
    [Route("{larsCode}/available-providerlocations")]
    public async Task<IActionResult> GetAvailableProviderLocations([FromRoute] int ukprn, [FromRoute] string larsCode)
    {
        var result = await _mediator.Send(new GetAvailableProviderLocationsQuery(ukprn, larsCode));
        _logger.LogInformation("Total {Count} provider locations are available for ukprn: {Ukprn} larsCode: {LarsCode}", result.AvailableProviderLocations.Count, ukprn, larsCode);
        return Ok(result);
    }
}
