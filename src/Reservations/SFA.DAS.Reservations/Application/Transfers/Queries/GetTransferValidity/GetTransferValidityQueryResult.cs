namespace SFA.DAS.Reservations.Application.Transfers.Queries.GetTransferValidity
{
    public class GetTransferValidityQueryResult
    {
        public bool IsValid { get; set; }
        public string PledgeApplicationStatus { get; set; }
    }
}