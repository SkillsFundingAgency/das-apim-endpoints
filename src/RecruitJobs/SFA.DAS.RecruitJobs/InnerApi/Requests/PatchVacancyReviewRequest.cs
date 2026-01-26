using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.RecruitJobs.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests;

public class PatchVacancyReviewRequest(Guid vacancyReviewId, JsonPatchDocument<VacancyReview> data): IPatchApiRequest<JsonPatchDocument<VacancyReview>>
{
    public string PatchUrl => $"/api/vacancyreviews/{vacancyReviewId}";
    public JsonPatchDocument<VacancyReview> Data { get; set; } = data;
}