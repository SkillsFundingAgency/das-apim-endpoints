using SFA.DAS.ProviderRelationships.Application.AccountUsers;
using SFA.DAS.ProviderRelationships.Application.AccountUsers.Queries;

namespace SFA.DAS.ProviderRelationships.Api.Models;

public class GetUserAccountsApiResponse
{
    public List<UserAccountsApiResponseItem> UserAccounts { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string EmployerUserId { get; set; }
    
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
            EmployerUserId = source.EmployerUserId,
            FirstName = source.FirstName,
            LastName = source.LastName,
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