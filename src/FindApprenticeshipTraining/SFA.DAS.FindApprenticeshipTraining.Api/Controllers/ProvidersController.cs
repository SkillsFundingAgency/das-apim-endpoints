using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProvidersController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetRoatpProvidersQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProviders([FromQuery] bool? Live, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetRoatpProvidersQuery() { Live = Live ?? false }, cancellationToken));
    }

    [HttpGet]
    [Route("{ukprn:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetProviderSummaryQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProviderSummary([FromRoute] int ukprn, CancellationToken cancellationToken)
    {
        return Ok(
            await _mediator.Send(new GetProviderSummaryQuery(ukprn), cancellationToken)
        );
    }
}