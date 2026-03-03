using System;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetReservationResponse
{
    public Guid Id { get; set; }
    public ReservationCourseResponse? Course { get; set; }
}

public class ReservationCourseResponse
{
    public byte? LearningType { get; set; }
}
