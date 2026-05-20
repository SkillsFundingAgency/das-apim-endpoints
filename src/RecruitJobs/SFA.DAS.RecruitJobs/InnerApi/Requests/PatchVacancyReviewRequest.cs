using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.RecruitJobs.Domain;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests;

public class PatchVacancyReviewRequest(Guid vacancyReviewId, JsonPatchDocument<VacancyReview> data): IPatchApiRequest<JsonPatchDocument<VacancyReview>>
{
    public string PatchUrl => $"/api/vacancyreviews/{vacancyReviewId}";
    public JsonPatchDocument<VacancyReview> Data { get; set; } = data;
}