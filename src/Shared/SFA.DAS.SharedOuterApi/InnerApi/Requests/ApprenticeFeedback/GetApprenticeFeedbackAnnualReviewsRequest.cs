using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ApprenticeFeedback;

public class GetApprenticeFeedbackAnnualReviewsRequest : IGetApiRequest
{
    public string GetUrl => $"api/ApprenticeFeedbackResult/reviews?timeperiod={_timePeriod}";
    private string _timePeriod { get; }
    public GetApprenticeFeedbackAnnualReviewsRequest(string timePeriod)
    {
        _timePeriod = timePeriod;
    }
}
