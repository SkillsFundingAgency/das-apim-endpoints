namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetRolloverWorkflowCandidatesQueryResponse
{
    public List<RolloverWorkflowCandidate> Data { get; set; } = new();
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public int TotalRecords { get; set; }
}

public class RolloverWorkflowCandidate
{
    public Guid Id { get; set; }
    public Guid RolloverWorkflowRunId { get; set; }
    public Guid QualificationVersionId { get; set; }
    public Guid FundingOfferId { get; set; }
    public string AcademicYear { get; set; } = null!;
    public Guid RolloverCandidateRecordId { get; set; }
    public bool PassP1 { get; set; }
    public string? P1FailureReason { get; set; }
    public bool IncludedInP1Export { get; set; }
    public bool IncludedInFinalUpload { get; set; }
    public DateTime CurrentFundingEndDate { get; set; }
    public DateTime? ProposedFundingEndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}