using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetApprenticeship;

public class GetApprenticeshipQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IMapper mapper) : IRequestHandler<GetApprenticeshipQuery, GetApprenticeshipQueryResult>
{
    public async Task<GetApprenticeshipQueryResult> Handle(GetApprenticeshipQuery request, CancellationToken cancellationToken)
    {
        var innerApiRequest = new GetApprenticeshipRequest(request.ApprenticeshipId);
        var innerApiResponse = await apiClient.GetWithResponseCode<GetApprenticeshipResponse>(innerApiRequest);

        if (innerApiResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        return mapper.Map<GetApprenticeshipQueryResult>(innerApiResponse.Body);
    }
}