using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

[ExcludeFromCodeCoverage]
public class GetRolloverStartSummaryQueryResponse
{
    public int TotalCandidatesCount { get; set; }
    public int CandidatesEligibleCount { get; set; }
    public int CandidatesIneligibleCount { get; set; }
    public int CandidatesRemainingCount { get; set; }
}
