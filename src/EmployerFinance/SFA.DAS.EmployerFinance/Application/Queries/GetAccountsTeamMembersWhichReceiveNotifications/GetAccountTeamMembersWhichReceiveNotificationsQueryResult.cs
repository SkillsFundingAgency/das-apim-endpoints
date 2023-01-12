using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using InnerApiResponseTeamMember = SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse.TeamMember;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications
{
    public class GetAccountTeamMembersWhichReceiveNotificationsQueryResult : List<TeamMember>
    {
        public static explicit operator GetAccountTeamMembersWhichReceiveNotificationsQueryResult(GetAccountTeamMembersWhichReceiveNotificationsResponse response)
        {
            var result = new GetAccountTeamMembersWhichReceiveNotificationsQueryResult();
            response.ForEach(p => result.Add((TeamMember)p));
            return result;
        }
    }

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

        public static explicit operator TeamMember(InnerApiResponseTeamMember teamMember)
        {
            return new TeamMember
            {
                UserRef = teamMember.UserRef,
                FirstName = teamMember.FirstName,
                LastName = teamMember.LastName,
                Name = teamMember.Name,
                Email = teamMember.Email,
                Role = teamMember.Role,
                CanReceiveNotifications = teamMember.CanReceiveNotifications,
                Status = (InvitationStatus)Enum.Parse(typeof(InvitationStatus), teamMember.Status.ToString())
            };
        }
    }

    public enum InvitationStatus
    {
        Pending = 1,
        Accepted = 2,
        Expired = 3,
        Deleted = 4
    }
}
