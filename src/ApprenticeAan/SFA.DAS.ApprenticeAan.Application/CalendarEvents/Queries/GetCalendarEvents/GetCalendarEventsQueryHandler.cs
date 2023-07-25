﻿using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Services;

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
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request);
        return await _apiClient.GetCalendarEvents(request.RequestedByMemberId!, parameters, cancellationToken);
    }

}
