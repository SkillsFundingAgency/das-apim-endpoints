using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerAccounts.Application.Queries.AccountUsers;
using SFA.DAS.EmployerAccounts.Application.Queries.AccountUsers.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.EmployerAccounts.Api.Models
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
            var userAccounts = source?.UserAccountResponse == null 
                ? new List<UserAccountsApiResponseItem>() 
                : source.UserAccountResponse.Select(c=>(UserAccountsApiResponseItem)c).ToList();
            
            return new UserAccountsApiResponse
            {
                EmployerUserId = source?.EmployerUserId,
                FirstName = source?.FirstName,
                LastName = source?.LastName,
                IsSuspended = source?.IsSuspended ?? false,
                UserAccounts = userAccounts
            };
        }
    }

    public class UserAccountsApiResponseItem
    {
        public string EncodedAccountId { get ; set ; }
        public string DasAccountName { get ; set ; }
        public string Role { get ; set ; }
        public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }

        public static implicit operator UserAccountsApiResponseItem(AccountUser source)
        {
            return new UserAccountsApiResponseItem
            {
                DasAccountName = source.DasAccountName,
                EncodedAccountId = source.EncodedAccountId,
                Role = source.Role,
                ApprenticeshipEmployerType = source.ApprenticeshipEmployerType
            };
        }
    }
}