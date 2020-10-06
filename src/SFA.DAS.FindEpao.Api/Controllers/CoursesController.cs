using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEpao.Api.Models;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseList;

namespace SFA.DAS.FindEpao.Api.Controllers
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
        public async Task<IActionResult> GetList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetCourseListQuery());
                
                var model = new GetCourseListResponse
                {
                    Courses = queryResult.Courses.Select(c=>(GetCourseListItem)c).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of training courses");
                return BadRequest();
            }
        }
    }
}