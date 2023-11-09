using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterestsLocation
{
    public class BrowseByInterestsLocationQuery : IRequest<BrowseByInterestsLocationQueryResult>
    {
        public string LocationSearchTerm { get; set; }
    }
}