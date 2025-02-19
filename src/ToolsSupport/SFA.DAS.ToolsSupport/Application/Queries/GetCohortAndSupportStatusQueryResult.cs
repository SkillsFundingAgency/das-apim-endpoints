namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetCohortAndSupportStatusQueryResult
{
    public long CohortId { get; set; }
    public string EmployerAccountName { get; set; }
    public string ProviderName { get; set; }
    public long UkPrn { get; set; }
    public string Status { get; set; }
    public string CohortReference { get; set; }
    public int NoOfApprentices { get; set; }
    public string CohortStatus { get; set; }

}