using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetCandidatesByActivity
{
    public record GetCandidateByActivityQuery(DateTime CutOffDateTime) : IRequest<GetCandidateByActivityQueryResult>;
}