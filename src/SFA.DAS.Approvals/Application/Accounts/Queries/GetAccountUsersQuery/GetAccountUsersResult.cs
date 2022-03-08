namespace SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountUsersQuery
{
    public class GetAccountUsersResult
    {
        public string UserRef { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public bool CanReceiveNotifications { get; set; }

        public int Status { get; set; }
    }
}