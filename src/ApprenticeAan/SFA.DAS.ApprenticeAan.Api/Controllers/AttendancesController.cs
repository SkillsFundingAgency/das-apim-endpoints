using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AttendancesController : ControllerBase
{
    private readonly IAanHubRestApiClient _outerApiClient;

    public AttendancesController(IAanHubRestApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate,
        CancellationToken cancellationToken)
    {

        var attendances = await _outerApiClient.GetAttendances(requestedByMemberId, fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"), cancellationToken);
        return new OkObjectResult(attendances);
    }
}
