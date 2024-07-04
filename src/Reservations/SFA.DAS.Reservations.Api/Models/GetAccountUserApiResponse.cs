using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Reservations.Application.Accounts;
using SFA.DAS.Reservations.Application.Accounts.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Reservations.Api.Models;

public record GetAccountUserApiResponse
{
    public List<AccountUserApiResponseItem> AccountUsers { get; set; }

    public static implicit operator GetAccountUserApiResponse(GetUsersQueryResult source)
    {
        var users = source.AccountUsersResponse == null
            ? new List<AccountUserApiResponseItem>()
            : source.AccountUsersResponse.Select(x => (AccountUserApiResponseItem)x).ToList();

        return new GetAccountUserApiResponse
        {
            AccountUsers = users
        };
    }
    
    public record AccountUserApiResponseItem
    {
        public string UserRef { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool CanReceiveNotifications { get; set; }
        public InvitationStatus Status { get; set; }

        public static implicit operator AccountUserApiResponseItem(User source)
        {
            return new AccountUserApiResponseItem
            {
                Email = source.Email,
                CanReceiveNotifications = source.CanReceiveNotifications,
                Name = source.Name,
                Role = source.Role,
                UserRef = source.UserRef,
                Status = source.Status
            };
        }
    }
}