using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetCandidatesByActivity
{
    public class GetCandidateByActivityQueryHandler(
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetCandidateByActivityQuery, GetCandidateByActivityQueryResult>
    {
        public async Task<GetCandidateByActivityQueryResult> Handle(GetCandidateByActivityQuery request, CancellationToken cancellationToken)
        {
            return await candidateApiClient.Get<GetCandidatesByActivityApiResponse>(
                new GetCandidatesByActivityApiRequest(request.CutOffDateTime));
        }
    }
}
