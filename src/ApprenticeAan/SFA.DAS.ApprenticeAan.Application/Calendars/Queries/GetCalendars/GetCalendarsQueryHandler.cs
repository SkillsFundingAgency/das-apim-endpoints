using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Calendars.Queries.GetCalendars;

public class GetCalendarsQueryHandler : IRequestHandler<GetCalendarsQuery, IEnumerable<CalendarModel>>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetCalendarsQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<CalendarModel>> Handle(GetCalendarsQuery request, CancellationToken cancellationToken)
    {
        var calendars = await _apiClient.GetCalendars(cancellationToken);
        return calendars.Select(cal => (CalendarModel)cal);
    }
}