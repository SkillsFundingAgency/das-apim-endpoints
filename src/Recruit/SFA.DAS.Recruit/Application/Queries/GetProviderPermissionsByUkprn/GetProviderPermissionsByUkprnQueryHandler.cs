using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprn;

public class GetProviderPermissionsByUkprnQueryHandler(IAccountLegalEntityPermissionService accountLegalEntityPermissionService)
    : MediatR.IRequestHandler<GetProviderPermissionsByUkprnQuery, GetProviderPermissionsByUkprnQueryResult>
{
    public async Task<GetProviderPermissionsByUkprnQueryResult> Handle(
        GetProviderPermissionsByUkprnQuery request,
        CancellationToken cancellationToken)
    {
        var accountLegalEntities = await accountLegalEntityPermissionService
            .GetProviderPermissionsAccountLegalEntities(
                request.Ukprn,
                request.Operations) ?? [];

        return new GetProviderPermissionsByUkprnQueryResult(accountLegalEntities);
    }
}