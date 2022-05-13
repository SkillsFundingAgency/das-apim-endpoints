using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.ApiResponses;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCourses;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class CoursesController : ControllerBase
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly IMediator _mediator;

        public CoursesController(ILogger<CoursesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetCoursesQuery());

                return Ok((GetCoursesResponse) result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get courses");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}