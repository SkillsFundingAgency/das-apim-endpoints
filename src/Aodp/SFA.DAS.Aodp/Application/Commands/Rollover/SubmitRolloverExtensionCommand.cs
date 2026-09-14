using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AODP.Application.Commands.Rollover
{
    [ExcludeFromCodeCoverage]
    public class SubmitRolloverExtensionCommand : IRequest<BaseMediatrResponse<SubmitRolloverExtensionCommandResponse>>
    {
        public List<FundingExtensionItem> Items { get; set; } = new();
    }

    public class FundingExtensionItem
    {
        public string Qan { get; set; }
        public string FundingStreamName { get; set; }
        public string RolloverStatus { get; set; }
        public string? ExclusionReason { get; set; }
        public DateTime ProposedFundingApprovalEndDate { get; set; }
        public string? Comments { get; set; }
    }

}