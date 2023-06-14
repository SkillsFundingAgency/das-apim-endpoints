using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Calendars.Queries.GetCalendars;

public class GetCalendarsQuery : IRequest<IEnumerable<CalendarModel>>
{
}