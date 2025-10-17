using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Queries.GetAllShortCourseTypes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using System.Net;

namespace SFA.DAS.AdminRoatp.Api.Controllers;
[Route("shortcourses")]
[ApiController]
public class ShortCourseTypesController(IMediator _mediator, ILogger<ShortCourseTypesController> _logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetAllCourseTypesResponse))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
    public async Task<IActionResult> GetAllShortCourseTypes(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to get all short course types.");
        GetAllCourseTypesResponse result = await _mediator.Send(new GetAllShortCourseTypesQuery(), cancellationToken);
        return Ok(result);
    }
}