using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Organisation.Queries;

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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to get all organisations");
        return Ok(new { Message = "This endpoint is not implemented yet." });
    }
}
