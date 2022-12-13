using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    /// This controller is to test pipeline only. This should be removed once we have a functional controller
    [HttpGet]
    [Route("{name}")]
    public IActionResult Greet(string name) => Ok($"Hello {name}");
}
