using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Models;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[Route("[controller]")]
public class LeavingReasonsController : Controller
{
    private readonly IAanHubRestApiClient _apiClient;

    public LeavingReasonsController(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    ///     Get list of leaving reasons
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<LeavingCategory>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLeavingReasons(CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetLeavingReasons(cancellationToken);
        return Ok(response);
    }
}