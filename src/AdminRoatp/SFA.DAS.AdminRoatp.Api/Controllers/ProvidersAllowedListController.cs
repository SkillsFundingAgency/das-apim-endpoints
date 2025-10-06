using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Queries.GetProvidersAllowedList;
using System.Net;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[Route("providers")]
[ApiController]
public class ProvidersAllowedListController(IMediator _mediator, ILogger<ProvidersAllowedListController> _logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<GetProvidersAllowedListQueryResponse>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
    [Route("allowedlist")]
    public async Task<IActionResult> GetAllowedList([FromQuery] string? sortColumn, [FromQuery] string? sortOrder, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get allowed list request received with sort column {SortColumn} and sort order {SortOrder}", sortColumn, sortOrder);
        GetProvidersAllowedListQueryResponse response = await _mediator.Send(new GetProvidersAllowedListQuery(sortColumn, sortOrder), cancellationToken);
        return Ok(response);
    }
}