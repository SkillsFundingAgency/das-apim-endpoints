using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgressInternal.Application.Services;

namespace SFA.DAS.TrackProgressInternal.Api.Controllers;

[ApiController]
public class TrackProgressController : ControllerBase
{
    [HttpPost]
    [Route("/apprenticeships/{commitmentsApprenticeshipId}/snapshot")]
    public async Task<IActionResult> CreateProgressSnapshot(
        [FromServices] ResponseReturningApiClient client,
        long commitmentsApprenticeshipId)
    {
        return await client.Post($"apprenticeships/{commitmentsApprenticeshipId}/AggregateProgress");
    }
}