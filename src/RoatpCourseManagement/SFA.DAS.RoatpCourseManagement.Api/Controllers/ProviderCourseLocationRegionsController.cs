using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllStandardRegions;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Course Location")]
[Route("")]
public class ProviderCourseLocationRegionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProviderCourseLocationRegionsController> _logger;

    public ProviderCourseLocationRegionsController(IMediator mediator, ILogger<ProviderCourseLocationRegionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    [HttpGet]
    [Route("providers/{ukprn}/courses/{larsCode}/locations/regions")]
    public async Task<IActionResult> GetAllStandardSubRegions(int ukprn, string larsCode)
    {
        _logger.LogInformation("Outer API: Request to get all standard subregions for ukprn: {Ukprn} larscode: {LarsCode}", ukprn, larsCode);

        var standardRegionsQueryResult = await _mediator.Send(new GetAllStandardRegionsQuery(ukprn, larsCode));

        if (standardRegionsQueryResult == null || standardRegionsQueryResult.Regions.Count == 0)
        {
            _logger.LogError("Standard subregions for ukprn not found for ukprn {Ukprn} and lars code {LarsCode}", ukprn, larsCode);
            return NotFound();
        }
        return Ok(standardRegionsQueryResult);
    }
}
