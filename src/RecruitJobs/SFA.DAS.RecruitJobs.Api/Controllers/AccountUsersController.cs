using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[ApiController, AllowAnonymous]
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