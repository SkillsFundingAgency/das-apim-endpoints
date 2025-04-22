using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ApprenticeFeedback;
public class GetApprenticeFeedbackDetailsAnnualRequest : IGetApiRequest
{
    public string GetUrl => $"api/ApprenticeFeedbackResult/{_ukprn}/annual";
    private long _ukprn { get; }
    public GetApprenticeFeedbackDetailsAnnualRequest(long ukprn)
    {
        _ukprn = ukprn;
    }
}
