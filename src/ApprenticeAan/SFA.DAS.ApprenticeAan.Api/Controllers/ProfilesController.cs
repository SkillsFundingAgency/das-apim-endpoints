using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Get list of profiles by user type
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("{userType}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetProfilesByUserTypeQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfilesByUserType([FromRoute] string userType, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetProfilesByUserTypeQuery(userType), cancellationToken));
    }
}