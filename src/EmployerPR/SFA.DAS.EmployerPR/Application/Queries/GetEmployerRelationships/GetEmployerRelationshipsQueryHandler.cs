using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _apiClient) : IRequestHandler<GetEmployerRelationshipsQuery, GetEmployerRelationshipsQueryResult>
{
    public async Task<GetEmployerRelationshipsQueryResult> Handle(GetEmployerRelationshipsQuery query, CancellationToken cancellationToken)
    {
        var response =
            await _apiClient.Get<GetEmployerRelationshipsResponse>(
                new GetEmployerRelationshipsRequest(query.AccountHashedId, query.Ukprn, query.AccountlegalentityPublicHashedId));

        return new GetEmployerRelationshipsQueryResult(
            response.AccountLegalEntities.Select(a => (AccountLegalEntityPermissionsModel)a).ToList()
        );
    }
}
