using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.RoatpOversight.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProvidersController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateProvider() => Created(String.Empty, null);
}
