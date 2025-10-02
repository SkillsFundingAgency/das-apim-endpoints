using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using System.Net;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class OrganisationsController(IMediator _mediator, ILogger<OrganisationsController> _logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<GetOrganisationsQueryResponse>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
    public async Task<IActionResult> GetOrganisations([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to get organisations with Search Term: {SearchTerm}", searchTerm);
        GetOrganisationsQueryResponse response = await _mediator.Send(new GetOrganisationsQuery(searchTerm), cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetOrganisationResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
    [Route("ukprn")]
    public async Task<IActionResult> GetOrganisation([FromRoute] int ukprn, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to get organisation for Ukprn: {Ukprn}", ukprn);
        GetOrganisationResponse? response = await _mediator.Send(new GetOrganisationQuery(ukprn), cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }
}