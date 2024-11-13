using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;

public class GetUnsubscribeSavedSearchQueryHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ICourseService courseService) : IRequestHandler<GetUnsubscribeSavedSearchQuery, GetUnsubscribeSavedSearchQueryResult>
{
    public async Task<GetUnsubscribeSavedSearchQueryResult> Handle(GetUnsubscribeSavedSearchQuery request, CancellationToken cancellationToken)
    {
        var routesTask = courseService.GetRoutes();
        var savedSearchTask = findApprenticeshipApiClient.Get<GetSavedSearchUnsubscribeApiResponse>(
            new GetSavedSearchUnsubscribeApiRequest(request.SavedSearchId));

        await Task.WhenAll(routesTask, savedSearchTask);

        return GetUnsubscribeSavedSearchQueryResult.From(savedSearchTask.Result, routesTask.Result);
    }
}