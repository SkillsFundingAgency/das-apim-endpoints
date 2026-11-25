using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprnAndAccountId;

public class GetProviderPermissionsByUkprnAndAccountIdQueryHandler(IAccountLegalEntityPermissionService accountLegalEntityPermissionService)
    : MediatR.IRequestHandler<GetProviderPermissionsByUkprnAndAccountIdQuery, GetProviderPermissionsByUkprnAndAccountIdQueryResult>
{
    public async Task<GetProviderPermissionsByUkprnAndAccountIdQueryResult> Handle(
        GetProviderPermissionsByUkprnAndAccountIdQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await accountLegalEntityPermissionService
            .GetProviderPermissionsForEmployerAccountLegalEntities(
                request.Ukprn,
                request.AccountId,
                request.Operations) ?? [];

        return new GetProviderPermissionsByUkprnAndAccountIdQueryResult(permissions);
    }
}