using System;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.SharedOuterApi.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;

public class PatchableVacancyReviewDto
{
    public ReviewStatus Status { get; init; } 
    public string ManualOutcome { get; init; }
    public DateTime ClosedDate { get; init; }
}

public sealed class PatchVacancyReviewRequest(Guid vacancyReviewId, JsonPatchDocument<PatchableVacancyReviewDto> data): IPatchApiRequest<JsonPatchDocument<PatchableVacancyReviewDto>>
{
    public string PatchUrl { get; } = $"api/vacancyreviews/{vacancyReviewId}";
    public JsonPatchDocument<PatchableVacancyReviewDto> Data { get; set; } = data;
}