using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.UnsubscribeSavedSearch
{
    public record UnsubscribeSavedSearchCommand(Guid SavedSearchId) : IRequest;
}