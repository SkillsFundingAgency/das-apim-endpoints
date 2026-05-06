using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourse;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Courses")]
[Route("")]
public class ProviderCourseDeleteController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProviderCourseDeleteController> _logger;

    public ProviderCourseDeleteController(ILogger<ProviderCourseDeleteController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("providers/{ukprn}/courses/{larsCode}/delete")]
    public async Task<IActionResult> DeleteProviderCourse(int ukprn, string larsCode, DeleteProviderCourseCommand command)
    {
        _logger.LogInformation("Outer API: Request received to delete provider course for ukprn: {Ukprn} larscode: {Larscode} by UserId: {UserId}", ukprn, larsCode, command.UserId);
        command.Ukprn = ukprn;
        command.LarsCode = larsCode;
        await _mediator.Send(command);
        return NoContent();
    }
}
