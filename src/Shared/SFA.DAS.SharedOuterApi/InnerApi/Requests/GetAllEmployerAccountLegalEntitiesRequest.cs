using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetAllEmployerAccountLegalEntitiesRequest : IGetApiRequest
    {
        public string EncodedAccountId { get; }

        public GetAllEmployerAccountLegalEntitiesRequest(string encodedAccountId)
        {
            EncodedAccountId = encodedAccountId;
        }

        public string GetUrl => $"api/accounts/{EncodedAccountId}";
    }
}