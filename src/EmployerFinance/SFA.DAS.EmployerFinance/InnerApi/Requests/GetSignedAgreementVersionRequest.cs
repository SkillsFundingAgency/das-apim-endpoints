using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests
{
    public class GetSignedAgreementVersionRequest : IGetApiRequest
    {
        private readonly string _hashedAccountId;

        public GetSignedAgreementVersionRequest(string hashedAccountId)
        {
            _hashedAccountId = hashedAccountId;
        }

        public string GetUrl => $"api/accounts/{_hashedAccountId}/signed-agreement-version";
    }
}