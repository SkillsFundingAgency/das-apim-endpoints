using MediatR;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
public class GetCalendarEventQueryHandler : IRequestHandler<GetCalendarEventQuery, GetCalendarEventQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetCalendarEventQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCalendarEventQueryResult?> Handle(GetCalendarEventQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.GetCalendarEvent(request.RequestedByMemberId, request.CalendarEventId, cancellationToken);
    }
}

