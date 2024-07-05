using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Reservations.Application.Accounts;
using SFA.DAS.Reservations.Application.Accounts.Queries;

namespace SFA.DAS.Reservations.Api.Models;

public record GetAccountUsersApiResponse
{
    public List<AccountUsersApiResponseItem> AccountUsers { get; set; }

    public static implicit operator GetAccountUsersApiResponse(GetAccountUsersQueryResult source)
    {
        var accountUsers = source.AccountUsersResponse == null
            ? []
            : source.AccountUsersResponse.Select(x => (AccountUsersApiResponseItem)x).ToList();

        return new GetAccountUsersApiResponse
        {
            AccountUsers = accountUsers
        };
    }
    
    public record AccountUsersApiResponseItem
    {
        public string UserRef { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool CanReceiveNotifications { get; set; }

        public static implicit operator AccountUsersApiResponseItem(User source)
        {
            return new AccountUsersApiResponseItem
            {
                Email = source.Email,
                CanReceiveNotifications = source.CanReceiveNotifications,
                Role = source.Role,
                UserRef = source.UserRef,
            };
        }
    }
}