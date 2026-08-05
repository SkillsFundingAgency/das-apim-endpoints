using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.EmployerPR.Application.AccountLegalEntities.Queries.GetAccountLegalEntities;
public class GetAccountLegalEntitiesQueryHandler : IRequestHandler<GetAccountLegalEntitiesQuery, GetAccountLegalEntitiesQueryResult>
{
    private readonly IAccountsApiClient<AccountsConfiguration> _apiClient;

    public GetAccountLegalEntitiesQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetAccountLegalEntitiesQueryResult> Handle(GetAccountLegalEntitiesQuery request, CancellationToken cancellationToken)
    {
        var accountLegalEntities =
            await _apiClient.GetAll<GetAccountLegalEntityResponse>(
                new GetAccountLegalEntitiesRequest(request.AccountId));

        return new GetAccountLegalEntitiesQueryResult
        {
            LegalEntities = accountLegalEntities.Select(legalEntities => (LegalEntity)legalEntities).ToList()
        };
    }
}
