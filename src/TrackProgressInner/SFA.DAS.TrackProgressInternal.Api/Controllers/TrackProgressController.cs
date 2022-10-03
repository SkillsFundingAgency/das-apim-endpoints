using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgressInternal.Application.Services;

namespace SFA.DAS.TrackProgressInternal.Api.Controllers;

[ApiController]
public class TrackProgressController : ControllerBase
{
    private readonly ResponseReturningApiClient _client;

    public TrackProgressController(ResponseReturningApiClient client)
        => _client = client;

    [HttpPost]
    [Route("/apprenticeships/{commitmentsApprenticeshipId}/AggregateProgress")]
    public async Task<IActionResult> AggregateProgress(long commitmentsApprenticeshipId)
        => await _client.Post<object>($"apprenticeships/{commitmentsApprenticeshipId}/AggregateProgress", null);
}