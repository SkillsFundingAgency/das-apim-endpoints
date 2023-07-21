using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Api.Controllers;
[Route("[controller]")]
public class AttendancesController
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
