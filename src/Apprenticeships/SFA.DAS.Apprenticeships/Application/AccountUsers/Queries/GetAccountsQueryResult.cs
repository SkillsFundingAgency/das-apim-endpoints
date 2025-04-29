namespace SFA.DAS.Apprenticeships.Application.AccountUsers.Queries
{
    public class GetAccountsQueryResult
    {
        public IEnumerable<AccountUser> UserAccountResponse { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmployerUserId { get; set; } = null!;
        public bool IsSuspended { get; set; }
    }
}