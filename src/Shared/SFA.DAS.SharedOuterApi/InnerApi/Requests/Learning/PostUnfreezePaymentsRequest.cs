using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

public class PostUnfreezePaymentsRequest : IPostApiRequest
{
    public PostUnfreezePaymentsRequest(Guid apprenticeshipKey)
    {
        ApprenticeshipKey = apprenticeshipKey;
    }

    public Guid ApprenticeshipKey { get; set; }
    public string PostUrl => $"{ApprenticeshipKey}/unfreeze";
    public object Data { get; set; }
}