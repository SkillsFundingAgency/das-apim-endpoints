using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries
{
    public class GetCohortQueryHandler : IRequestHandler<GetCohortQuery, GetCohortResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public GetCohortQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetCohortResult> Handle(GetCohortQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetCohortResponse>(new GetCohortRequest(request.CohortId));

            if (result == null)
                return null;

            return (GetCohortResult)result;
        }
    }
}
