using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetLocationsBySearch
{
    public class GetLocationsBySearchQuery(string searchTerm) : IRequest<GetLocationsBySearchQueryResult>
    {
        public string SearchTerm { get; set; } = searchTerm;
    }
}