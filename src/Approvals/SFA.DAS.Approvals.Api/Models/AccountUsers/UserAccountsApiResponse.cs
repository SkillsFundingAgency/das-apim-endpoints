using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Approvals.Application.AccountUsers;
using SFA.DAS.Approvals.Application.AccountUsers.Queries;

namespace SFA.DAS.Approvals.Api.Models.AccountUsers
{
    public class UserAccountsApiResponse
    {
        public List<UserAccountsApiResponseItem> UserAccounts { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmployerUserId { get; set; }
        public bool IsSuspended { get; set; }

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
                EmployerUserId = source.EmployerUserId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                IsSuspended = source.IsSuspended,
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