namespace SFA.DAS.EmployerFinance.Api.Models.Transfers
{
    public class GetCountsResponse
    {
        public int PledgesCount { get; set; }
        public int ApplicationsCount { get; set; }
        public decimal CurrentYearEstimatedCommittedSpend { get; set; }
    }
}
