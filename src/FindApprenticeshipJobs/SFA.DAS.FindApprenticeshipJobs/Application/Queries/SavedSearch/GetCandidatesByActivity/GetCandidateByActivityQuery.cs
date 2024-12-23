using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetCandidatesByActivity
{
    public record GetCandidateByActivityQuery(
        DateTime CutOffDateTime,
        int PageNumber = 1,
        int PageSize = 10) : IRequest<GetCandidateByActivityQueryResult>;
}