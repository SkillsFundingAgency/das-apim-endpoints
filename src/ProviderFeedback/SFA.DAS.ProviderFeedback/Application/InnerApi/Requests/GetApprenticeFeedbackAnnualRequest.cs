using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Requests
{
    public class GetApprenticeFeedbackAnnualRequest : IGetApiRequest
    {
        public string GetUrl => $"api/apprenticefeedbackresult/{_ukprn}/annual";
        private long _ukprn { get; }
        public GetApprenticeFeedbackAnnualRequest(long ukprn)
        {
            _ukprn = ukprn;
        }
    }

}