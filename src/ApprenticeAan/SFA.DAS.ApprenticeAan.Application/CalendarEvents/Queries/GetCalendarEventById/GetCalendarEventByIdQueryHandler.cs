using MediatR;
using RestEase;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEventById;

public class GetCalendarEventByIdQueryHandler : IRequestHandler<GetCalendarEventByIdQuery, GetCalendarEventByIdQueryResponse>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetCalendarEventByIdQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCalendarEventByIdQueryResponse> Handle(GetCalendarEventByIdQuery command, CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetCalendarEventById(command.CalendarEventId, command.RequestedByMemberId, cancellationToken);
        return new GetCalendarEventByIdQueryResponse(result.GetContent(), result.ResponseMessage.StatusCode);
    }
}
