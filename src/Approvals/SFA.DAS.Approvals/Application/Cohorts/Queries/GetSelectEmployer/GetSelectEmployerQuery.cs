using MediatR;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectEmployer;

public class GetSelectEmployerQuery : IRequest<GetSelectEmployerQueryResult>
{
    public int ProviderId { get; set; }
    public string SearchTerm { get; set; }
    public string SortField { get; set; }
    public bool ReverseSort { get; set; }
    public bool UseLearnerData { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
