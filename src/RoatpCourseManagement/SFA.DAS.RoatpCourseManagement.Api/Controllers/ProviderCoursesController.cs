using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllProviderCourses;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourse;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Courses")]
[Route("")]
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
    public async Task<IActionResult> GetProviderCourse([FromRoute] int ukprn, [FromRoute] string larsCode)
    {
        if (ukprn <= 9999999)
        {
            _logger.LogWarning("Invalid ukprn {Ukprn}", ukprn);
            return BadRequest();
        }

        if (larsCode == string.Empty)
        {
            _logger.LogWarning("Invalid lars code {LarsCode}", larsCode);
            return BadRequest();
        }

        var providerCourseResult = await _mediator.Send(new GetProviderCourseQuery(ukprn, larsCode));

        if (providerCourseResult == null)
        {
            _logger.LogError("Provider Course not found for ukprn {Ukprn} and lars code {LarsCode}", ukprn, larsCode);
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
            _logger.LogInformation("Invalid ukprn number {Ukprn} ukprn number has to be a positive number", ukprn);
            return BadRequest();
        }

        _logger.LogInformation("Get Standards for ukprn number {Ukprn}", ukprn);
        try
        {
            var result = await _mediator.Send(new GetAllProviderCoursesQuery(ukprn));

            if (result == null)
            {
                _logger.LogInformation("Standards data not found for ukprn number {Ukprn}", ukprn);
                return NotFound();
            }

            _logger.LogInformation("Standards data found for ukprn number {Ukprn}", ukprn);
            return Ok(result);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred trying to retrieve Standards data for ukprn number {Ukprn}", ukprn);
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("providers/{ukprn}/available-courses/{courseType}")]
    public async Task<IActionResult> GetAllAvailableCourses([FromRoute] int ukprn, [FromRoute] CourseType courseType)
    {
        var result = await _mediator.Send(new GetAvailableCoursesForProviderQuery(ukprn, courseType));
        _logger.LogInformation("Total {AvailableCoursesCount} courses are available for ukprn: {Ukprn}", result.AvailableCourses.Count, ukprn);
        return Ok(result);
    }
}