using System;
using System.Text.Json.Serialization;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetReservationResponse
{
    public Guid Id { get; set; }
    public ReservationCourseResponse? Course { get; set; }
}

public class ReservationCourseResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LearningType? LearningType { get; set; }
}
