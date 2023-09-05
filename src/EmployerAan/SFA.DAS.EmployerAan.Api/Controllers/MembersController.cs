using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
using SFA.DAS.EmployerAan.Application.Models;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetMembersQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMembers([FromQuery] GetMembersRequestModel requestModel, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((GetMembersQuery)requestModel, cancellationToken);
        return Ok(response);
    }
}
