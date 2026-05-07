using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;

public class PatchableVacancyReviewDto
{
    public ReviewStatus Status { get; init; }   
}

public sealed class PatchVacancyReviewRequest(Guid vacancyReviewId, JsonPatchDocument<PatchableVacancyReviewDto> data): IPatchApiRequest<JsonPatchDocument<PatchableVacancyReviewDto>>
{
    public string PatchUrl { get; } = $"api/vacancyreviews/{vacancyReviewId}";
    public JsonPatchDocument<PatchableVacancyReviewDto> Data { get; set; } = data;
}