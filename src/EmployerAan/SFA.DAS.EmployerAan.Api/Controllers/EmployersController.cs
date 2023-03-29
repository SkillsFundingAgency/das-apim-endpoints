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
    public async Task<IActionResult> Index(Guid userRef)
    {
        var employer = await _mediator.Send(new GetEmployerMemberQuery(userRef));
        return Ok(employer);
    }
}
