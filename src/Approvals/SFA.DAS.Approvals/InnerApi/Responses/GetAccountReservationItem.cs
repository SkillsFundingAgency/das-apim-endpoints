using System;

namespace SFA.DAS.Approvals.InnerApi.Responses;
public class GetAccountReservationItem
{
    public Guid Id { get; set; }
    public bool IsExpired { get; set; }
    public ReservationStatus Status { get; set; }
}

public enum ReservationStatus
{
    Pending = 0,
    Confirmed = 1,
    Completed = 2,
    Deleted = 3,
    Change = 4
}