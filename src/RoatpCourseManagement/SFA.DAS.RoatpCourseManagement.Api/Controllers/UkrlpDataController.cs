using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData.Queries.GetUkrlpProviders;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Lookups")]
[Route("")]
public class UkrlpDataController : ControllerBase
{
    private readonly ILogger<UkrlpDataController> _logger;
    private readonly IMediator _mediator;

    public UkrlpDataController(IMediator mediator, ILogger<UkrlpDataController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("lookup/ukrlp/providers")]
    [ProducesResponseType<GetUkrlpProvidersQueryResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProvidersData([FromQuery] DateTime? updatedSinceDate, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request to retrieve course directory data received");

        GetUkrlpProvidersQuery query = new(updatedSinceDate);

        GetUkrlpProvidersQueryResult response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }
}
