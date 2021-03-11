using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRegisterDemand;

namespace SFA.DAS.EmployerDemand.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class DemandController : ControllerBase
    {
        private readonly ILogger<DemandController> _logger;
        private readonly IMediator _mediator;

        public DemandController(
            ILogger<DemandController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("create/{courseId}")]
        public async Task<IActionResult> Create(int courseId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetRegisterDemandQuery{CourseId = courseId});
                
                var model = new GetCourseResponse
                {
                    TrainingCourse = queryResult.Course
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get course id [{courseId}]");
                return BadRequest();
            }
        }
    }
}