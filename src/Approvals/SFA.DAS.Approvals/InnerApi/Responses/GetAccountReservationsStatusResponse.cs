namespace SFA.DAS.Approvals.InnerApi.Responses;
public class GetAccountReservationsStatusResponse
{
    public bool CanAutoCreateReservations { get; set; }
    public bool HasReachedReservationsLimit { get; set; }
    public bool HasPendingReservations { get; set; }
}