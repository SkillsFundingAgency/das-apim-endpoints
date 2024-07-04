using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.EmployerAccounts.Application.Queries.AccountUsers
{
    public class AccountUser
    {
        public string DasAccountName { get; set; }
        public string EncodedAccountId { get; set; }
        public string Role { get; set; }
        public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }
    }
}