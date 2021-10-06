using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.InnerApi.Requests
{
    public class GetAllEmployerAccountLegalEntitiesRequest : IGetAllApiRequest
    {
        public string EncodedAccountId { get; }

        public GetAllEmployerAccountLegalEntitiesRequest(string encodedAccountId)
        {
            EncodedAccountId = encodedAccountId;
        }

        public string GetAllUrl => $"api/accounts/{EncodedAccountId}/legalentities";
    }
}