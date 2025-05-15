using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries.SearchEmployerAccounts;
public class SearchEmployerAccountsQueryHandler(IInternalApiClient<AccountsConfiguration> client)
    : IRequestHandler<SearchEmployerAccountsQuery, SearchEmployerAccountsQueryResult>
{
    public async Task<SearchEmployerAccountsQueryResult> Handle(SearchEmployerAccountsQuery request, CancellationToken cancellationToken)
    {
        var response = new SearchEmployerAccountsQueryResult();

        if (request.AccountId != null)
        {
            await AddAccountDetails(request.AccountId.Value, response);
        }
        else if (request.PayeSchemeRef != null)
        {
            var payeDetail = await client.Get<GetEmployerAccountByPayeResponse>(new GetEmployerAccountByPayeRequest(request.PayeSchemeRef));
            if (payeDetail != null)
            {
                await AddAccountDetails(payeDetail.AccountId, response);
            }
        }
        else if (!string.IsNullOrWhiteSpace(request.EmployerName))
        {
            var results = await client.Get<GetEmployerAccountsByNameResponse>(new GetEmployerAccountsByNameRequest(request.EmployerName));
            if (results?.EmployerAccounts != null)
            {
                response.EmployerAccounts = results.EmployerAccounts.ConvertAll(acc => (EmployerAccount)acc);
            }
        }
        
        return response;
    }

    private async Task AddAccountDetails(long accountId, SearchEmployerAccountsQueryResult response)
    {
        var account = await client.Get<GetEmployerAccountByIdResponse>(
            new GetEmployerAccountByIdRequest(accountId));

        if (account != null)
        {
            response.EmployerAccounts.Add(new EmployerAccount
            {
                AccountId = account.AccountId,
                DasAccountName = account.DasAccountName,
                HashedAccountId = account.HashedAccountId,
                PublicHashedAccountId = account.PublicHashedAccountId
            });
        }
    }
}