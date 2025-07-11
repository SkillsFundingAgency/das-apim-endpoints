using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.SharedOuterApi.Employer.GovUK.Auth.Application.Queries.EmployerAccounts
{
    public class GetAccountsQueryResult
    {
        public string EmployerUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<AccountUser> UserAccountResponse { get; set; }
        public bool IsSuspended { get; set; }
    }
    public class AccountUser
    {
        public string DasAccountName { get; set; }
        public string EncodedAccountId { get; set; }
        public string Role { get; set; }
        public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }
    }
}