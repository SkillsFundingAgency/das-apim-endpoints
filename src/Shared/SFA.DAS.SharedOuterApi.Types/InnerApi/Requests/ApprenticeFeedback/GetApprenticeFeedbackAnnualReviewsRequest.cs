using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ApprenticeFeedback;

public class GetApprenticeFeedbackAnnualReviewsRequest(string timePeriod) : IGetApiRequest
{
    public string GetUrl => $"api/ApprenticeFeedbackResult/reviews?timeperiod={_timePeriod}";
    private string _timePeriod { get; } = timePeriod;
}
