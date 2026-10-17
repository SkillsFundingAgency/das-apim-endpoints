using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public class EstimatesTimeline
{
    public long AccountId { get; set; }
    public List<MonthlyFundingBreakdown> Projections { get; set; } = [];
}