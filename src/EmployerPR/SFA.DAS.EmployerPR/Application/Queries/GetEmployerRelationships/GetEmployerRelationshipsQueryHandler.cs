using MediatR;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<GetEmployerRelationshipsQuery, GetEmployerRelationshipsQueryResult>
{
    public async Task<GetEmployerRelationshipsQueryResult> Handle(GetEmployerRelationshipsQuery query, CancellationToken cancellationToken)
    {
        var response =
            await _providerRelationshipsApiRestClient.GetEmployerRelationships(
                query.AccountHashedId, 
                query.Ukprn, 
                query.AccountlegalentityPublicHashedId, 
                cancellationToken
        );

        return new GetEmployerRelationshipsQueryResult(
            response.AccountLegalEntities.Select(a => (AccountLegalEntityPermissionsModel)a).ToList()
        );
    }
}
