namespace SFA.DAS.Aodp.Application.Queries.Rollover
{
    public class GetRolloverWorkflowCandidatesQueryResponse
    {
        public Guid WorkflowRunId { get; set; }
        public DateTime? FundingEndDateEligibilityThreshold { get; set; }
        public DateTime? OperationalEndDateEligibilityThreshold { get; set; }
        public DateTime? MaximumApprovalFundingEndDate { get; set; }
        public IEnumerable<RolloverWorkflowCandidate> RolloverWorkflowCandidates { get; set; } = new List<RolloverWorkflowCandidate>();
    }

    public class RolloverWorkflowCandidate
    {
        public Guid Id { get; set; }
        public Guid RolloverWorkflowRunId { get; set; }
        public Guid QualificationVersionId { get; set; }
        public Guid FundingOfferId { get; set; }
        public string AcademicYear { get; set; } = null!;
        public Guid RolloverCandidatesId { get; set; }
        public bool PassP1 { get; set; }
        public string? P1FailureReason { get; set; }
        public bool IncludedInP1Export { get; set; }
        public bool IncludedInFinalUpload { get; set; }
        public DateTime CurrentFundingEndDate { get; set; }
        public DateTime? ProposedFundingEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}