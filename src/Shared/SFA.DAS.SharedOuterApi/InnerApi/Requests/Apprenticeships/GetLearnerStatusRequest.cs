using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

public class GetLearnerStatusRequest : IGetApiRequest
{
    public GetLearnerStatusRequest(Guid apprenticeshipKey)
    {
        ApprenticeshipKey = apprenticeshipKey;
    }

    public Guid ApprenticeshipKey { get; }
    public string GetUrl => $"{ApprenticeshipKey}/LearnerStatus";
}