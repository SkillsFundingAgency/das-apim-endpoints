using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries;
public class GetEmployerAccountsQueryHandler(IInternalApiClient<AccountsConfiguration> client)
    : IRequestHandler<GetEmployerAccountsQuery, GetEmployerAccountsQueryResult>
{
    public async Task<GetEmployerAccountsQueryResult> Handle(GetEmployerAccountsQuery request, CancellationToken cancellationToken)
    {
        var response = new GetEmployerAccountsQueryResult();

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
            if (results?.Accounts != null)
            {
                foreach (var account in results.Accounts)
                {
                    response.Accounts.Add(new EmployerAccount
                    {
                        AccountId = account.AccountId,
                        DasAccountName = account.DasAccountName,
                        HashedAccountId = account.HashedAccountId,
                        PublicHashedAccountId = account.PublicHashedAccountId
                    });
                }
            }
        }
        
        return response;
    }

    private async Task AddAccountDetails(long accountId, GetEmployerAccountsQueryResult response)
    {
        var account = await client.Get<GetEmployerAccountByIdResponse>(
            new GetEmployerAccountByIdRequest(accountId));

        if (account != null)
        {
            response.Accounts.Add(new EmployerAccount
            {
                AccountId = account.AccountId,
                DasAccountName = account.DasAccountName,
                HashedAccountId = account.HashedAccountId,
                PublicHashedAccountId = account.PublicHashedAccountId
            });
        }
    }
}