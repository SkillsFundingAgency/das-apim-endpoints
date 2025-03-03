using System.Net;
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
        var savedSearchTask = findApprenticeshipApiClient.GetWithResponseCode<GetSavedSearchUnsubscribeApiResponse>(
            new GetSavedSearchApiRequest(request.SavedSearchId));

        await Task.WhenAll(routesTask, savedSearchTask);

        if (savedSearchTask.Result.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetUnsubscribeSavedSearchQueryResult(null, routesTask.Result.Routes);    
        }
        
        return GetUnsubscribeSavedSearchQueryResult.From(savedSearchTask.Result.Body, routesTask.Result);
    }
}