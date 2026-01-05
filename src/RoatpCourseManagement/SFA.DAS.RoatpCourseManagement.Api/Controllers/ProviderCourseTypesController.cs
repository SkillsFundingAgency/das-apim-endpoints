using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseTypes.Queries;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Route("providers/{ukprn}/course-types")]
public class ProviderCourseTypesController(IMediator _mediator, ILogger<ProviderCourseTypesController> _logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProviderCourseTypes(int ukprn)
    {
        _logger.LogInformation("Request received to get all provider Course Types for ukprn: {Ukprn}", ukprn);

        var response = await _mediator.Send(new GetProviderCourseTypesQuery(ukprn));

        if (response == null)
        {
            _logger.LogError("Provider not found for ukprn {Ukprn}", ukprn);
            return NotFound();
        }

        return Ok(response);
    }
}
