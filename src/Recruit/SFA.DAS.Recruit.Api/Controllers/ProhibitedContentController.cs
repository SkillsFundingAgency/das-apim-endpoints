using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ProhibitedContentController(ILogger<ProhibitedContentController> logger): ControllerBase
{
    [HttpGet]
    [Route("profanities")]
    public async Task<IActionResult> GetProfanities([FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitInnerClient)
    {
        var response = await recruitInnerClient.GetWithResponseCode<List<string>>(new GetProfanitiesRequest());
        if (response.StatusCode.IsSuccessStatusCode())
        {
            return Ok(response.Body);
        }

        logger.LogError("Failed to retrieve profanities list, response had status '{Status}' with content '{ErrorContent}'", response.StatusCode, response.ErrorContent);
        return Ok(new List<string>());
    }
    
    [HttpGet]
    [Route("bannedPhrases")]
    public async Task<IActionResult> GetBannedPhrases([FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitInnerClient)
    {
        var response = await recruitInnerClient.GetWithResponseCode<List<string>>(new GetBannedPhrasesRequest());
        if (response.StatusCode.IsSuccessStatusCode())
        {
            return Ok(response.Body);
        }

        logger.LogError("Failed to retrieve banned phrases, response had status '{Status}' with content '{ErrorContent}'", response.StatusCode, response.ErrorContent);
        return Ok(new List<string>());
    }
}