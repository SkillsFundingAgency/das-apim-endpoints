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
    public class GetOverlapRequestHandler : IRequestHandler<GetOverlapRequestQuery, GetOverlapRequestResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public GetOverlapRequestHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetOverlapRequestResult> Handle(GetOverlapRequestQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWithResponseCode<GetOverlapRequestResponse>(new GetOverlapRequestRequest(request.DraftApprneticeshipId));
            response.EnsureSuccessStatusCode();
            return new GetOverlapRequestResult
            {
                DraftApprenticeshipId = response.Body?.DraftApprenticeshipId,
                PreviousApprenticeshipId = response.Body?.PreviousApprenticeshipId,
                CreatedOn = response.Body?.CreatedOn
            };
        }
    }
}