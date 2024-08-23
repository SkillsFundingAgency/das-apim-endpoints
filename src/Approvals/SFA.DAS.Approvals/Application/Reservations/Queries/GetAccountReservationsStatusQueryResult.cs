namespace SFA.DAS.Approvals.Application.Reservations.Queries;

public class GetAccountReservationsStatusQueryResult
{
    public bool CanAutoCreateReservations { get; set; }
    public bool HasReachedReservationsLimit { get; set; }
    public int UnallocatedPendingReservations  { get; set; }
}