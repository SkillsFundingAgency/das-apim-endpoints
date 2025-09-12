using System;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public record PatchRecruitApplicationReviewApiRequest(
    Guid ApplicationId,
    JsonPatchDocument<ApplicationReview> Data)
    : IPatchApiRequest<JsonPatchDocument<ApplicationReview>>
{
    public JsonPatchDocument<ApplicationReview> Data { get; set; } = Data;

    public string PatchUrl => $"api/applicationReviews/{ApplicationId}";
}
public abstract record ApplicationReview
{
    public bool HasEverBeenEmployerInterviewing { get; set; }
    public DateTime? DateSharedWithEmployer { get; set; }
    public DateTime? StatusUpdatedDate { get; set; }
    public string? EmployerFeedback { get; set; }
    public string? Status { get; set; }
    public string? TemporaryReviewStatus { get; set; }
}