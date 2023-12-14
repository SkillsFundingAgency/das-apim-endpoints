using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

public class SearchIndexQuery : IRequest<SearchIndexQueryResult>
{
    public string LocationSearchTerm { get; set; }
}