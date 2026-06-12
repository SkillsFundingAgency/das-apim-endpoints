using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    [ExcludeFromCodeCoverage]
    public class ValidateFundingExtensionCandidatesCommand : IRequest<BaseMediatrResponse<ValidateFundingExtensionCandidatesCommandResponse>>
    {
        public List<FundingExtensionCandidate> FundingExtensionCandidates { get; set; } = new();
    }

    [ExcludeFromCodeCoverage]
    public class FundingExtensionCandidate
    {
        public int RowNumber { get; set; }
        public string? Qan { get; set; }
        public string? QualificationTitle { get; set; }
        public string? AwardingOrganisation { get; set; }
        public string? QualificationLevel { get; set; }
        public string? QualificationType { get; set; }
        public string? Ssa { get; set; }
        public DateTime? OperationalEndDate { get; set; }
        public bool? OfferedInEngland { get; set; }
        public bool? FundedInEngland { get; set; }
        public string? Glh { get; set; }
        public string? Tqt { get; set; }
        public bool? PreSixteen { get; set; }
        public bool? SixteenToEighteen { get; set; }
        public bool? EighteenPlus { get; set; }
        public bool? NineteenPlus { get; set; }
        public string? FundingStreamName { get; set; }
        public DateTime? FundingApprovalStartDate { get; set; }
        public string? ProposedOutcome { get; set; }
        public string? RollOverStatus { get; set; }
        public string? ExclusionReason { get; set; }
        public DateTime? CurrentFundingApprovalEndDate { get; set; }
        public DateTime? ProposedFundingApprovalEndDate { get; set; }
        public Guid RolloverCandidateId { get; set; }
        public Guid RolloverWorkflowCandidateId { get; set; }
        public bool IsActive { get; set; }
        public string? Comments { get; init; }
    }
}