using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ApplicationReviewController(ILogger<ApplicationReviewController> logger) : ControllerBase
{
    private readonly ILogger<ApplicationReviewController> _logger = logger;

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetApplicationReview()
    {
        return Ok();
    }
}