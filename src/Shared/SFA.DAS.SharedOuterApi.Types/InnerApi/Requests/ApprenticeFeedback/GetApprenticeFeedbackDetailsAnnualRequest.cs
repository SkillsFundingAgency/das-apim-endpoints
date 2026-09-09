using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ApprenticeFeedback;

public class GetApprenticeFeedbackDetailsAnnualRequest(long ukprn) : IGetApiRequest
{
    public string GetUrl => $"api/ApprenticeFeedbackResult/{_ukprn}/annual";
    private long _ukprn { get; } = ukprn;
}
