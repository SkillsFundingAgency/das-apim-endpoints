using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetInactiveCandidates
{
    public record GetInactiveCandidatesQuery(
        DateTime CutOffDateTime,
        int PageNumber = 1,
        int PageSize = 10) : IRequest<GetInactiveCandidatesQueryResult>;
}