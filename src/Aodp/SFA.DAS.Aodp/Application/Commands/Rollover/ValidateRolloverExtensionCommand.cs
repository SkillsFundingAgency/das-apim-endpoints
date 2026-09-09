using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    [ExcludeFromCodeCoverage]
    public class ValidateRolloverExtensionCommand : IRequest<BaseMediatrResponse<ValidateRolloverExtensionCommandResponse>>
    {
        public List<RolloverCandidateForValidation> RolloverCandidates { get; set; } = new();
    }

    [ExcludeFromCodeCoverage]
    public class RolloverCandidateForValidation
    {
        public int RowNumber { get; set; }
        public string Qan { get; set; } 
        public string FundingStreamName { get; set; }
        public string RollOverStatus { get; set; }
        public string? ExclusionReason { get; set; }
        public DateTime ProposedFundingApprovalEndDate { get; set; }
        public string? Comments { get; init; }
    }
}