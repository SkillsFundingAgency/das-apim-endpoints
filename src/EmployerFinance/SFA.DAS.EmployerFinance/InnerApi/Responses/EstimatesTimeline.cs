using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public class EstimatesTimeline
{
    public long AccountId { get; set; }
    public decimal TotalEstimatedCommitedCost { get; set; }
    public decimal TotalEstimatedCommitedTransferCost { get; set; }
    public List<MonthlyFundingBreakdown> Projections { get; set; } = [];
}