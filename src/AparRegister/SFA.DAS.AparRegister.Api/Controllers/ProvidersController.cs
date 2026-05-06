using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.AparRegister.Api.Models;
using SFA.DAS.AparRegister.Application.Queries.ProviderRegister;
using SFA.DAS.AparRegister.Application.Queries.ProviderStatusEvents;
using SFA.DAS.AparRegister.InnerApi.Responses;

namespace SFA.DAS.AparRegister.Api.Controllers;

/// <summary>
/// Provides API endpoints for retrieving information about registered providers.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ProvidersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProvidersController> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    public ProvidersController(IMediator mediator, ILogger<ProvidersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves the list of registered providers.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing a <see cref="ProvidersApiResponse"/> with the collection of registered
    /// providers. Returns a 200 OK response on success.</returns>
    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(ProvidersApiResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProviders(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting Providers");
        GetProvidersQueryResult result = await _mediator.Send(new GetProvidersQuery(), cancellationToken);
        ProvidersApiResponse providersApiResponse = new()
        {
            Providers = result.RegisteredProviders.Select(o => (ProviderModel)o)
        };

        return Ok(providersApiResponse);
    }

    /// <summary>
    /// Retrieves a paginated list of provider status events that have occurred since the specified event ID.
    /// </summary>
    /// <remarks>Use this endpoint to poll for new provider status events in a paginated manner. If the provided parameters are outside their valid ranges, default values are applied.</remarks>
    /// <param name="sinceEventId">The ID of the last received event. Only events with an ID greater than this value are returned. Must be zero or greater.</param>
    /// <param name="pageSize">The maximum number of events to return in the response. Must be between 1 and 1000. Defaults to 1000.</param>
    /// <param name="pageNumber">The page number of results to retrieve. Must be 1 or greater. Defaults to 1.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="IActionResult"/> containing a collection of <see cref="ProviderStatusEvent"/> objects that match the specified criteria.</returns>
    [HttpGet]
    [Route("status-events")]
    [ProducesResponseType(typeof(IEnumerable<ProviderStatusEvent>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProviderStatusEvents([FromQuery] int sinceEventId = 0, [FromQuery] int pageSize = 1000, [FromQuery] int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting Provider Status Events");
        sinceEventId = sinceEventId < 0 ? 0 : sinceEventId;
        pageSize = pageSize is < 1 or > 1000 ? 1000 : pageSize;
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        IEnumerable<ProviderStatusEvent> result = await _mediator.Send(new GetProviderStatusEventsQuery(sinceEventId, pageSize, pageNumber), cancellationToken);
        return Ok(result);
    }
}
