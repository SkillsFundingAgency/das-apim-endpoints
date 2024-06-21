using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

public class PatchApproveApprenticeshipStartDateChangeRequest : IPatchApiRequest<ApproveApprenticeshipStartDateChangeRequest>
{
    public PatchApproveApprenticeshipStartDateChangeRequest(
        Guid apprenticeshipKey,
        string userId)
    {
        ApprenticeshipKey = apprenticeshipKey;
        Data = new ApproveApprenticeshipStartDateChangeRequest
        {
            UserId = userId
        };
    }

    public Guid ApprenticeshipKey { get; set; }
    public string PatchUrl => $"{ApprenticeshipKey}/startDateChange/pending";
    public ApproveApprenticeshipStartDateChangeRequest Data { get; set; }
}

public class ApproveApprenticeshipStartDateChangeRequest
{
    public string UserId { get; set; }
}