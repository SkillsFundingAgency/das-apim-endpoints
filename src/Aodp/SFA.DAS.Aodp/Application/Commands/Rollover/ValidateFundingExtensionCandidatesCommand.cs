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
        public string Qan { get; set; } = string.Empty;
        public string FundingStreamName { get; set; } = string.Empty;
        public DateTime ProposedFundingEndDate { get; set; }
        public string RolloverStatus { get; set; } = string.Empty;
        public string ExclusionReason { get; set; } = string.Empty;
    }
}