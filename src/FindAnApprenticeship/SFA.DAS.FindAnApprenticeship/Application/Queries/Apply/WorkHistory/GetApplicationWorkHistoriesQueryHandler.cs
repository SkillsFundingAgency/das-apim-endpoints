using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory
{
    public class GetApplicationWorkHistoriesQueryHandler : IRequestHandler<GetApplicationWorkHistoriesQuery, GetApplicationWorkHistoriesQueryResult>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

        public GetApplicationWorkHistoriesQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        {
            _candidateApiClient = candidateApiClient;
        }

        public async Task<GetApplicationWorkHistoriesQueryResult> Handle(GetApplicationWorkHistoriesQuery request, CancellationToken cancellationToken)
        {
            var workHistories = await _candidateApiClient.Get<List<GetWorkHistoriesApiResponse>>(new GetWorkHistoriesApiRequest(request.CandidateId, request.ApplicationId));

            return new GetApplicationWorkHistoriesQueryResult
            {
                WorkHistories = workHistories.Select(wk => (Models.WorkHistory)wk).ToList()
            };
        }
    }
}
