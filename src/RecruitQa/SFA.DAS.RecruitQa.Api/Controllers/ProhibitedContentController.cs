using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Application.BannedPhrases.GetBannedPhrases;
using SFA.DAS.RecruitQa.Application.Profanity.GetProfanity;

namespace SFA.DAS.RecruitQa.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProhibitedContentController(IMediator mediator, ILogger<ProhibitedContentController> logger) : ControllerBase
{
    [HttpGet]
    [Route("profanity")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetAllProfanityList(
        CancellationToken cancellationToken)
    {
        var results = await mediator.Send(new GetProfanityListQuery(), cancellationToken);
        return TypedResults.Ok(results.ProfanityList);
    }

    [HttpGet]
    [Route("bannedPhrases")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetAllBannedPhrasesList(
        CancellationToken cancellationToken)
    {
        var results = await mediator.Send(new GetBannedPhrasesQuery(), cancellationToken);
        return TypedResults.Ok(results.BannedPhraseList);
    }
}