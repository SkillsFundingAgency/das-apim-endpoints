using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Organisation.Queries;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class OrganisationsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Route("{ukprn}")]
    public async Task<IActionResult> Get(int ukprn, CancellationToken cancellationToken)
    {
        GetOrganisationQueryResponse response = await _mediator.Send(new GetOrganisationQuery(ukprn), cancellationToken);
        return Ok(response);
    }
}
