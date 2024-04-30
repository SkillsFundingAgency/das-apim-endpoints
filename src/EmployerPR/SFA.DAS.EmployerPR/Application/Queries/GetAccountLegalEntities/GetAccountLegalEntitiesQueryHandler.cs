using MediatR;
using SFA.DAS.EmployerPR.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;
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
            await _apiClient.GetAll<AccountLegalEntity>(
                new GetAccountLegalEntitiesRequest(request.HashedAccountId));

        return new GetAccountLegalEntitiesQueryResult
        {
            LegalEntities = accountLegalEntities?.Select(legalEntities => (LegalEntity)legalEntities).ToList()
        };
    }
}
