using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
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
            .GetProviderAccountLegalEntities(
                request.Ukprn,
                [Operation.Recruitment, Operation.RecruitmentRequiresReview]) ?? [];

        return new GetProviderPermissionsByUkprnQueryResult(accountLegalEntities);
    }
}