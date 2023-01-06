using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetTotalProvidersForStandardRequest : IGetApiRequest
    {
        private readonly int _larsCode;

        public GetTotalProvidersForStandardRequest(int standardId)
        {
            _larsCode = standardId;
        }

        public string GetUrl => $"/api/courses/{_larsCode}/providers/count";
    }
}