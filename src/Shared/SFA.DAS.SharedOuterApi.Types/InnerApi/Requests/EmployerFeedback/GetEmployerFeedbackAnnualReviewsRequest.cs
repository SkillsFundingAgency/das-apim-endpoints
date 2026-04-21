using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerFeedback;

public class GetEmployerFeedbackAnnualReviewsRequest(string timePeriod) : IGetApiRequest
{
    public string GetUrl => $"api/EmployerFeedbackResult/reviews?timeperiod={_timePeriod}";
    private string _timePeriod { get; } = timePeriod;
}
