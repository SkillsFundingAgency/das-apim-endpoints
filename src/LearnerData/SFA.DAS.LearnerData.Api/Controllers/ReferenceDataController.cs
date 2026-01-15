using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.LearnerData.Responses;

namespace SFA.DAS.LearnerData.Api.Controllers;

[Route("reference-data")]
[ApiController]
public class ReferenceDataController(
IMediator mediator,
ILogger<LearnersController> logger) : ControllerBase
{

    [HttpGet("providers/{ukprn}")]
    public async Task<IActionResult> GetEmployerAgreementDetails([FromRoute] int ukprn)
    {
        logger.LogInformation("GetEmployerAgreementId for ukprn {Ukprn}", ukprn);            

        var query = new GetProviderRelationshipQuery()
        {
            Ukprn = ukprn
        };

        var response = await mediator.Send(query);

        if(response is null) { return NotFound(); }

        return Ok(response);
    }

    [HttpGet("reference-data/providers")]
    public async Task<IActionResult> GetAllEmployerAgreementDetails([FromQuery] int page = 1, [FromQuery] int pagesize = 20)
    {
        logger.LogInformation("GetEmployerAgreementId");

        var query = new GetAllProviderRelationshipQuery()
        {                
            Page = page,
            PageSize = pagesize
        };

        var response = await mediator.Send(query);
        return Ok((GetAllProviderRelationshipQueryResponse)response);
    }

}
