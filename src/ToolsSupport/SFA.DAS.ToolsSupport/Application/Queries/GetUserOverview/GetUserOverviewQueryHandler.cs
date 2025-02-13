using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetUserOverview;

public class GetUserOverviewQueryHandler(
    IInternalApiClient<EmployerProfilesApiConfiguration> client,
     IAccountsService accountsService
    ) : IRequestHandler<GetUserOverviewQuery, GetUserOverviewQueryResult>
{
    public async Task<GetUserOverviewQueryResult> Handle(GetUserOverviewQuery query, CancellationToken cancellationToken)
    {
        var accountsTask = accountsService.GetUserAccounts(query.UserId);

        var userResponseTask = client.Get<GetUserOverviewResponse>(new GetUserByIdRequest(query.UserId));

        await Task.WhenAll(accountsTask, userResponseTask);

        var accounts = await accountsTask;
        var userResponse = await userResponseTask;
        if (userResponse == null)
        {
            return new GetUserOverviewQueryResult();
        }

        return new GetUserOverviewQueryResult
        {
            Id = userResponse.Id,
            FirstName = userResponse.FirstName,
            LastName = userResponse.LastName,
            Email = userResponse.Email,
            IsActive = userResponse.IsActive,
            IsLocked = userResponse.IsLocked,
            IsSuspended = userResponse.IsSuspended,
            AccountSummaries =  accounts != null ? accounts.Select(x =>
                new AccountSummary
                {
                    DasAccountName = x.DasAccountName,
                    HashedAccountId = x.HashedAccountId
                }).ToList() : [],
        };
    }
}
