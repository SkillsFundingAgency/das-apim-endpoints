using MediatR;
using SFA.DAS.LevyTransferMatching.Application.Queries.Shared.GetApplications;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQuery : GetApplicationsQueryBase, IRequest<GetApplicationsQueryResult>
    {

    }
}
