using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchResults
{
    public class SearchResultsQuery : IRequest<SearchResultsQueryResult>
    {
        public List<string> RouteIds { get; set; }
        public string Location { get; set; }
        public string SearchTerm { get; set; }

        //public long Latitude { get; set; }
        //public long Longitude { get; set; }
    }
}
