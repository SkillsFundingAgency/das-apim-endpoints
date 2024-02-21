using MediatR;
using SFA.DAS.EmployerAan.Models;
using SFA.DAS.EmployerAan.Services;

namespace SFA.DAS.EmployerAan.Application.User.GetUserAccounts;

public class GetUserAccountsQueryHandler : IRequestHandler<GetUserAccountsQuery, GetUserAccountsQueryResult>
{
    private readonly IEmployerAccountsService _employerAccountService;

    public GetUserAccountsQueryHandler(IEmployerAccountsService employerAccountService)
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

        var response = await _employerAccountService.GetEmployerAccounts(profile, cancellationToken);

        var employerAccountUsers = response.ToList();
        var account = employerAccountUsers.FirstOrDefault();

        if (account == null) throw new InvalidOperationException($"No employer accounts found for Employer UserId: {request.UserId} and Email: {request.Email}");

        var employerAccounts = employerAccountUsers.Where(c => c.EncodedAccountId != null).Select(c => (EmployerAccount)c);

        return new GetUserAccountsQueryResult(account.FirstName, account.LastName, account.UserId, account.IsSuspended, employerAccounts);
    }
}