using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Forecasting.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class CoursesController : ControllerBase
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly IMediator _mediator;

        [HttpGet]
        public async Task<IActionResult> GetStandardsList()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetFrameworksList()
        {

        }
    }
}