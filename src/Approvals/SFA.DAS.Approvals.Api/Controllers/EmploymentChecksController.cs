using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Requests;
using SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class EmploymentChecksController(IEmploymentCheckApiClient<EmploymentCheckConfiguration> client) : ControllerBase
{
    private const int MaxApprenticeshipIds = 1000;

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get([FromQuery] List<long> apprenticeshipIds)
    {
        if (apprenticeshipIds == null || apprenticeshipIds.Count == 0)
        {
            return BadRequest("apprenticeshipIds is required and must not be empty.");
        }

        if (apprenticeshipIds.Count > MaxApprenticeshipIds)
        {
            return BadRequest($"apprenticeshipIds must not exceed {MaxApprenticeshipIds}.");
        }

        var ids = apprenticeshipIds.AsReadOnly();
        var request = new GetEmploymentCheckLearnersRequest(ids);
        var response = await client.GetWithResponseCode<List<EvsCheckResponse>>(request);

        var code = (int)response.StatusCode;
        if (code is < 200 or >= 300)
        {
            return StatusCode(code, response.ErrorContent ?? "Employment check API error.");
        }

        return Ok(response.Body ?? []);
    }
}
