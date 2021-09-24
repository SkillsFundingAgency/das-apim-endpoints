using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.InnerApi.Requests
{
    public class GetEmployerAccountLegalEntitiesRequest : IGetApiRequest
    {
        public string EncodedAccountId { get; }

        public GetEmployerAccountLegalEntitiesRequest(string encodedAccountId)
        {
            EncodedAccountId = encodedAccountId;
        }

        public string GetUrl => $"api/accounts/{EncodedAccountId}/legalentities";
    }
}