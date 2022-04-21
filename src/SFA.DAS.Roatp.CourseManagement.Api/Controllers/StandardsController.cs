using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Queries.GetCourseQuery;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class StandardsController : ControllerBase
    {
        private readonly ILogger<StandardsController> _logger;
        private readonly IMediator _mediator;

        public StandardsController(ILogger<StandardsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{ukprn}")]
        public async Task<IActionResult> GetAllCourses(int ukprn)
        {
            if (ukprn <= 0)
            {
                _logger.LogInformation("Invalid ukprn number {ukprn} ukprn number has to be a positive number", ukprn);
                return BadRequest();
            }

            _logger.LogInformation("Get Standards for ukprn number {ukprn}", ukprn);
            try
            {
                var result = await _mediator.Send(new GetAllCoursesQuery(ukprn));

                if (result == null)
                {
                    _logger.LogInformation("Standards data not found for ukprn number {ukprn}", ukprn);
                    return NotFound();
                }

                _logger.LogInformation("Standards data found for ukprn number {ukprn}", ukprn);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred trying to retrieve Standards data for ukprn number {ukprn}");
                return BadRequest();
            }
        }
    }
}
