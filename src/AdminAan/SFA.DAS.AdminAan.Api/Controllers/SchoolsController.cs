using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Application.Schools.Queries;

namespace SFA.DAS.AdminAan.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class SchoolsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SchoolsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("find/{searchTerm}")]
    [ProducesResponseType(typeof(List<School>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSchools(string searchTerm, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetSchoolsQuery { SearchTerm = searchTerm }, cancellationToken);
        return Ok(new GetSchoolsQueryResult(((GetSchoolsQueryApiResult)response!).Data));
    }
}