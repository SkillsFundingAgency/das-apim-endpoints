using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class GetMyApprenticeshipByUlnRequest : IGetApiRequest
    {
        private readonly int _uln;

        public GetMyApprenticeshipByUlnRequest(int uln)
        {
            _uln = uln;
        }

        public string GetUrl => $"apprentice/{_uln}";
    }
}
