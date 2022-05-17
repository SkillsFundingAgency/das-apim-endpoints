namespace SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetIndex
{
    public class GetIndexQueryResult
    {
        public int PledgesCount { get; set; }
        public int ApplicationsCount { get; set; }
        public bool IsTransferReceiver { get; set; }
        public decimal ActivePledgesTotalAmount { get; set; }
    }
}
