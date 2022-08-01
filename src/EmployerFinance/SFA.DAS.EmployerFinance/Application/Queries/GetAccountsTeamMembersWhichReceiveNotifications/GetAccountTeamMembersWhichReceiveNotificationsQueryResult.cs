using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications
{
    public class GetAccountTeamMembersWhichReceiveNotificationsQueryResult : List<TeamMember>
    {
    }

    public class TeamMember
    {
        public string UserRef { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public bool CanReceiveNotifications { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public InvitationStatus Status { get; set; }
    }

    public enum InvitationStatus
    {
        Pending = 1,
        Accepted = 2,
        Expired = 3,
        Deleted = 4
    }
}
