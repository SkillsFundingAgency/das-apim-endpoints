using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetAccountProvidersRequest : IGetApiRequest
    {
        private readonly long _accountId;
        public GetAccountProvidersRequest(long accountId)
        {
            _accountId = accountId;
        }
        
        public string GetUrl => $"accounts/{_accountId}/providers";
    }
}