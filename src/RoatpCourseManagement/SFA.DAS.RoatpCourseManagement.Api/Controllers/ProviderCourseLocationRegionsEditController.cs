using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateStandardSubRegions;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Course Location")]
[Route("")]
public class ProviderCourseLocationRegionsEditController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProviderCourseLocationRegionsEditController> _logger;

    public ProviderCourseLocationRegionsEditController(IMediator mediator, ILogger<ProviderCourseLocationRegionsEditController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    [HttpPost]
    [Route("providers/{ukprn}/courses/{larsCode}/locations/regions")]
    public async Task<IActionResult> UpdateStandardSubRegions(int ukprn, string larsCode, UpdateStandardSubRegionsCommand command)
    {
        _logger.LogInformation("Outer API: Request to update standard subregions for ukprn: {Ukprn} larscode: {Larscode}", ukprn, larsCode);
        command.Ukprn = ukprn;
        command.LarsCode = larsCode;
        var httpStatusCode = await _mediator.Send(command);
        if (httpStatusCode != HttpStatusCode.NoContent)
        {
            _logger.LogError("Outer API: Failed request to update standard subregions for ukprn: {Ukprn} larscode: {Larscode} with HttpStatusCode: {HttpStatusCode}", ukprn, larsCode, httpStatusCode);
            return new StatusCodeResult((int)httpStatusCode);
        }
        return NoContent();
    }
}
