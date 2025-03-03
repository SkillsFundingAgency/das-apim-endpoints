using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;

public class GetApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService) : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
{
    public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applicationsResponse = await levyTransferMatchingService.GetApplications(new GetApplicationsRequest
        {
            AccountId = request.AccountId,
            Page = request.Page,
            PageSize = request.PageSize ?? int.MaxValue
        });

        return applicationsResponse;
    }
}