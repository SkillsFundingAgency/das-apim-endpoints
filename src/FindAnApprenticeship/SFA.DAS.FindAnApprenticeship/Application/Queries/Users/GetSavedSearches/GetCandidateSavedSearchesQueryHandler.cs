using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearches;

public class GetCandidateSavedSearchesQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient) : IRequestHandler<GetCandidateSavedSearchesQuery, GetCandidateSavedSearchesQueryResult>
{
    public async Task<GetCandidateSavedSearchesQueryResult> Handle(GetCandidateSavedSearchesQuery request, CancellationToken cancellationToken)
    {
        var result = await findApprenticeshipApiClient.Get<GetCandidateSavedSearchesApiResponse>(new GetCandidateSavedSearchesApiRequest(request.CandidateId));
        return GetCandidateSavedSearchesQueryResult.From(result);
    }
}