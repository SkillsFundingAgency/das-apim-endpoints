using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;

namespace SFA.DAS.ProviderPR.Application.Queries.GetEasUserByEmail;
public class GetEasUserByEmailQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient) : IRequestHandler<GetEasUserByEmailQuery, GetEasUserByEmailQueryResult>
{
    public async Task<GetEasUserByEmailQueryResult> Handle(GetEasUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.GetWithResponseCode<GetUserByEmailResponse>(new GetUserByEmailRequest(request.Email));

        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetEasUserByEmailQueryResult(false, null, null, null);
        }

        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Error calling get user by email for {request.Email}");
        }

        var userRef = result.Body.Ref;

        var accounts = await apiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userRef.ToString()));

        if (!accounts.Any())
        {
            return new GetEasUserByEmailQueryResult(false, null, null, null);
        }

        if (accounts.Count() > 1)
        {
            return new GetEasUserByEmailQueryResult(true, false, null, null);
        }

        var account = accounts.First();
        var accountId = account!.AccountId;

        var accountLegalEntities = await apiClient.GetAll<GetAccountLegalEntityResponse>(new GetAccountLegalEntitiesRequest(accountId));

        var hasOneLegalEntity = accountLegalEntities.Count() == 1;

        return new GetEasUserByEmailQueryResult(true, true, account.AccountId, hasOneLegalEntity);
    }
}
