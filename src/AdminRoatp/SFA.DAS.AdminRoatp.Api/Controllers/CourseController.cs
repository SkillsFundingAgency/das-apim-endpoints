using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Queries.GetAllowedProviders;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[ApiController]
[Route("/courses")]
public class CourseController(IMediator _mediator, ILogger<CourseController> _logger) : ControllerBase
{
    [HttpGet("{larsCode}/providers/allowed")]
    [ProducesResponseType(typeof(GetAllowedProvidersResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllowedProvidersByCourse([FromRoute] string larsCode)
    {
        _logger.LogInformation("Request received to get allowed providers for a course");

        GetAllowedProvidersQuery query = new(larsCode);
        GetAllowedProvidersResponse result = await _mediator.Send(query);
        return Ok(result);
    }
}
