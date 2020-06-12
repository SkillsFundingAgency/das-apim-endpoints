using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers
{
    [ApiController]
    [Route("findapprenticeshiptraining/[controller]/")]
    public class TrainingCoursesController : ControllerBase
    {
        private readonly ILogger<TrainingCoursesController> _logger;
        private readonly IMediator _mediator;

        public TrainingCoursesController(
            ILogger<TrainingCoursesController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            await Task.CompletedTask;
            return Ok("coming soon");
        }
    }
}
