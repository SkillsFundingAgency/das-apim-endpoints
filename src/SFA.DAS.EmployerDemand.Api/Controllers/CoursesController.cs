using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Courses.Queries.GetCourse;

namespace SFA.DAS.EmployerDemand.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class CoursesController : ControllerBase
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly IMediator _mediator;

        public CoursesController(
            ILogger<CoursesController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetCourseQuery{CourseId = id});
                
                var model = new GetCourseResponse
                {
                    Course = queryResult.Course
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get course id [{id}]");
                return BadRequest();
            }
        }
    }
}