using MediatR;
using SFA.DAS.ApprenticeAan.Application.Model;

namespace SFA.DAS.ApprenticeAan.Application.Calendars.Queries.GetCalendars;

public class GetCalendarsQuery : IRequest<IEnumerable<Calendar>>
{
}