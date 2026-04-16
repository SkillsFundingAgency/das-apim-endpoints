using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisationTypes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using System.Net;

namespace SFA.DAS.AdminRoatp.Api.Controllers;
[Route("organisation-types")]
[ApiController]
public class OrganisationTypesController(IMediator _mediator, ILogger<OrganisationTypesController> _logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetOrganisationTypesResponse))]
    public async Task<IActionResult> GetOrganisationTypes(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to GetOrganisationTypes.");
        GetOrganisationTypesResponse result = await _mediator.Send(new GetOrganisationTypesQuery(), cancellationToken);
        return Ok(result);
    }
}