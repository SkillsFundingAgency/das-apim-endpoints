using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

public class FreezePaymentsRequest : IPostApiRequest
{
    public Guid ApprenticeshipKey { get; set; }
    public string PostUrl => $"{ApprenticeshipKey}/freeze";
    public object Data { get; set; }

    public FreezePaymentsRequest(Guid apprenticeshipKey)
    {
        ApprenticeshipKey = apprenticeshipKey;
        Data = string.Empty;//no data required, apprenticeship key is in the url
    }
}
