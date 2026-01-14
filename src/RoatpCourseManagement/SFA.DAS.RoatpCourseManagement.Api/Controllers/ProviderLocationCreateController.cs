using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Locations")]
[Route("")]
public class ProviderLocationCreateController : ControllerBase
{
    public readonly IMediator _mediator;
    public readonly ILogger<ProviderLocationCreateController> _logger;

    public ProviderLocationCreateController(IMediator mediator, ILogger<ProviderLocationCreateController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [Route("providers/{ukprn}/locations/create-provider-location")]
    public async Task<IActionResult> CreateProviderLocation([FromRoute] int ukprn, [FromBody] CreateProviderLocationCommand command)
    {
        _logger.LogInformation("Outer API: Request received to create provider location {LocationName} for ukprn: {Ukprn} by user: {UserId}", command.LocationName, ukprn, command.UserId);

        command.Ukprn = ukprn;

        await _mediator.Send(command);

        return new StatusCodeResult(StatusCodes.Status201Created);
    }
}
