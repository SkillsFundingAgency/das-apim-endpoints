using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications;
using ResultTeamMember = SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications.TeamMember;

namespace SFA.DAS.EmployerFinance.Api.Models.Accounts
{
    public class GetAccountTeamMembersWhichReceiveNotificationsResponse : List<TeamMember>
    {
        public static explicit operator GetAccountTeamMembersWhichReceiveNotificationsResponse(GetAccountTeamMembersWhichReceiveNotificationsQueryResult result)
        {
            var response = new GetAccountTeamMembersWhichReceiveNotificationsResponse();
            result.ForEach(p => response.Add((TeamMember)p));
            return response;
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

        public static explicit operator TeamMember(ResultTeamMember teamMember)
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
