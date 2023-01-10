using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Forecasting.Application.AccountUsers;

namespace SFA.DAS.Forecasting.Api.Models
{
    public class GetUserAccountsApiResponse
    {
        public List<UserAccountsApiResponseItem> UserAccounts { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmployerUserId { get; set; }
        public string Email { get; set; }
        public static implicit operator GetUserAccountsApiResponse(GetAccountsQueryResult source)
        {
            if (source?.UserAccountResponse == null)
            {
                return new GetUserAccountsApiResponse
                {
                    Email = source?.Email,
                    FirstName = source?.FirstName,
                    LastName = source?.LastName,
                    EmployerUserId = source?.EmployerUserId,
                    UserAccounts = new List<UserAccountsApiResponseItem>()
                };
            }
            return new GetUserAccountsApiResponse
            {
                Email = source.Email,
                FirstName = source.FirstName,
                LastName = source.LastName,
                EmployerUserId = source.EmployerUserId,
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