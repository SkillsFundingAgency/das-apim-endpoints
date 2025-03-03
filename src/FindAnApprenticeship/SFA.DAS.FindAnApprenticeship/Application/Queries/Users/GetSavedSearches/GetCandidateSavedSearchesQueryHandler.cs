using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearches;

public class GetCandidateSavedSearchesQueryHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ICourseService courseService) : IRequestHandler<GetCandidateSavedSearchesQuery, GetCandidateSavedSearchesQueryResult>
{
    public async Task<GetCandidateSavedSearchesQueryResult> Handle(GetCandidateSavedSearchesQuery request, CancellationToken cancellationToken)
    {
        var routesTask = courseService.GetRoutes();
        var savedSearchesTask = findApprenticeshipApiClient.Get<GetCandidateSavedSearchesApiResponse>(
                new GetCandidateSavedSearchesApiRequest(request.CandidateId));
        
        await Task.WhenAll(routesTask, savedSearchesTask);
        
        return GetCandidateSavedSearchesQueryResult.From(savedSearchesTask.Result, routesTask.Result);
    }
}