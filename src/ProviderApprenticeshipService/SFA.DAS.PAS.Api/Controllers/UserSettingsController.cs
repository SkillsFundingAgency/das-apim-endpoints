using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PAS.Application.UpsertUserSettings;
using SFA.DAS.PAS.Infrastructure;

namespace SFA.DAS.PAS.Api.Controllers;

[Route("users/{userRef}/settings")]
[ApiController]
public class UserSettingsController(IPasApiClient _pasApiClient) : ControllerBase
{
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpsertUserSettings(
        [FromRoute] string userRef,
        [FromBody] UpsertUserSettingsApiRequest request,
        CancellationToken cancellationToken)
    {
        await _pasApiClient.UpsertUserSettingsAsync(userRef, request, cancellationToken);
        return NoContent();
    }
}
