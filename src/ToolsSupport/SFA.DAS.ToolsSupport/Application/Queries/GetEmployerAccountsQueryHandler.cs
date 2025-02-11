using System.Net;
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
            var account = await client.Get<GetEmployerAccountByIdResponse>(
                    new GetEmployerAccountByIdRequest(request.AccountId.Value));

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
        else if (request.PayeSchemeRef != null)
        {

        }

        return response;
    }
}