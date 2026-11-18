using System;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class SelectMultipleReservationAssignment
{
    public long LearnerId { get; set; }
    public Guid? ReservationId { get; set; }
    public bool UseNewReservation { get; set; }
}
