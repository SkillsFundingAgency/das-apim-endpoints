using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseLevels;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseRoutes;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourses;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public sealed class CoursesController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Route("levels")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetCourseLevelsListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseLevels()
    {
        var response = await _mediator.Send(new GetCourseLevelsQuery());
        return Ok(response);
    }

    [HttpGet]
    [Route("routes")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetRoutesListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseRoutes()
    {
        var result = await _mediator.Send(new GetCourseRoutesQuery());
        return Ok(result);
    }

    [HttpGet]
    [Route("{larscode}/providers")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetCourseProvidersResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseProviders(string larscode, [FromQuery] GetCourseProvidersModel model)
    {
        var result = await _mediator.Send(new GetCourseProvidersQuery
        {
            LarsCode = larscode,
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

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetCoursesQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourses([FromQuery] GetCoursesQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{larscode}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetCourseByLarsCodeQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseByLarsCode([FromRoute] string larscode, [FromQuery] int? distance, [FromQuery] string location)
    {
        var result = await _mediator.Send(new GetCourseByLarsCodeQuery
        {
            LarsCode = larscode,
            Location = location,
            Distance = distance
        });

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [Route("{larsCode}/providers/{ukprn:int}")]
    public async Task<IActionResult> GetCourseProvider([FromRoute] string larsCode, [FromRoute] long ukprn, [FromQuery] GetCourseProviderRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCourseProviderQuery(
                ukprn,
                larsCode,
                request.ShortlistUserId,
                request.Location,
                request.Distance
            ),
            cancellationToken
        );

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
