using System;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public record PatchRecruitApplicationReviewStatusApiRequest(
    Guid ApplicationId,
    JsonPatchDocument<ApplicationReviewStatusData> Data)
    : IPatchApiRequest<JsonPatchDocument<ApplicationReviewStatusData>>
{
    public JsonPatchDocument<ApplicationReviewStatusData> Data { get; set; } = Data;

    public string PatchUrl => $"api/applicationReviews/{ApplicationId}";
}
public abstract record ApplicationReviewStatusData
{
    public string CandidateFeedback { get; set; }
    public string Status { get; set; }
    public DateTime StatusUpdatedDate { get; set; }
}