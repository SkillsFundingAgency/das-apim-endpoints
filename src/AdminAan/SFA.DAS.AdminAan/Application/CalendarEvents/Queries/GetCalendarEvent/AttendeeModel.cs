namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;

public record AttendeeModel(Guid MemberId, string UserType, string MemberName, DateTime? AddedDate);