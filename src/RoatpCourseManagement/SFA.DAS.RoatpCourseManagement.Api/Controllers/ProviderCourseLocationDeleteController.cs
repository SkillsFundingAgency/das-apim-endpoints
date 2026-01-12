using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.BulkDeleteProviderCourse;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourseLocation;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Course Location")]
[Route("")]
public class ProviderCourseLocationDeleteController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProviderCourseLocationDeleteController> _logger;

    public ProviderCourseLocationDeleteController(ILogger<ProviderCourseLocationDeleteController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("providers/{ukprn}/courses/{larsCode}/bulk-delete-course-locations")]
    public async Task<IActionResult> BulkDeleteProviderCourseLocations(int ukprn, string larsCode, BulkDeleteProviderCourseLocationsCommand command)
    {
        _logger.LogInformation("Outer API: Request received to bulk delete provider course locations for ukprn: {Ukprn} larscode: {Larscode} option: {DeleteOption}", ukprn, larsCode, command.DeleteProviderCourseLocationOption);
        command.Ukprn = ukprn;
        command.LarsCode = larsCode;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost]
    [Route("providers/{ukprn}/courses/{larsCode}/location/{id}/delete")]
    public async Task<IActionResult> DeleteProviderCourseLocation(int ukprn, string larsCode, Guid id, DeleteProviderCourseLocationCommand command)
    {
        _logger.LogInformation("Outer API: Request received to delete provider course location for ukprn: {Ukprn} larscode: {Larscode} providerCourseLocationId: {LocationId}", ukprn, larsCode, id);
        command.Ukprn = ukprn;
        command.LarsCode = larsCode;
        await _mediator.Send(command);
        return NoContent();
    }
}
