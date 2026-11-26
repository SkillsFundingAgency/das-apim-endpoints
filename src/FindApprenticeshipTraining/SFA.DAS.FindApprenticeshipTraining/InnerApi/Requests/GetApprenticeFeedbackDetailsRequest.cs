using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetApprenticeFeedbackDetailsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/apprenticefeedbackresult/{_ukprn}";
        private long _ukprn { get; }
        public GetApprenticeFeedbackDetailsRequest(long ukprn)
        {
            _ukprn = ukprn;
        }
    }

}