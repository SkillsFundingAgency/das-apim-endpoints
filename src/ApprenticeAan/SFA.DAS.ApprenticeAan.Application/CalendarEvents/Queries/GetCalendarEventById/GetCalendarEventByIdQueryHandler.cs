using MediatR;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEventById;

public class GetCalendarEventByIdQueryHandler : IRequestHandler<GetCalendarEventByIdQuery, CalendarEventSummary?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetCalendarEventByIdQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<CalendarEventSummary?> Handle(GetCalendarEventByIdQuery command, CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetCalendarEventById(command.CalendarEventId, command.RequestedByMemberId, cancellationToken);

        return result.ResponseMessage.StatusCode != System.Net.HttpStatusCode.OK
            ? null
            : result.GetContent();
    }
}
