using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Reservations.Application.AccountUsers;
using SFA.DAS.Reservations.Application.AccountUsers.Queries;

namespace SFA.DAS.Reservations.Api.Models
{
    public class GetUserAccountsApiResponse
    {
        public List<UserAccountsApiResponseItem> UserAccounts { get; set; }
        public bool IsSuspended { get; set; }
        public string UserId { get; set; }
        public static implicit operator GetUserAccountsApiResponse(GetAccountsQueryResult source)
        {
            var accounts = source?.UserAccountResponse == null
                ? new List<UserAccountsApiResponseItem>()
                : source.UserAccountResponse.Select(c => (UserAccountsApiResponseItem) c).ToList();
            
            return new GetUserAccountsApiResponse
            {
                IsSuspended = source?.IsSuspended ?? false,
                UserId = source?.UserId,
                UserAccounts = accounts
            };
        }

        
    }

    public class UserAccountsApiResponseItem
    {
        public string EncodedAccountId { get ; set ; }
        public string DasAccountName { get ; set ; }
        public string Role { get ; set ; }

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