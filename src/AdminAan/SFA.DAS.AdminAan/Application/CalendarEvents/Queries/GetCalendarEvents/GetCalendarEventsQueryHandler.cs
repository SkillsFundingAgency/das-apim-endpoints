using MediatR;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.AdminAan.Infrastructure.Configuration;
using SFA.DAS.AdminAan.Services;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandler : IRequestHandler<GetCalendarEventsQuery, GetCalendarEventsQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetCalendarEventsQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCalendarEventsQueryResult?> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
    {
        request.PageSize ??= Constants.AdminSettings.PageSize;

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request);
        return await _apiClient.GetCalendarEvents(request.RequestedByMemberId!, parameters, cancellationToken);

    }
}
