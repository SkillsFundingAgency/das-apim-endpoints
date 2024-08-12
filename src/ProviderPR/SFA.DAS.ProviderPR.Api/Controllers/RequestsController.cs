using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderPR.Application.Requests.Commands;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestsController(IMediator _mediator) : ControllerBase
{
    [HttpPost("addaccount")]
    [ProducesResponseType(typeof(GetProviderRelationshipsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAccount([FromBody] AddAccountRequestCommand command, CancellationToken cancellationToken)
    {
        AddAccountRequestCommandResult result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
