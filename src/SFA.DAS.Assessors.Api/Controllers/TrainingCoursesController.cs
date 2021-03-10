using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Assessors.Api.Models;
using SFA.DAS.Assessors.Application.Queries.GetStandardDetails;
using SFA.DAS.Assessors.Application.Queries.GetStandardOptions;
using SFA.DAS.Assessors.Application.Queries.GetTrainingCourses;
using SFA.DAS.Assessors.InnerApi.Responses;

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
            try
            {
                var queryResult = await _mediator.Send(new GetTrainingCoursesQuery());
                
                var model = new GetCourseListResponse
                {
                    Courses = queryResult.TrainingCourses.Select(c=>(GetCourseListItem)c).ToList()
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
        [Route("{standardUId}")]
        public async Task<IActionResult> GetByStandardUId(string standardUId)
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetStandardDetailsQuery(standardUId));

                if (queryResponse.StandardDetails == null) return NotFound();

                return Ok((GetStandardDetailsResponse)queryResponse.StandardDetails);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get standard using {standardUId}");
                return BadRequest();
            }
        }
        
        [Route("options")]
        public async Task<IActionResult> GetStandardOptionsList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetStandardOptionsQuery());

                var model = new GetStandardOptionsResponse
                {
                    StandardOptions = queryResult.StandardOptions.Select(standard => (GetStandardOptionsItem)standard).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {

                _logger.LogError(e, "Error attempting to get list of standard options");
                return BadRequest();
            }
        }
    }
}