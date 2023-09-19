using MediatR;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEventById;
public class GetCalendarEventByIdQueryHandler : IRequestHandler<GetCalendarEventByIdQuery, CalendarEvent?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetCalendarEventByIdQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<CalendarEvent?> Handle(GetCalendarEventByIdQuery command, CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetCalendarEventById(command.CalendarEventId, command.RequestedByMemberId, cancellationToken);

        return result.ResponseMessage.StatusCode != System.Net.HttpStatusCode.OK
            ? null
            : result.GetContent();
    }
}
