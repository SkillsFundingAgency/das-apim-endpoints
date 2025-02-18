using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseLevels;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseRoutes;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public sealed class CoursesController(IMediator _mediator) : ControllerBase
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
    public async Task<IActionResult> GetCourseProviders(int id, [FromQuery] GetCourseProvidersModel model)
    {

        var result = await _mediator.Send(new GetCourseProvidersQuery
        {
            Id = id,
            OrderBy = model.OrderBy,
            Distance = model.Distance,
            Location = model.Location,
            DeliveryModes = model.DeliveryModes,
            EmployerProviderRatings = model.EmployerProviderRatings,
            ApprenticeProviderRatings = model.ApprenticeProviderRatings,
            Qar = model.Qar,
            Page = model.Page,
            PageSize = model.PageSize,
            ShortlistUserId = model.ShortlistUserId
        });

        return Ok(result);
    }
}
