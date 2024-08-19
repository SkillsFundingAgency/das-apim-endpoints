using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses;

public class GetAccountTeamMembersWhichReceiveNotificationsResponse : List<TeamMember>
{
    public class TeamMember
    {
        public string UserRef { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool CanReceiveNotifications { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public InvitationStatus Status { get; set; }
    }
}

public enum InvitationStatus
{
    Pending = 1,
    Accepted = 2,
    Expired = 3,
    Deleted = 4
}

public enum Role
{
    None = 0,
    Owner = 1,
    Transactor = 2,
    Viewer = 3
}

public static class TeamMemberExtensions
{
    public static bool IsAcceptedOwnerWithNotifications(this TeamMember member)
    {
        return member.Status == InvitationStatus.Accepted &&
               member.Role == Role.Owner.ToString("D") &&
               member.CanReceiveNotifications;
    }

    public static bool IsAccountOwner(this TeamMember member)
    {
        return member.Role == Role.Owner.ToString("D");
    }
}