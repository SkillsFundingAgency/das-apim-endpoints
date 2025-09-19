using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Organisation.Queries;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using System.Net;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class OrganisationsController(IMediator _mediator, ILogger<OrganisationsController> _logger) : ControllerBase
{
    [HttpGet]
    [Route("{ukprn}")]
    [ProducesResponseType(typeof(GetOrganisationQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] int ukprn, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to get organisation with UKPRN: {Ukprn}", ukprn);
        GetOrganisationQueryResponse response = await _mediator.Send(new GetOrganisationQuery(ukprn), cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<GetOrganisationsQueryResponse>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
    public async Task<IActionResult> GetOrganisations([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        GetOrganisationsQueryResponse response = await _mediator.Send(new GetOrganisationsQuery(searchTerm), cancellationToken);
        return Ok(response);
    }
}
