using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MembersController : ControllerBase
{
    public readonly IAanHubRestApiClient _apiClient;

    public MembersController(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetMembersResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMembers([FromQuery] GetMembersRequest requestModel, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetMembers(Request.QueryString.ToString(), cancellationToken);
        return Ok(response);
    }
}
