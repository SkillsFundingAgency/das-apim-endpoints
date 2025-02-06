using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseLevels;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseRoutes;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public sealed class CoursesController(IMediator _mediator, ILogger<CoursesController> _logger) : ControllerBase
{
    [HttpGet]
    [Route("levels")]
    public async Task<IActionResult> GetCourseLevels()
    {
        var response = await _mediator.Send(new GetCourseLevelsQuery());
        return Ok(response);
    }

    [HttpGet]
    [Route("routes")]
    public async Task<IActionResult> GetCourseRoutes()
    {
        var result = await _mediator.Send(new GetCourseRoutesQuery());
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}/providers")]
    public async Task<IActionResult> GetProviders(int id, [FromQuery] GetCourseProvidersRequest request)
    {
        try
        {
            var result = await _mediator.Send(new GetTrainingCourseProvidersQuery
            {
                Id = id,
                OrderBy = request.OrderBy,
                Distance = request.Distance,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                DeliveryModes = request.DeliveryModes,
                EmployerProviderRatings = request.EmployerProviderRatings,
                ApprenticeProviderRatings = request.ApprenticeProviderRatings,
                Qar = request.Qar,
                Page = request.Page,
                PageSize = request.PageSize,
                ShortlistUserId = request.ShortlistUserId
            });

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error attempting to get providers for course {id}");
            return BadRequest();
        }
    }
}
