using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndexLocation
{
    public class IndexLocationQuery : IRequest<IndexLocationQueryResult>
    {
        public string LocationSearchTerm { get; set; }
    }
}