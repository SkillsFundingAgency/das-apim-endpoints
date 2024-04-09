using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ProviderFeedback.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var response = new
            {
                Message = "This is a sample API response.",
                Timestamp = DateTime.UtcNow
            };

            return Ok(response);
        }
    }
}