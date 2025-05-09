using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.LearnerData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InfoController() : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        { 
            return Ok("Leaner data APIM for the apprenticeship service - Version 1.0.0");
        }
    }
}
