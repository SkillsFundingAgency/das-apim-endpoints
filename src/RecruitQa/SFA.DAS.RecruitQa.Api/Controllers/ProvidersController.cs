using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Models.Responses;
using SFA.DAS.RecruitQa.Application.Provider.GetProvider;

namespace SFA.DAS.RecruitQa.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProvidersController(IMediator mediator, ILogger<ProvidersController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{ukprn:int}")]
    public async Task<IActionResult> GetProvider([FromRoute] int ukprn)
    {
        try
        {
            var response = await mediator.Send(new GetProviderQuery(ukprn));
            if (response?.Provider == null)
                return NotFound();

            return Ok((GetProviderResponse)response.Provider);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting provider information");
            return BadRequest();
        }
    }
}