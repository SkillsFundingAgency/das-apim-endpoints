using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using System.Net;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandler : IRequestHandler<GetCalendarEventsQuery, GetCalendarEventsQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetCalendarEventsQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCalendarEventsQueryResult?> Handle(GetCalendarEventsQuery request,
        CancellationToken cancellationToken)
    {

        var response = await _apiClient.GetCalendarEvents(request.RequestedByMemberId.ToString(), cancellationToken);

        if (response.ResponseMessage.StatusCode == HttpStatusCode.OK)
            return response.GetContent();

        throw new InvalidOperationException($"Unexpected response received from aan hub api when getting calendar results for memberId");
    }
}
