using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ReferenceDataController: ControllerBase
{
    [HttpGet]
    [Route("candidate-skills")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<IResult> GetCandidateSkills([FromServices] IRecruitApiClient<RecruitApiConfiguration> apiClient)
    {
        var results = await apiClient.Get<List<string>>(new GetCandidateSkillsRequest());
        return TypedResults.Ok(results);
    }

    [HttpGet]
    [Route("candidate-qualifications")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<IResult> GetCandidateQualifications([FromServices] IRecruitApiClient<RecruitApiConfiguration> apiClient)
    {
        var results = await apiClient.Get<List<string>>(new GetCandidateQualificationsRequest());
        return TypedResults.Ok(results);
    }
}