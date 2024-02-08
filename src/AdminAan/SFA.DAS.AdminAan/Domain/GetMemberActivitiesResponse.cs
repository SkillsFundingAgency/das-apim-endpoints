namespace SFA.DAS.AdminAan.Domain;
public record GetMemberActivitiesResponse(DateTime? LastSignedUpDate, EventsModel EventsAttended, EventsModel EventsPlanned);

public record EventsModel(DateRangeModel EventsDateRange, List<EventAttendanceModel> Events);

public record DateRangeModel(DateTime FromDate, DateTime ToDate);

public record EventAttendanceModel(Guid CalendarEventId, DateTime EventDate, string EventTitle, long? Urn);