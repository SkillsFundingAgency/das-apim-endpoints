using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ApprenticeFeedback;
public class GetApprenticeFeedbackDetailsAnnualRequest : IGetApiRequest
{
    public string GetUrl => $"api/ApprenticeFeedbackResult/{_ukprn}/annual";
    private long _ukprn { get; }
    public GetApprenticeFeedbackDetailsAnnualRequest(long ukprn)
    {
        _ukprn = ukprn;
    }
}
