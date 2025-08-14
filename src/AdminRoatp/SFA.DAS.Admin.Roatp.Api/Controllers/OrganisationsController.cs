using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Admin.Roatp.Application.Organisation.Queries;

namespace SFA.DAS.Admin.Roatp.Api.Controllers;

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
