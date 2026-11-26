using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Ai;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RecruitAi;

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