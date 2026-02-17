using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.AparRegister.Api.Models;
using SFA.DAS.AparRegister.Application.Queries.ProviderRegister;

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
}
