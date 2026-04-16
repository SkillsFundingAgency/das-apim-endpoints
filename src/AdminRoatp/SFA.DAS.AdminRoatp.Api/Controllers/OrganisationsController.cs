using System.Net;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Commands.PatchOrganisation;
using SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
using SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class OrganisationsController(IMediator _mediator, ILogger<OrganisationsController> _logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetOrganisationsQueryResult))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
    public async Task<IActionResult> GetOrganisations(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request for GetOrganisations.");
        GetOrganisationsQueryResult response = await _mediator.Send(new GetOrganisationsQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetOrganisationQueryResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
    [Route("{ukprn}")]
    public async Task<IActionResult> GetOrganisation([FromRoute] int ukprn, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to get organisation for Ukprn: {Ukprn}", ukprn);
        GetOrganisationQueryResult? response = await _mediator.Send(new GetOrganisationQuery(ukprn), cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetUkrlpQueryResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("{ukprn}/ukrlp-data")]
    public async Task<IActionResult> GetUkrlp([FromRoute] int ukprn, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to get UKRLP for Ukprn: {Ukprn}", ukprn);
        GetUkrlpQueryResult? response = await _mediator.Send(new GetUkrlpQuery(ukprn), cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("")]
    public async Task<IActionResult> PostOrganisation([FromBody] PostOrganisationCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to Post organisation with ukprn {Ukprn}", command.Ukprn);
        HttpStatusCode response = await _mediator.Send(command, cancellationToken);
        return new StatusCodeResult((int)response);
    }

    [HttpPatch]
    [Route("{ukprn:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PatchOrganisation([FromRoute] int ukprn, [FromBody] JsonPatchDocument<PatchOrganisationModel> patchDoc, [FromHeader(Name = Constants.RequestingUserIdHeader)] string userId, [FromHeader(Name = Constants.RequestingUserNameHeader)] string userName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing Organisations-PatchOrganisations");

        HttpStatusCode response = await _mediator.Send(new PatchOrganisationCommand(ukprn, userId, userName, patchDoc), cancellationToken);

        return new StatusCodeResult((int)response);
    }
}
