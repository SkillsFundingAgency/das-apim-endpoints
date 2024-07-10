namespace SFA.DAS.EmployerAan.InnerApi.Standards.Responses;

public class StandardVersionDetail
{
    public DateTime? EarliestStartDate { get; set; }
    public DateTime? LatestStartDate { get; set; }
    public DateTime? LatestEndDate { get; set; }
    public DateTime? ApprovedForDelivery { get; set; }
    public int ProposedTypicalDuration { get; set; }
    public int ProposedMaxFunding { get; set; }
}