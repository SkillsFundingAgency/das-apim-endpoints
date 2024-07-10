using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Requests
{
    public class GetApprenticeFeedbackRequest : IGetApiRequest
    {
        public string GetUrl => $"api/apprenticefeedbackresult/{_ukprn}";
        private long _ukprn { get; }
        public GetApprenticeFeedbackRequest(long ukprn)
        {
            _ukprn = ukprn;
        }
    }

}