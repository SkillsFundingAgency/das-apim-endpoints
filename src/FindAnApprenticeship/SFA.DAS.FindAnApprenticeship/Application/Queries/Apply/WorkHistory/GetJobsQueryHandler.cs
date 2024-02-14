using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory
{
    public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, GetJobsQueryResult>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

        public GetJobsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        {
            _candidateApiClient = candidateApiClient;
        }

        public async Task<GetJobsQueryResult> Handle(GetJobsQuery request, CancellationToken cancellationToken)
        {
            return await _candidateApiClient.Get<GetWorkHistoriesApiResponse>(new GetWorkHistoriesApiRequest(request.ApplicationId, request.CandidateId, WorkHistoryType.Job));
        }
    }
}
