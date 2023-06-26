using MediatR;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.EmployerAan.Application.User.GetUserAccounts;
public record GetUserAccountsQuery(string UserId, string Email) : IRequest<GetUserAccountsQueryResult>;

public record GetUserAccountsQueryResult(string FirstName, string LastName, string EmployerUserId, bool IsSuspended, IEnumerable<EmployerAccount> UserAccountResponse);

public record EmployerAccount(string DasAccountName, string EncodedAccountId, string Role);

public class GetUserAccountQueryHandler : IRequestHandler<GetUserAccountsQuery, GetUserAccountsQueryResult>
{
    private readonly IEmployerAccountsService _employerAccountService;

    public GetUserAccountQueryHandler(IEmployerAccountsService employerAccountService)
    {
        _employerAccountService = employerAccountService;
    }

    public async Task<GetUserAccountsQueryResult> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
    {
        var profile = new EmployerProfile
        {
            Email = request.Email,
            UserId = request.UserId
        };

        var response = await _employerAccountService.GetEmployerAccounts(profile);

        var employerAccountUsers = response.ToList();
        var account = employerAccountUsers.FirstOrDefault();

        if (account == null) throw new InvalidOperationException($"No employer accounts found for Employer UserId: {request.UserId} and Email: {request.Email}");

        var employerAccounts = employerAccountUsers.Where(c => c.EncodedAccountId != null).Select(c => new EmployerAccount(c.DasAccountName, c.EncodedAccountId, c.Role));

        return new GetUserAccountsQueryResult(account.FirstName, account.LastName, account.UserId, account.IsSuspended, employerAccounts);
    }
}