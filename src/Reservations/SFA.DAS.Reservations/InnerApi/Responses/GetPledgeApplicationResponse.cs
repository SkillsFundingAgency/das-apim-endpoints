namespace SFA.DAS.Reservations.InnerApi.Responses
{
    public class GetPledgeApplicationResponse
    {
        public long SenderEmployerAccountId { get; set; }
        public long ReceiverEmployerAccountId { get; set; }
        public string Status { get; set; }
    }
}
