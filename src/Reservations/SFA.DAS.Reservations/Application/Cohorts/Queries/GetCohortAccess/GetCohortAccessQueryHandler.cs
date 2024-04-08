using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.Cohorts.Queries.GetCohortAccess;

public class GetCohortAccessQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient) : IRequestHandler<GetCohortAccessQuery, bool>
{
    public async Task<bool> Handle(GetCohortAccessQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetCohortAccessRequest(request.Party, request.PartyId, request.CohortId);

        return await apiClient.Get<bool>(apiRequest);
    }
}