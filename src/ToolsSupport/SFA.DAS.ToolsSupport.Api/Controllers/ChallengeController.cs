using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Models.Challenge;
using SFA.DAS.ToolsSupport.Application.Commands.ChallengeEntry;
using SFA.DAS.ToolsSupport.Application.Queries.GetChallenge;

namespace SFA.DAS.ToolsSupport.Api.Controllers;

[Route("[controller]/")]
[ApiController]
public class ChallengeController(IMediator mediator, ILogger<ChallengeController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{accountId}")]
    public async Task<IActionResult> GetChallenge(long accountId)
    {
        try
        {
            var result = await mediator.Send(new GetChallengeQuery
            {
                AccountId = accountId
            });

            return Ok((GetChallengeResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError("Error: GetChallenge: {error}", e);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("{accountId}")]
    public async Task<IActionResult> ChallengeEntry(long accountId, [FromBody] ChallengeEntryRequest request)
    {
        try
        {
            logger.LogInformation("Sending ChallengeEntry for AccountId {Id}", accountId);

            var command = new ChallengeEntryCommand
            {
                    AccountId = accountId,
                    Id = request.Id,
                    Challenge1 = request.Challenge1,
                    Challenge2 = request.Challenge2,
                    Balance = request.Balance,
                    FirstCharacterPosition = request.FirstCharacterPosition,
                    SecondCharacterPosition = request.SecondCharacterPosition
            };

            var response = await mediator.Send(command);

            return Ok((ChallengeEntryResponse)response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in ChallengeEntry for AccountId Id: {Id}", accountId);
            return BadRequest();
        }
    }
}
