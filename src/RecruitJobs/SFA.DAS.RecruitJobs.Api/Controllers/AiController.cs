using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitJobs.Ai;
using SFA.DAS.RecruitJobs.Api.Models.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Domain.Recruit.Ai;
using SFA.DAS.SharedOuterApi.Domain.Recruit.VacancyReviews;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RecruitAi;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[ApiController]
[Route("[controller]/vacancies/{vacancyId:guid}")]
public class AiController: ControllerBase
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
        var patchDocument = new JsonPatchDocument<PatchableVacancyReviewDto>();
        patchDocument.Replace(x => x.Status, ReviewStatus.PendingReview);
        
        var request = new PatchVacancyReviewRequest(vacancyReviewId, patchDocument);
        var response = await recruitApiClient.PatchWithResponseCode<JsonPatchDocument<PatchableVacancyReviewDto>, NullResponse>(request, false);
        response.EnsureSuccessStatusCode();

        return TypedResults.NoContent();
    }
    
    [HttpPost]
    [Route("approve")]
    public async Task<IResult> AutoApproveVacancyAsync(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] Guid vacancyId,
        [FromBody] Guid vacancyReviewId,
        CancellationToken cancellationToken)
    {
        // Patch the VacancyReview
        var vacancyReviewPatchDocument = new JsonPatchDocument<PatchableVacancyReviewDto>();
        vacancyReviewPatchDocument.Replace(x => x.ManualOutcome, ManualQaOutcome.Approved);
        vacancyReviewPatchDocument.Replace(x => x.Status, ReviewStatus.Closed);
        vacancyReviewPatchDocument.Replace(x => x.ClosedDate, DateTime.UtcNow);
        
        var request = new PatchVacancyReviewRequest(vacancyReviewId, vacancyReviewPatchDocument);
        var patchVacancyReviewResponse = await recruitApiClient.PatchWithResponseCode<JsonPatchDocument<PatchableVacancyReviewDto>, NullResponse>(request, false);
        patchVacancyReviewResponse.EnsureSuccessStatusCode();
        
        return TypedResults.NoContent();
    }
}