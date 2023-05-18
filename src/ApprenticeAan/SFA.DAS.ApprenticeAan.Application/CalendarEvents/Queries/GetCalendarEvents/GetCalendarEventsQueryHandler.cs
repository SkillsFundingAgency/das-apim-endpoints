using MediatR;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.CalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandler : IRequestHandler<GetCalendarEventsQuery, GetCalendarEventsQueryResult?>
{
    private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;
    public GetCalendarEventsQueryHandler(IAanHubApiClient<AanHubApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCalendarEventsQueryResult?> Handle(GetCalendarEventsQuery query, CancellationToken cancellationToken)
    {
        var request = new GetCalendarEventsQueryRequest();

        // (_apiClient as HttpClient).DefaultRequestHeaders.Add("X-RequestedByMemberId", "3FA85F64-5717-4562-B3FC-2C963F66AFA6");
        // // HttpContext.Response.Headers.Add(RequestedByMemberIdHeader, requestedByMemberId.ToString());
        return await _apiClient.Get<GetCalendarEventsQueryResult>(request);
    }
}
