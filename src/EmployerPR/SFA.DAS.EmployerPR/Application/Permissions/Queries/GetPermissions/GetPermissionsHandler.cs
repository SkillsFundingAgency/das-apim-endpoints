using MediatR;
using SFA.DAS.EmployerPR.Infrastructure;
using System.Net;

namespace SFA.DAS.EmployerPR.Application.Permissions.Queries.GetPermissions;

public class GetPermissionsHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<GetPermissionsQuery, GetPermissionsResponse>
{
    public async Task<GetPermissionsResponse> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        var response =
            await _providerRelationshipsApiRestClient.GetPermissions(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

        switch (response.ResponseMessage.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    return response.GetContent();
                }
            case HttpStatusCode.NotFound:
                return null!;
            default:
                throw new InvalidOperationException(
                    $"Invalid operation occurred trying to retrieve permissions for ukprn {query.Ukprn} and AccountLegalEntityId {query.AccountLegalEntityId}");
        }
    }
}