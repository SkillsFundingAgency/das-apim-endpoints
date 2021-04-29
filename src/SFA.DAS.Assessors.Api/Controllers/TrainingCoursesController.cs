using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Assessors.Api.Models;
using SFA.DAS.Assessors.Application.Queries.GetStandardDetails;
using SFA.DAS.Assessors.Application.Queries.GetTrainingCourses;

namespace SFA.DAS.Assessors.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingCoursesController : ControllerBase
    {
        private readonly ILogger<TrainingCoursesController> _logger;
        private readonly IMediator _mediator;

        public TrainingCoursesController (ILogger<TrainingCoursesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var queryResult = await _mediator.Send(new GetTrainingCoursesExportQuery());

            var model = new GetCourseExportListResponse
            {
                Courses = queryResult.TrainingCourses.Select(s => (GetStandardDetailsResponse)s)
            };

            return Ok(model);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStandardById(string id)
        {
            // Id can be standardUId, LarsCode or IfateReferenceNumber
            // It will return one record, either uniquely via the StandardUid
            // Or the latest active via LarsCode / IFateReferenceNumber
            var queryResponse = await _mediator.Send(new GetStandardDetailsQuery(id));

            if (queryResponse.StandardDetails == null) return NotFound();

            return Ok((GetStandardDetailsResponse)queryResponse.StandardDetails);
        }
        

        [HttpGet]
        [Route("active")]
        public async Task<IActionResult> GetActiveList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetActiveTrainingCoursesQuery());

                var model = new GetCourseListResponse
                {
                    Courses = queryResult.TrainingCourses.Select(c => (GetCourseListItem)c).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of training courses");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("draft")]
        public async Task<IActionResult> GetDraftList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetDraftTrainingCoursesQuery());

                var model = new GetCourseListResponse
                {
                    Courses = queryResult.TrainingCourses.Select(c => (GetCourseListItem)c).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of draft training courses");
                return BadRequest();
            }
        }
    }
}