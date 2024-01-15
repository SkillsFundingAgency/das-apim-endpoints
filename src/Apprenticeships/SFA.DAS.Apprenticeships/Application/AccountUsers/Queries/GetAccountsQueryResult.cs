namespace SFA.DAS.Apprenticeships.Application.AccountUsers.Queries
{
    public class GetAccountsQueryResult
    {
        public IEnumerable<AccountUser> UserAccountResponse { get ; set ; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployerUserId { get; set; }
        public bool IsSuspended { get; set; }
    }
}