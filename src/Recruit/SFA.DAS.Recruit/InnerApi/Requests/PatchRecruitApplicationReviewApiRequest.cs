using System;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

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
    public string CandidateFeedback { get; set; }
    public string Status { get; set; }
    public DateTime StatusUpdatedDate { get; set; }
}