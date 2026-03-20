using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.Application.Providers.Commands;

namespace SFA.DAS.Roatp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProvidersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CharitiesController> _logger;

    public ProvidersController(IMediator mediator, ILogger<CharitiesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [Route("update-names")]
    public async Task<IActionResult> UpdateProviderNames()
    {
        _logger.LogInformation("Updating provider names...");

        await _mediator.Send(new UpdateProviderNamesCommand());

        return Ok();
    }
}
