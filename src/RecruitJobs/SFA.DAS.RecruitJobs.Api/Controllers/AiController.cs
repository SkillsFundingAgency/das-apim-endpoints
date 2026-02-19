using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitJobs.Ai;
using SFA.DAS.RecruitJobs.Ai.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RecruitAi;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class AiController: ControllerBase
{
    [HttpPost]
    [Route("vacancy/{vacancyId:guid}/review")]
    public async Task<IResult> PerformVacancyReviewAsync(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromServices] IRecruitAiApiClient<RecruitAiApiConfiguration> recruitAiApiClient,
        [FromServices] IRecruitArtificialIntelligenceService aiService,
        [FromServices] IAiReviewResultChecker aiReviewResultChecker,
        [FromRoute] Guid vacancyId,
        [FromBody] Guid vacancyReviewId,
        CancellationToken cancellationToken)
    {
        // fetch the vacancy
        var response = await recruitApiClient.GetWithResponseCode<GetVacancyResponse>(new GetVacancyRequest(vacancyId));
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return TypedResults.NotFound("Vacancy not found");            
        }
        
        // send to chat gpt
        var result = await aiService.ReviewVacancyAsync(response.Body, cancellationToken);
        var flagForReview = aiReviewResultChecker.FlagForReview(result, out var reviewStatus);
        
        // update the inner record
        var patchDocument = new JsonPatchDocument<PatchableAiVacancyReviewDto>();
        patchDocument.Replace(x => x.Status, reviewStatus);
        patchDocument.Replace(x => x.ManualReviewRequired, flagForReview);
        patchDocument.Replace(x => x.Output, JsonSerializer.Serialize(result));
    
        var patchResponse = await recruitAiApiClient.PatchWithResponseCode(new PatchAiVacancyReviewRequest(vacancyReviewId, patchDocument));
        if (patchResponse.StatusCode == HttpStatusCode.NotFound)
        {
            await recruitAiApiClient.Put(new PutAiVacancyReviewRequest(vacancyReviewId, new PutAiVacancyReviewDto()
            {
                ManualReviewRequired = flagForReview,
                Output = JsonSerializer.Serialize(result),
                Status = reviewStatus,
                VacancyId = vacancyId,
            }));
        }
        
        return patchResponse.StatusCode.IsSuccessStatusCode()
            ? TypedResults.Ok(new { FlaggedForReview = flagForReview })
            : TypedResults.Problem();
    }
}