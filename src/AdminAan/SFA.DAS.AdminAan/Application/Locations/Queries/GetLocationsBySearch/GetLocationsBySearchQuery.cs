using MediatR;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.AdminAan.Application.Locations.Queries.GetLocationsBySearch
{
    public class GetLocationsBySearchQuery(string searchTerm) : IRequest<GetLocationsBySearchQueryResult>
    {
        public string SearchTerm { get; set; } = searchTerm;
    }
}