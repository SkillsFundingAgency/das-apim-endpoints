using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpProviderModeration.Application.Provider.Commands.UpdateProviderDescription;
using SFA.DAS.RoatpProviderModeration.Application.Provider.Queries.GetProvider;

namespace SFA.DAS.RoatpProviderModeration.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProvidersController : ControllerBase
{
    private readonly ILogger<ProvidersController> _logger;
    private readonly IMediator _mediator;

    public ProvidersController(ILogger<ProvidersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{ukprn}")]
    public async Task<IActionResult> GetProvider([FromRoute] int ukprn)
    {
        var providerResult = await _mediator.Send(new GetProviderQuery(ukprn));

        if (providerResult != null) return Ok(providerResult);

        _logger.LogError("Provider not found for ukprn {ukprn}", ukprn);
        return NotFound();
    }

    [HttpPost]
    [Route("{ukprn}/update-provider-description")]
    public async Task<IActionResult> UpdateProviderDescription([FromRoute] int ukprn, UpdateProviderDescriptionCommand command)
    {
        _logger.LogInformation("Outer API: Request to update provider description for ukprn: {ukprn}", ukprn);
        command.Ukprn = ukprn;

        await _mediator.Send(command);

        return NoContent();
    }
}
