using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch
{
    public class GetLocationsBySearchQuery : IRequest<GetLocationsBySearchQueryResult>
    {
        public string SearchTerm { get; set; }
    }
}
