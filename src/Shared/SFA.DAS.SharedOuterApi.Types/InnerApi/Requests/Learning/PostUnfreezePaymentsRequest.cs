using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning;

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