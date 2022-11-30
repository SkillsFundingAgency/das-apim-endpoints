using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.InnerApi.Requests
{
    public class GetEnglishFractionHistoryRequest : IGetApiRequest
    {
        private readonly string _hashedAccountId;
        private readonly string _empRef;

        public GetEnglishFractionHistoryRequest(string hashedAccountId, string empRef)
        {
            _hashedAccountId = hashedAccountId;
            _empRef = empRef;
        }

        public string GetUrl => $"api/accounts/{_hashedAccountId}/levy/english-fraction-history?empRef={_empRef}";
    }
}
