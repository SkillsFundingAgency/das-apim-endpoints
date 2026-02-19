using System;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.SharedOuterApi.Domain.Recruit.Ai;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RecruitAi;

public struct PatchAiVacancyReviewRequest(Guid vacancyReviewId, JsonPatchDocument<PatchableAiVacancyReviewDto> patchDocument) : IPatchApiRequest<JsonPatchDocument<PatchableAiVacancyReviewDto>>
{
    public string PatchUrl => $"api/ai-vacancy-reviews/{vacancyReviewId}";
    public JsonPatchDocument<PatchableAiVacancyReviewDto> Data { get; set; } = patchDocument;
}

public class PatchableAiVacancyReviewDto
{
    public bool ManualReviewRequired { get; set; }
    public string? Output { get; set; }
    public AiReviewStatus Status { get; set; }
}