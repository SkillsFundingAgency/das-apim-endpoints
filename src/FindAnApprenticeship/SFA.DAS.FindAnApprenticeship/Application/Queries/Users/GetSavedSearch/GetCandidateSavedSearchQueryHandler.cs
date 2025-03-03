using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearch;

public class GetCandidateSavedSearchQueryHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ICourseService courseService) : IRequestHandler<GetCandidateSavedSearchQuery, GetCandidateSavedSearchQueryResult>
{
    public async Task<GetCandidateSavedSearchQueryResult> Handle(GetCandidateSavedSearchQuery request, CancellationToken cancellationToken)
    {
        var routesTask = courseService.GetRoutes();
        var savedSearchTask = findApprenticeshipApiClient.Get<GetCandidateSavedSearchApiResponse>(
                new GetCandidateSavedSearchApiRequest(request.CandidateId, request.Id));

        await Task.WhenAll(routesTask, savedSearchTask);

        return GetCandidateSavedSearchQueryResult.From(savedSearchTask.Result, routesTask.Result);
    }
}