using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Funding.InnerApi.Requests.Learning
{
    public class GetLearningsRequest : IGetAllApiRequest
    {
        private readonly long _ukprn;

        public GetLearningsRequest(long ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetAllUrl => $"{_ukprn}/learnings";
    }
}