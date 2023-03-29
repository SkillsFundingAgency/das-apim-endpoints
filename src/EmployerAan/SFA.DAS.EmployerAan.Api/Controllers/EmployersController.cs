using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployersController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{userRef}")]
    [ProducesResponseType(typeof(GetEmployerMemberQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployerMember(Guid userRef, CancellationToken cancellationToken)
    {
        var employer = await _mediator.Send(new GetEmployerMemberQuery(userRef), cancellationToken);
        return employer is null ? NotFound() : Ok(employer);
    }
}
