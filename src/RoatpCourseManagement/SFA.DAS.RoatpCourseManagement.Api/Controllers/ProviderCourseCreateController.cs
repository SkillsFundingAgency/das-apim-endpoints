using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.CreateProviderCourse;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Courses")]
[Route("")]
public class ProviderCourseCreateController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProviderCourseCreateController> _logger;

    public ProviderCourseCreateController(ILogger<ProviderCourseCreateController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("providers/{ukprn}/courses/{larsCode}/create")]
    public async Task<IActionResult> CreateProviderCourse([FromRoute] int ukprn, [FromRoute] string larsCode, [FromBody] CreateProviderCourseCommand command)
    {
        _logger.LogInformation("Outer API: Request received to create provider course: {Larscode} for ukprn: {Ukprn} by user: {UserId}", larsCode, ukprn, command.UserId);

        command.Ukprn = ukprn;
        command.LarsCode = larsCode;

        await _mediator.Send(command);

        return new StatusCodeResult(StatusCodes.Status201Created);
    }
}
