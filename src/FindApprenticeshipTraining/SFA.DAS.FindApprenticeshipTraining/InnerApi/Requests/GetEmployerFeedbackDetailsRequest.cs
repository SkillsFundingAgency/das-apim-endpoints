using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetEmployerFeedbackDetailsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/employerfeedbackresult/{_ukprn}";
        private long _ukprn { get; }
        public GetEmployerFeedbackDetailsRequest(long ukprn)
        {
            _ukprn = ukprn;
        }
    }
}