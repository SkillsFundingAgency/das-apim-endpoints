using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IAanHubRestApiClient _aanHubRestApiClient;

    public ProfilesController(IAanHubRestApiClient aanHubRestApiClient)
    {
        _aanHubRestApiClient = aanHubRestApiClient;
    }

    [HttpGet]
    [Route("{userType}")]
    [ProducesResponseType(typeof(GetProfilesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfiles([FromRoute] string userType, CancellationToken cancellationToken)
    {
        var response = await _aanHubRestApiClient.GetProfiles(userType, cancellationToken);
        return Ok(response);
    }
}
