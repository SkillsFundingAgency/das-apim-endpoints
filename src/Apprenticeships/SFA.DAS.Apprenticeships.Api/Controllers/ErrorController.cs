using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Apprenticeships.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("")]
    public IActionResult Error()
    {
        if (HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error is ApiUnauthorizedException)
        {
            _logger.LogError("");
            return Unauthorized();
        }

        // This error is very unlikely to be thrown as .Error should never be null
        _logger.LogError("An unexpected error has occurred. Error in the {p1} is null.", nameof(IExceptionHandlerFeature));
        throw HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error ?? new Exception("An exception has occurred."); 
    }
}