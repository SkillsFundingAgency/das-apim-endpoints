using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions;

public class ApplicationsForAutomaticDeclineQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
    : IRequestHandler<ApplicationsForAutomaticDeclineQuery, ApplicationsForAutomaticDeclineResult>
{
    public async Task<ApplicationsForAutomaticDeclineResult> Handle(ApplicationsForAutomaticDeclineQuery request, CancellationToken cancellationToken)
    {
        var getApplicationsResponse = await levyTransferMatchingService.GetApplicationsToAutoDecline(new GetApplicationsToAutoDeclineRequest());

        var applicationIdsToDecline = getApplicationsResponse != null ? getApplicationsResponse.ApplicationIdsToDecline : [];

        return new ApplicationsForAutomaticDeclineResult
        {
            ApplicationIdsToDecline = applicationIdsToDecline
        };
    }
}
