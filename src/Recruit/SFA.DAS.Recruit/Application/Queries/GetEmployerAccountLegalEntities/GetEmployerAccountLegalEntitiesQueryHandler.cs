using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetEmployerAccountLegalEntities;

public class GetEmployerAccountLegalEntitiesQueryHandler(IAccountLegalEntityPermissionService accountLegalEntityPermissionService)
: MediatR.IRequestHandler<GetEmployerAccountLegalEntitiesQuery, GetEmployerAccountLegalEntitiesQueryResult>
{
    public async Task<GetEmployerAccountLegalEntitiesQueryResult> Handle(
        GetEmployerAccountLegalEntitiesQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await accountLegalEntityPermissionService
            .GetEmployerAccountLegalEntities(
                request.AccountHashedId,
                request.Operations) ?? [];

        return new GetEmployerAccountLegalEntitiesQueryResult(permissions);
    }
}