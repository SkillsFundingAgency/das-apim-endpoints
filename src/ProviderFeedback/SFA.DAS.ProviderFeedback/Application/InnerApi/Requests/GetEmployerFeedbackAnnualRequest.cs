using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Requests
{
    public class GetEmployerFeedbackAnnualRequest : IGetApiRequest
    {
        public string GetUrl => $"api/employerfeedbackresult/{_ukprn}/annual";
        private long _ukprn { get; }
        public GetEmployerFeedbackAnnualRequest(long ukprn)
        {
            _ukprn = ukprn;
        }
    }

}