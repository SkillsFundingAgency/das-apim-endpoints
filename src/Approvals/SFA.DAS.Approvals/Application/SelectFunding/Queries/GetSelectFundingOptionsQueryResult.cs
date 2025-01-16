namespace SFA.DAS.Approvals.Application.SelectFunding.Queries;

public class GetSelectFundingOptionsQueryResult
{
    public bool IsLevyAccount { get; set; }
    public bool HasDirectTransfersAvailable { get; set; }
    public bool HasLtmTransfersAvailable { get; set; }
    public bool HasUnallocatedReservationsAvailable { get; set; }
    public bool HasAdditionalReservationFundsAvailable { get; set; }
}
