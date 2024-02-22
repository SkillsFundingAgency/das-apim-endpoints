using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEpao.Api.Models;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourse;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpaos;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseList;
using SFA.DAS.SharedOuterApi.Exceptions;

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

        [HttpGet]
        [Route("{id}/epaos")]
        public async Task<IActionResult> CourseEpaos(int id)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetCourseEpaosQuery{CourseId = id});
                
                var model = new GetCourseEpaoListResponse
                {
                    Course = queryResult.Course,
                    Epaos = queryResult.Epaos.Select(item => (GetCourseEpaoListItem)item)
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get list of epaos for course id [{id}]");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}/epaos/{epaoId}")]
        public async Task<IActionResult> CourseEpao(int id, string epaoId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetCourseEpaoQuery
                    {CourseId = id, EpaoId = epaoId});

                var model = new GetCourseEpaoResponse
                {
                    Course = queryResult.Course,
                    StandardVersions = queryResult.StandardVersions
                        .Select(item => (StandardVersions)item),
                    Epao = queryResult.Epao,
                    CourseEpaosCount = queryResult.CourseEpaosCount,
                    EffectiveFrom = queryResult.EffectiveFrom,
                    EpaoDeliveryAreas = queryResult.EpaoDeliveryAreas
                        .Select(area => (EpaoDeliveryArea)area),
                    DeliveryAreas = queryResult.DeliveryAreas
                        .Select(item => (GetDeliveryAreaListItem)item),
                    AllCourses = queryResult.AllCourses
                        .Select(item => (GetAllCoursesListItem)item)
                };

                return Ok(model);
            }
            catch (NotFoundException<GetCourseEpaoResult> e)
            {
                _logger.LogError(e, $"Not found Error attempting to get epao details for course id [{id}], epao id [{epaoId}]");
                return NotFound();
            }
            catch (ValidationException e)
            {
                _logger.LogInformation(e, $"Validation error attempting to get epao details for course id [{id}], epao id [{epaoId}]");
                return BadRequest();
            }
        }
    }
}