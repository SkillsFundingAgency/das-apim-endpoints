using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.InnerApi.Requests
{
    public class GetReservationsRequest : IGetAllApiRequest
    {
        private readonly string _accountId;

        public GetReservationsRequest(string accountId)
        {
            _accountId = accountId;
        }
        public string GetAllUrl => $"/api/accounts/{_accountId}/reservations";
    }
}