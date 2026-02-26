using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourse;
using SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourseList;

namespace SFA.DAS.Reservations.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingCoursesController(
        ILogger<TrainingCoursesController> logger,
        IMediator mediator)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var queryResult = await mediator.Send(new GetTrainingCoursesQuery());

                var model = new GetTrainingCoursesListResponse
                {
                    Standards = queryResult.Courses.Select(c => (GetTrainingCoursesListItem) c).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error attempting to get list of training courses");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTrainingCourse(string id)
        {
            try
            {
                var queryResult = await mediator.Send(new GetTrainingCourseQuery(id));

                if (queryResult == null)
                {
                    return NotFound();
                }
                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error attempting to get training course {0}", id);
                return BadRequest();
            }
        }
    }
}