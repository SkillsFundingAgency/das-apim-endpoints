using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerAccounts.InnerApi.Requests
{
    public class GetReservationsRequest : IGetAllApiRequest
    {
        private readonly long _accountId;

        public GetReservationsRequest(long accountId) => _accountId = accountId;
        
        public string GetAllUrl => $"/api/accounts/{_accountId}/reservations";
    }
}