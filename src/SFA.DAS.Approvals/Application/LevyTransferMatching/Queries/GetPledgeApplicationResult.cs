namespace SFA.DAS.Approvals.Application.LevyTransferMatching.Queries
{
    public class GetPledgeApplicationResult
    {
        public long SenderEmployerAccountId { get; set; }
        public long ReceiverEmployerAccountId { get; set; }
        public string Status { get; set; }
        public bool AutomaticApproval { get; set; }
        public int TotalAmount { get; set; }
        public int AmountUsed { get; set; }
        public int AmountRemaining { get; set; }
        public int PledgeId { get; set; }
    }
}