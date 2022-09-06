using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Models;

namespace SFA.DAS.Apprenticeships.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrainingCoursesController : ControllerBase
    {
        private readonly ILogger<TrainingCoursesController> _logger;
        private readonly IMediator _mediator;

        public TrainingCoursesController(ILogger<TrainingCoursesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("standards/{courseCode}")]
        public async Task<IActionResult> GetStandard(string courseCode)
        {
            try
            {
                //var queryResult = await _mediator.Send(new GetStandardQuery(courseCode)); //todo commented out to allow deployment work to start

                //if (queryResult == null)
                //{
                //    return NotFound();
                //}

                //var model = (GetStandardResponse)queryResult;

                var model = new GetStandardResponse();
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of standards");
                return BadRequest();
            }
        }
    }
}