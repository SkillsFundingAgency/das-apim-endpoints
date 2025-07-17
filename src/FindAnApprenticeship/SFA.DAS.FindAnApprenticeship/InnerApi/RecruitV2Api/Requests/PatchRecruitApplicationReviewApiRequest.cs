using System;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;

public record PatchRecruitApplicationReviewApiRequest(
    Guid ApplicationId,
    JsonPatchDocument<ApplicationReview> Data)
    : IPatchApiRequest<JsonPatchDocument<ApplicationReview>>
{
    public JsonPatchDocument<ApplicationReview> Data { get; set; } = Data;

    public string PatchUrl => $"api/applicationReviews/{ApplicationId}";
}
public record ApplicationReview
{
    public DateTime? WithdrawnDate { get; set; }
    public string Status { get; set; }
    public DateTime? StatusUpdatedDate { get; set; }
    public Guid Id { get; set; }
}