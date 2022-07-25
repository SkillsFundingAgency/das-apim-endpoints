using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries
{
    public class GetOverlappingApprenticeshipDetailsQueryHandler : IRequestHandler<GetOverlappingApprenticeshipDetailsQuery, GetOverlappingApprenticeshipDetailsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public async Task<GetOverlappingApprenticeshipDetailsQueryResult> Handle(GetOverlappingApprenticeshipDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWithResponseCode<GetOverlappingApprenticeshipDetailsResponse>(
             new GetOverlappingApprenticeshipDetailsRequest(request.ProviderId, request.DraftApprenticeshipId));

            response.EnsureSuccessStatusCode();

            return new GetOverlappingApprenticeshipDetailsQueryResult { ApprenticeshipId = response.Body.ApprenticeshipId, 
                Status = response.Body.Status };
        }
    }
}
