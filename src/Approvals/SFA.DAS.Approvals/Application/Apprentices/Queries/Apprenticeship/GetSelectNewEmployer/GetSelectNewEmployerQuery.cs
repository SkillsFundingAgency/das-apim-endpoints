using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetSelectNewEmployer;

public class GetSelectNewEmployerQuery : IRequest<GetSelectNewEmployerQueryResult>
{
    public int ProviderId { get; set; }
    public string SearchTerm { get; set; }
    public string SortField { get; set; }
    public bool ReverseSort { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public long ApprenticeshipId { get; set; }
}