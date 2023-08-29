using MediatR;
using SFA.DAS.AdminAan.Application.Entities;

namespace SFA.DAS.AdminAan.Application.Calendars.Queries.GetCalendars;
public class GetCalendarsQuery : IRequest<IEnumerable<Calendar>>
{
}
