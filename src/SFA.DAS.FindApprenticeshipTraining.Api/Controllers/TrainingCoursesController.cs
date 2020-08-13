using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCoursesList;
using SFA.DAS.FindApprenticeshipTraining.Application;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

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
        public async Task<IActionResult> GetList( [FromQuery] string keyword = "", [FromQuery] List<Guid> routeIds = null, [FromQuery]List<int> levels = null, [FromQuery]string orderBy = "relevance")
        {
            try
            {
                var queryResult = await _mediator.Send(new GetTrainingCoursesListQuery
                {
                    Keyword = keyword, 
                    RouteIds = routeIds,
                    Levels = levels,
                    OrderBy = orderBy.Equals("relevance", StringComparison.CurrentCultureIgnoreCase) ? OrderBy.Score : OrderBy.Title
                });
                
                var model = new GetTrainingCoursesListResponse
                {
                    TrainingCourses = queryResult.Courses.Select(response => (GetTrainingCourseListItem)response),
                    Sectors = queryResult.Sectors.Select(response => (GetTrainingSectorsListItem)response),
                    Levels = queryResult.Levels.Select(response => (GetTrainingLevelsListItem)response),
                    Total = queryResult.Total,
                    TotalFiltered = queryResult.TotalFiltered
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
        
        [HttpGet]
        [Route("{id}/providers")]
        public async Task<IActionResult> GetProviders(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetTrainingCourseProvidersQuery {Id = id});
                var model = new GetTrainingCourseProvidersResponse
                {
                    TrainingCourse = result.Course,
                    TrainingCourseProviders = result.Providers
                        .Select(c=> new GetTrainingCourseProviderListItem().Map(c,result.Course.SectorSubjectAreaTier2Description, result.Course.Level)).ToList(),
                    Total = result.Total
                };
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get a training course {id}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}/providers/{providerId}")]
        public async Task<IActionResult> GetProviderCourse(int id, int providerId)
        {
            try
            {
                var result = await _mediator.Send(new GetTrainingCourseProviderQuery
                {
                    CourseId = id, 
                    ProviderId = providerId
                });
                var model = new GetTrainingCourseProviderResponse
                {
                    TrainingCourse = result.Course,
                    TrainingCourseProvider = new GetProviderCourseItem().Map(result,result.Course.SectorSubjectAreaTier2Description, result.Course.Level),
                    AdditionalCourses = result.AdditionalCourses
                };
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get a training course {id} with provider {providerId}");
                return BadRequest();
            }
        }
    }
}
