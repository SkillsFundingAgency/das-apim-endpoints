using MediatR;
using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.Application.Calendars.Queries.GetCalendars;
public class GetCalendarsQuery : IRequest<IEnumerable<Calendar>>
{
}
