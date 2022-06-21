using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountUsersQuery
{
    public class GetAccountUsersResult : List<TeamMember>
    {
        public string HashedAccountId { get; set; }

        public GetAccountUsersResult(string hashedAccountId, IEnumerable<TeamMember> teamMembers)
        {
            HashedAccountId = hashedAccountId;
            AddRange(teamMembers);
        }
    }

    public class TeamMember
    {
        public string UserRef { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public bool CanReceiveNotifications { get; set; }

        public string Status { get; set; }
    }
}