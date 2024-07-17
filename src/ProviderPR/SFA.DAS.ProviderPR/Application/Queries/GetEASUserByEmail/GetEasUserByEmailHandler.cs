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

        var userRef = result?.Ref;

        if (userRef == null || userRef == Guid.Empty)
        {
            return new GetEasUserByEmailQueryResult { HasUserAccount = false };
        }

        var accounts = await apiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userRef.ToString()));

        if (accounts.Count() != 1)
        {
            return new GetEasUserByEmailQueryResult { HasUserAccount = true, HasOneEmployerAccount = false };
        }

        var account = accounts.FirstOrDefault();
        var hashedId = account!.EncodedAccountId;

        var accountLegalEntities = await apiClient.GetAll<GetAccountLegalEntityResponse>(new GetAccountLegalEntitiesRequest(hashedId));

        var hasOneLegalEntity = accountLegalEntities.Count() == 1;

        return new GetEasUserByEmailQueryResult { HasUserAccount = true, AccountId = account.AccountId, HasOneEmployerAccount = true, HasOneLegalEntity = hasOneLegalEntity };
    }
}
