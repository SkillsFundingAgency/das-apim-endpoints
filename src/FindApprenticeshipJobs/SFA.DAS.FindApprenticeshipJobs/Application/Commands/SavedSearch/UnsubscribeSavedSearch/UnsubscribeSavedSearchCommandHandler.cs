using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.UnsubscribeSavedSearch
{
    public class UnsubscribeSavedSearchCommandHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> apiClient)
        : IRequestHandler<UnsubscribeSavedSearchCommand>
    {
        public async Task Handle(UnsubscribeSavedSearchCommand request, CancellationToken cancellationToken)
        {
            await apiClient.Delete(new DeleteSavedSearchRequest(request.SavedSearchId));
        }
    }
}