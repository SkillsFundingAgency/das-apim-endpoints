namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetPledgeApplicationResponse
    {
        public long SenderEmployerAccountId { get; set; }
        public long ReceiverEmployerAccountId { get; set; }
        public string Status { get; set; }
        public bool AutomaticApproval { get; set; }
        public int TotalAmount { get; set; }
        public int AmountUsed { get; set; }
        public int AmountRemaining => TotalAmount - AmountUsed;
        public int PledgeId { get; set; }
    }
}
