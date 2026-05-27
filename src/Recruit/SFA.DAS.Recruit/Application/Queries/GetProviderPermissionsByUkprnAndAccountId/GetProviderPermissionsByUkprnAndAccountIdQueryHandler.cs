using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprnAndAccountId;

public class GetProviderPermissionsByUkprnQueryHandler(IAccountsApiClient<AccountsConfiguration> accountsApi,
    IAccountLegalEntityPermissionService accountLegalEntityPermissionService)
    : MediatR.IRequestHandler<GetProviderPermissionsByUkprnAndAccountIdQuery, GetProviderPermissionsByUkprnAndAccountIdQueryResult>
{
    public async Task<GetProviderPermissionsByUkprnAndAccountIdQueryResult> Handle(
        GetProviderPermissionsByUkprnAndAccountIdQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await accountLegalEntityPermissionService
            .GetProviderPermissionsForEmployer(
                request.Ukprn,
                request.AccountId,
                [Operation.Recruitment, Operation.RecruitmentRequiresReview]) ?? [];

        var accountLegalEntitiesResponse =
            await accountsApi.GetAll<GetAccountLegalEntityResponseItem>(
                new GetAccountLegalEntitiesRequest(request.AccountId));

        var permittedAccountLegalEntityIds = permissions
            .Select(x => x.AccountLegalEntityId)
            .ToHashSet();

        var accountLegalEntities = accountLegalEntitiesResponse
            .Where(x => permittedAccountLegalEntityIds.Contains(x.AccountLegalEntityId))
            .ToList();

        return new GetProviderPermissionsByUkprnAndAccountIdQueryResult(accountLegalEntities);
    }
}