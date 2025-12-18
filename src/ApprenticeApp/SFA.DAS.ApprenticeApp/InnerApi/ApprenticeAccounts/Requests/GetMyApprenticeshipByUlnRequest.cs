using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class GetMyApprenticeshipByUlnRequest : IGetApiRequest
    {
        private readonly long _uln;

        public GetMyApprenticeshipByUlnRequest(long uln)
        {
            _uln = uln;
        }

        public string GetUrl => $"apprentice/{_uln}";
    }
}
