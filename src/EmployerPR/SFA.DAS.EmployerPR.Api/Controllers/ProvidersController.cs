using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Queries.GetRoatpProviders;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProvidersController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetRoatpProvidersQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProviders(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetRoatpProvidersQuery(), cancellationToken));
    }
}
