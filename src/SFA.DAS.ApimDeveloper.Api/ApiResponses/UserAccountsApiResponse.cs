using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Queries;
using SFA.DAS.ApimDeveloper.Models;

namespace SFA.DAS.ApimDeveloper.Api.ApiResponses
{
    public class UserAccountsApiResponse
    {
        public List<UserAccountsApiResponseItem> UserAccounts { get; set; }
        public static implicit operator UserAccountsApiResponse(GetAccountsQueryResult source)
        {
            if (source?.UserAccountResponse == null)
            {
                return new UserAccountsApiResponse
                {
                    UserAccounts = new List<UserAccountsApiResponseItem>()
                };
            }
            return new UserAccountsApiResponse
            {
                UserAccounts = source.UserAccountResponse.Select(c=>(UserAccountsApiResponseItem)c).ToList()
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