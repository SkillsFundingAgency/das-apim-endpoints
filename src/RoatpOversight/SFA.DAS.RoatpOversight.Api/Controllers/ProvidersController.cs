using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;

namespace SFA.DAS.RoatpOversight.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProvidersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProvidersController> _logger;

    public ProvidersController(IMediator mediator, ILogger<ProvidersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProvider([FromBody] CreateProviderCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Outer API: Request received to create provider course for ukprn: {ukprn} by user: {userId}", command.Ukprn, command.UserId);

        await _mediator.Send(command, cancellationToken);

        return new StatusCodeResult(StatusCodes.Status201Created);
    }
}
