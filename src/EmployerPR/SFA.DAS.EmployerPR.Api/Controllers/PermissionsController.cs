using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Commands.PostPermissions;
using SFA.DAS.EmployerPR.Application.Queries.GetPermissions;


namespace SFA.DAS.EmployerPR.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PermissionsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetPermissionsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPermissions([FromQuery] GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Unit), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PostPermissions([FromBody] PostPermissionsCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}