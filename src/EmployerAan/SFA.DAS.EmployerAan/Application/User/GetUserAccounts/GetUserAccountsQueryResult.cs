namespace SFA.DAS.EmployerAan.Application.User.GetUserAccounts;

public record GetUserAccountsQueryResult(string FirstName, string LastName, string EmployerUserId, bool IsSuspended, IEnumerable<EmployerAccount> UserAccountResponse);
