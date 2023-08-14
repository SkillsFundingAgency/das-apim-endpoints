using MediatR;
using SFA.DAS.EmployerAan.Entities;

namespace SFA.DAS.EmployerAan.Application.Calendars.Queries.GetCalendars;
public class GetCalendarsQuery : IRequest<IEnumerable<Calendar>>
{
}
