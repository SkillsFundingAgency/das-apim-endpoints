using SFA.DAS.Apprenticeships.Application.AccountUsers;
using SFA.DAS.Apprenticeships.Application.AccountUsers.Queries;

namespace SFA.DAS.Apprenticeships.Api.Models
{
    public class UserAccountsApiResponse
    {
        public List<UserAccountsApiResponseItem> UserAccounts { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string EmployerUserId { get; set; } = null!;
        public bool IsSuspended { get; set; }



        public static implicit operator UserAccountsApiResponse(GetAccountsQueryResult source)
        {
            var accounts = source.UserAccountResponse == null
                ? new List<UserAccountsApiResponseItem>()
                : source.UserAccountResponse.Select(c => (UserAccountsApiResponseItem) c).ToList();
            
            return new UserAccountsApiResponse
            {
                EmployerUserId = source.EmployerUserId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                UserAccounts = accounts,
                IsSuspended = source.IsSuspended
            };
        }
    }

    public class UserAccountsApiResponseItem
    {
        public string EncodedAccountId { get ; set ; } = null!;
        public string DasAccountName { get ; set ; } = null!;
        public string Role { get ; set ; } = null!;

        public static implicit operator UserAccountsApiResponseItem(AccountUser source)
        {
            return new UserAccountsApiResponseItem
            {
                DasAccountName = source.DasAccountName,
                EncodedAccountId = source.EncodedAccountId,
                Role = source.Role
            };
        }
    }
}