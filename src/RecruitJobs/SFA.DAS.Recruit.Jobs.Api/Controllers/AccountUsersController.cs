using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System;

namespace SFA.DAS.Recruit.Jobs.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ApplicationReviewController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ApplicationReviewController> _logger;

    public ApplicationReviewController(IMediator mediator, ILogger<ApplicationReviewController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetApplicationReview()
    {
        return Ok();
    }
}