using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissions;

public class GetProviderPermissionsByUkprnQueryHandler(IAccountLegalEntityPermissionService accountLegalEntityPermissionService)
    : MediatR.IRequestHandler<GetProviderPermissionsByUkprnQuery, GetProviderPermissionsByUkprnQueryResult>
{
    public async Task<GetProviderPermissionsByUkprnQueryResult> Handle(GetProviderPermissionsByUkprnQuery request, CancellationToken cancellationToken)
    {
        var permissions = await accountLegalEntityPermissionService.GetProviderPermissionsForEmployer(request.Ukprn,
            [Operation.Recruitment, Operation.RecruitmentRequiresReview]);

        return new GetProviderPermissionsByUkprnQueryResult(permissions);
    }
}