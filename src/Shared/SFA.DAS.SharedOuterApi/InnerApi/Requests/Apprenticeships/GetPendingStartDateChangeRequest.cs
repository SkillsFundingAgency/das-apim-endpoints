using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

public class GetPendingStartDateChangeRequest : IGetApiRequest
{
    public GetPendingStartDateChangeRequest(Guid apprenticeshipKey)
    {
        ApprenticeshipKey = apprenticeshipKey;
    }

    public Guid ApprenticeshipKey { get; }
    public string GetUrl => $"{ApprenticeshipKey}/startDateChange/pending";
}