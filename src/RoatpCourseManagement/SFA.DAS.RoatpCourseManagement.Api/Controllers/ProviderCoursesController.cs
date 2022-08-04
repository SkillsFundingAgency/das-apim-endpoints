using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllProviderCourses;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourse;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class ProviderCoursesController : ControllerBase
    {
        private readonly ILogger<ProviderCoursesController> _logger;
        private readonly IMediator _mediator;

        public ProviderCoursesController(ILogger<ProviderCoursesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("providers/{ukprn}/courses/{larsCode}")]
        public async Task<IActionResult> GetProviderCourse([FromRoute] int ukprn, [FromRoute] int larsCode)
        {
            if (ukprn <= 9999999)
            {
                _logger.LogWarning("Invalid ukprn {ukprn}", ukprn);
                return BadRequest();
            }

            if (larsCode <= 0)
            {
                _logger.LogWarning("Invalid lars code {larsCode}", larsCode);
                return BadRequest();
            }

            var providerCourseResult = await _mediator.Send(new GetProviderCourseQuery(ukprn, larsCode));

            if (providerCourseResult == null)
            {
                _logger.LogError($"Provider Course not found for ukprn {ukprn} and lars code {larsCode}");
                return NotFound();
            }

            return Ok(providerCourseResult);
        }

        [HttpGet]
        [Route("providers/{ukprn}/courses")]
        public async Task<IActionResult> GetAllProviderCourses([FromRoute] int ukprn)
        {
            if (ukprn <= 0)
            {
                _logger.LogInformation("Invalid ukprn number {ukprn} ukprn number has to be a positive number", ukprn);
                return BadRequest();
            }

            _logger.LogInformation("Get Standards for ukprn number {ukprn}", ukprn);
            try
            {
                var result = await _mediator.Send(new GetAllProviderCoursesQuery(ukprn));

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

        [HttpGet]
        [Route("providers/{ukprn}/available-courses")]
        public async Task<IActionResult> GetAllAvailableCourses([FromRoute] int ukprn)
        {
            var result = await _mediator.Send(new GetAvailableCoursesForProviderQuery(ukprn));
            _logger.LogInformation($"Total {result.AvailableCourses.Count} courses are available for ukprn: {ukprn}");
            return Ok(result);
        }
    }
}
