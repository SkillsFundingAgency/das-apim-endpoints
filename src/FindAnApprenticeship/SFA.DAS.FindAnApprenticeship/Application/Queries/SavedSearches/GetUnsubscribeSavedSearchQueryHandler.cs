using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;

public class GetUnsubscribeSavedSearchQueryHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ICourseService courseService) : IRequestHandler<GetUnsubscribeSavedSearchQuery, GetUnsubscribeSavedSearchQueryResult>
{
    public async Task<GetUnsubscribeSavedSearchQueryResult> Handle(GetUnsubscribeSavedSearchQuery request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}