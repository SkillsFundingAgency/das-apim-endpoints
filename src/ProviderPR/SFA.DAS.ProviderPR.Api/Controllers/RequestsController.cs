using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderPR.Application.Requests.Commands;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestsController(IMediator _mediator) : ControllerBase
{
    [HttpPost("addaccount")]
    [ProducesResponseType(typeof(AddAccountRequestCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAccount([FromBody] AddAccountRequestCommand command, CancellationToken cancellationToken)
    {
        AddAccountRequestCommandResult result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
