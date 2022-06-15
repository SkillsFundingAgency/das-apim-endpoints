using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetExpiredShortlists
{
    public class GetExpiredShortlistsQuery : IRequest<GetExpiredShortlistsQueryResult>
    {
        public uint ExpiryInDays { get ; set ; }
    }
}