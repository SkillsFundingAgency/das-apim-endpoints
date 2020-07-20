using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCoursesList;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
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
        public async Task<IActionResult> GetList( [FromQuery] string keyword = "", [FromQuery] List<Guid> routeIds = null, [FromQuery]List<int> levels = null, [FromQuery]int orderBy = 0)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetTrainingCoursesListQuery
                {
                    Keyword = keyword, 
                    RouteIds = routeIds,
                    Levels = levels,
                    OrderBy = orderBy == 0 ? OrderBy.Score : OrderBy.Title
                });
                
                var model = new GetTrainingCoursesListResponse
                {
                    TrainingCourses = queryResult.Courses.Select(response => (GetTrainingCourseListItem)response),
                    Sectors = queryResult.Sectors.Select(response => (GetTrainingSectorsListItem)response),
                    Levels = queryResult.Levels.Select(response => (GetTrainingLevelsListItem)response),
                    Total = queryResult.Total,
                    TotalFiltered = queryResult.TotalFiltered,
                    OrderBy = queryResult.OrderBy
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of training courses");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetTrainingCourseQuery {Id = id});
                var model = new GetTrainingCourseResponse
                {
                    TrainingCourse = result.Course
                };
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get a training course {id}");
                return BadRequest();
            }
        }
    }
}
