using System.Linq;
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

    [HttpPost]
    [Route("lookup/ukrlp/providers")]
    [ProducesResponseType<GetUkrlpProvidersQueryResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProvidersData([FromBody] GetUkrlpProvidersQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request to retrieve course directory data received");

        if (!query.Ukprns.Any()) return BadRequest("No UKPRNs provided");

        GetUkrlpProvidersQueryResult response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }
}
