using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.RecruitJobs.Ai;
using SFA.DAS.RecruitJobs.Api.Models.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Ai;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RecruitAi;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Recruit;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[ApiController]
[Route("[controller]/vacancies/{vacancyId:guid}")]
public class AiController(ILogger<AiController> logger): ControllerBase
{
    [HttpPost]
    [Route("review/{vacancyReviewId:guid}")]
    public async Task<IResult> CreateVacancyReviewAsync(
        [FromServices] IRecruitAiApiClient<RecruitAiApiConfiguration> recruitAiApiClient,
        [FromRoute] Guid vacancyId,
        [FromRoute] Guid vacancyReviewId,
        [FromBody] CreateVacancyReviewData data,
        CancellationToken cancellationToken)
    {
        var reviewRequired = data.ReviewStatus switch
        {
            AiReviewStatus.Skipped => true,
            AiReviewStatus.Failed => true,
            _ => false
        };
        
        var response = await recruitAiApiClient.PutWithResponseCode<PutAiVacancyReviewDto, NullResponse>(new PutAiVacancyReviewRequest(vacancyReviewId, new PutAiVacancyReviewDto()
        {
            ManualReviewRequired = reviewRequired,
            Output = null,
            Status = data.ReviewStatus,
            VacancyId = vacancyId,
        }));
        return response.StatusCode.IsSuccessStatusCode() ? TypedResults.Ok() : TypedResults.Problem();
    }
    
    [HttpPost]
    [Route("review")]
    public async Task<IResult> PerformVacancyReviewAsync(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromServices] IRecruitAiService aiService,
        [FromRoute] Guid vacancyId,
        [FromBody] Guid vacancyReviewId,
        CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<GetVacancyResponse>(new GetVacancyRequest(vacancyId));
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return TypedResults.NotFound("Vacancy not found");
        }
        
        var result = await aiService.ReviewVacancyAsync(vacancyReviewId, response.Body, cancellationToken);
        return result ? Results.Ok() : Results.Problem();
    }

    [HttpPost]
    [Route("refer-to-manual")]
    public async Task<IResult> SendVacancyForManualReviewAsync(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] Guid vacancyId,
        [FromBody] Guid vacancyReviewId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Request to send vacancy for manual review. Id={VacancyId}, ReviewId={VacancyReviewId}", vacancyId, vacancyReviewId);
        
        var patchDocument = new JsonPatchDocument<PatchableVacancyReviewDto>();
        patchDocument.Replace(x => x.Status, ReviewStatus.PendingReview);
        var request = new PatchVacancyReviewRequest(vacancyReviewId, patchDocument);
        var response = await recruitApiClient.PatchWithResponseCode<JsonPatchDocument<PatchableVacancyReviewDto>, NullResponse>(request, false);
        return response.StatusCode.IsSuccessStatusCode()
            ? TypedResults.Ok()
            : TypedResults.Problem();
    }
    
    [HttpPost]
    [Route("approve")]
    public async Task<IResult> AutoApproveVacancyAsync(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] Guid vacancyId,
        [FromBody] Guid vacancyReviewId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Request to auto approve vacancy. Id={VacancyId}, ReviewId={VacancyReviewId}", vacancyId, vacancyReviewId);
        
        // TODO: change this to approve when we are ready
        var patchDocument = new JsonPatchDocument<PatchableVacancyReviewDto>();
        patchDocument.Replace(x => x.Status, ReviewStatus.PendingReview);
        var request = new PatchVacancyReviewRequest(vacancyReviewId, patchDocument);
        var response = await recruitApiClient.PatchWithResponseCode<JsonPatchDocument<PatchableVacancyReviewDto>, NullResponse>(request, false);
        return response.StatusCode.IsSuccessStatusCode()
            ? TypedResults.Ok()
            : TypedResults.Problem();
    }
}