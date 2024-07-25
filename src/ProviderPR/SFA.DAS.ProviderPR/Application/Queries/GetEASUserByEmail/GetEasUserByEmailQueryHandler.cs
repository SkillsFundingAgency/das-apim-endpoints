using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderPR.Application.Queries.GetEasUserByEmail;
public class GetEasUserByEmailQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient) : IRequestHandler<GetEasUserByEmailQuery, GetEasUserByEmailQueryResult>
{
    public async Task<GetEasUserByEmailQueryResult> Handle(GetEasUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetUserByEmailResponse>(new GetUserByEmailRequest(request.Email));

        if (result == null)
        {
            return new GetEasUserByEmailQueryResult(false, false, null, false);
        }

        var userRef = result.Ref;

        var accounts = await apiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userRef.ToString()));

        if (accounts.Count() > 1)
        {
            return new GetEasUserByEmailQueryResult(true, false, null, false);
        }

        var account = accounts.First();
        var accountId = account!.AccountId;

        var accountLegalEntities = await apiClient.GetAll<GetAccountLegalEntityResponse>(new GetAccountLegalEntitiesRequest(accountId));

        var hasOneLegalEntity = accountLegalEntities.Count() == 1;

        return new GetEasUserByEmailQueryResult(true, true, account.AccountId, hasOneLegalEntity);
    }
}
