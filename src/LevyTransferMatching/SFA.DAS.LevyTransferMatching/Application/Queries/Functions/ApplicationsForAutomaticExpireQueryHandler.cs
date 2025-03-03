using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions;

public class ApplicationsForAutomaticExpireQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
    : IRequestHandler<ApplicationsForAutomaticExpireQuery, ApplicationsForAutomaticExpireResult>
{
    public async Task<ApplicationsForAutomaticExpireResult> Handle(ApplicationsForAutomaticExpireQuery request, CancellationToken cancellationToken)
    {
        var getApplicationsResponse = await levyTransferMatchingService.GetApplicationsToAutoExpire(new GetApplicationsToAutoExpireRequest());

        var applicationIdsToExpire = getApplicationsResponse != null ? getApplicationsResponse.ApplicationIdsToExpire : [];

        return new ApplicationsForAutomaticExpireResult
        {
            ApplicationIdsToExpire = applicationIdsToExpire
        };
    }
}