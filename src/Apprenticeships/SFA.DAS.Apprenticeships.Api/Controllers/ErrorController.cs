using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Apprenticeships.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public IActionResult Error()
    {
        if (HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error is ApiUnauthorizedException)
            return Unauthorized();

        throw HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error ?? new Exception("An exception has occurred."); //this is a terrible error message, but it should never be thrown as .Error should never be null
    }
}