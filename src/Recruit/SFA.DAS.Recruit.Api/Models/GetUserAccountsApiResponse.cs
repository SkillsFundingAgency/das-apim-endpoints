using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Recruit.Application.AccountUsers;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetUserAccountsApiResponse
    {
        public List<UserAccountsApiResponseItem> UserAccounts { get; set; }
        public static implicit operator GetUserAccountsApiResponse(GetAccountsQueryResult source)
        {
            if (source?.UserAccountResponse == null)
            {
                return new GetUserAccountsApiResponse
                {
                    UserAccounts = new List<UserAccountsApiResponseItem>()
                };
            }
            return new GetUserAccountsApiResponse
            {
                UserAccounts = source.UserAccountResponse.Select(c => (UserAccountsApiResponseItem)c).ToList()
            };
        }
    }

    public class UserAccountsApiResponseItem
    {
        public string EncodedAccountId { get; set; }
        public string DasAccountName { get; set; }
        public string Role { get; set; }

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
