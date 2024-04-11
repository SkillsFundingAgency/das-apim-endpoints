using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Requests
{
    public class GetEmployerFeedbackRequest : IGetApiRequest
    {
        public string GetUrl => $"api/employerfeedbackresult/{_ukprn}";
        private long _ukprn { get; }
        public GetEmployerFeedbackRequest(long ukprn)
        {
            _ukprn = ukprn;
        }
    }

}