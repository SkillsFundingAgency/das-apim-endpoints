using SFA.DAS.Approvals.Application.LevyTransferMatching.Queries;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetPledgeApplicationResponse
    {
        public long SenderEmployerAccountId { get; set; }
        public long ReceiverEmployerAccountId { get; set; }
        public string Status { get; set; }
        public bool AutomaticApproval { get; set; }
        public int TotalAmount { get; set; }
        public int AmountUsed { get; set; }
        public int AmountRemaining { get; set; }
        public int PledgeId { get; set; }

        public static implicit operator GetPledgeApplicationResponse(GetPledgeApplicationResult source)
        {
            return new GetPledgeApplicationResponse
            {
                SenderEmployerAccountId = source.SenderEmployerAccountId,
                ReceiverEmployerAccountId = source.ReceiverEmployerAccountId,
                Status = source.Status,
                AutomaticApproval = source.AutomaticApproval,
                TotalAmount = source.TotalAmount,
                AmountUsed = source.AmountUsed,
                AmountRemaining = source.AmountRemaining,
                PledgeId = source.PledgeId
            };
        }
    }
}
