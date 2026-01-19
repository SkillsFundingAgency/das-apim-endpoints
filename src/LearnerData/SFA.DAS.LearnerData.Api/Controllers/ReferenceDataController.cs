using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.GetProviderRelationships;

namespace SFA.DAS.LearnerData.Api.Controllers;

[Route("reference-data")]
[ApiController]
public class ReferenceDataController(
IMediator mediator,
ILogger<ReferenceDataController> logger) : ControllerBase
{
    [HttpGet("providers/{ukprn}")]
    public async Task<IActionResult> GetProviderRelationshipDetails([FromRoute] int ukprn)
    {
        logger.LogInformation("GetEmployerAgreementId for ukprn {Ukprn}", ukprn);

        var query = new GetProviderRelationshipQuery()
        {
            Ukprn = ukprn
        };

        var response = await mediator.Send(query);

        if (response is null) return NotFound(); 

        return Ok(response);
    }

    [HttpGet("providers")]
    public async Task<IActionResult> GetAllProviderRelationshipDetails([FromQuery] int page = 1, [FromQuery] int pagesize = 20)
    {
        const int MAX_PAGE_SIZE = 100;
        pagesize = Math.Min(pagesize, MAX_PAGE_SIZE);

        logger.LogInformation("GetEmployerAgreementId");

        var query = new GetAllProviderRelationshipQuery()
        {
            Page = page,
            PageSize = pagesize
        };

        var response = await mediator.Send(query);
       
        return Ok(response);
    }
}