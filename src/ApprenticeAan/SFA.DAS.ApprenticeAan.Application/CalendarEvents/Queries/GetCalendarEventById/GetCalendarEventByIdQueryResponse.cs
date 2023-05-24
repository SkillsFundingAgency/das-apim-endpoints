using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using System.Net;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEventById;

public record GetCalendarEventByIdQueryResponse(CalendarEvent? CalendarEvent, HttpStatusCode StatusCode);
