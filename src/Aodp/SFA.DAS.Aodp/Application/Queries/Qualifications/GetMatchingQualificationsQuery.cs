using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetMatchingQualificationsQuery : IRequest<BaseMediatrResponse<GetMatchingQualificationsQueryResponse>>
{
    public string SearchTerm { get; set; }
    
    // Pagination
    public int? Skip { get; set; } = 0;
    public int? Take { get; set; } = 25;

    public GetMatchingQualificationsQuery(string searchTerm, int? skip, int? take)
    {
        SearchTerm = searchTerm;
        Skip = skip;
        Take = take;
    }
}