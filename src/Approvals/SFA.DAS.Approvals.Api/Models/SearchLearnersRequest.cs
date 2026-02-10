namespace SFA.DAS.Approvals.Api.Models;

public class SearchLearnersRequest
{
    public int? StartMonth { get; set; }
    public int StartYear { get; set; }
    public int Page { get; set; } = 1;
    public int? PageSize { get; set; } = 20;
    public string SortColumn { get; set; } = string.Empty;
    public bool SortDescending { get; set; } = false;
    public string CourseCode { get; set; } = null;
    public long? AccountLegalEntityId { get; set; }
    public long? CohortId { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}