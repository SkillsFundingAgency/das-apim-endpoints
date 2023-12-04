using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndexLocation
{
    public class IndexLocationQuery : IRequest<IndexLocationResult>
    {
        public string LocationSearchTerm { get; set; }
    }
}