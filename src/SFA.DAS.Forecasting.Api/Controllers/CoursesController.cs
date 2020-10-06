using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Courses.Queries.GetFrameworkCoursesList;
using SFA.DAS.Forecasting.Application.Courses.Queries.GetStandardCoursesList;

namespace SFA.DAS.Forecasting.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class CoursesController : ControllerBase
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator, ILogger<CoursesController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("standards")]
        public async Task<IActionResult> GetStandardsList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetStandardCoursesQuery());

                var model = new GetStandardsListResponse
                {
                    Standards = queryResult.Standards.Select(c => (ApprenticeshipCourse)c).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of standards");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("frameworks")]
        public async Task<IActionResult> GetFrameworksList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetFrameworkCoursesQuery());

                var model = new GetFrameworksListResponse
                {
                    Frameworks = queryResult.Frameworks.Select(c => (ApprenticeshipCourse)c).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of frameworks");
                return BadRequest();
            }
        }
    }
}