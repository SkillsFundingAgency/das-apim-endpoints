using MediatR;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Application.Queries.GetPermissions;

public class GetPermissionsHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<GetPermissionsQuery, GetPermissionsResponse>
{
    public async Task<GetPermissionsResponse> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        var response =
            await _providerRelationshipsApiRestClient.GetPermissions(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

        return response;
    }
}