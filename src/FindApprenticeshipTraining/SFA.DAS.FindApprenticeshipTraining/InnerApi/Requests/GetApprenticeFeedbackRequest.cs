using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetApprenticeFeedbackRequest : IGetApiRequest
    {
        private readonly long _ukprn;
        public GetApprenticeFeedbackRequest (long ukprn)
        {
            _ukprn = ukprn;
        }
        
        public string GetUrl => $"api/apprenticefeedbackresult/{_ukprn}";
    }
}