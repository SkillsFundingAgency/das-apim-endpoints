using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFeedback;

public class GetEmployerFeedbackAnnualReviewsRequest : IGetApiRequest
{
    public string GetUrl => $"api/EmployerFeedbackResult/reviews?timeperiod={_timePeriod}";
    private string _timePeriod { get; }
    public GetEmployerFeedbackAnnualReviewsRequest(string timePeriod)
    {
        _timePeriod = timePeriod;
    }
}
