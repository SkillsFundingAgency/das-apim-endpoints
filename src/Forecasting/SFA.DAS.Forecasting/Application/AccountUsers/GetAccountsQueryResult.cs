using System.Collections.Generic;

namespace SFA.DAS.Forecasting.Application.AccountUsers
{
    public class GetAccountsQueryResult
    {
        public IEnumerable<AccountUser> UserAccountResponse { get; set; }
        public string EmployerUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}