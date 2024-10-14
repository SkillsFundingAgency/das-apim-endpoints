using MediatR;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.AdminAan.Services;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandler : IRequestHandler<GetCalendarEventsQuery, GetCalendarEventsQueryResult?>
{
    public const int PageSize = 10;

    private readonly IAanHubRestApiClient _apiClient;
    private readonly ILocationLookupService _locationLookupService;

    public GetCalendarEventsQueryHandler(IAanHubRestApiClient apiClient, ILocationLookupService locationLookupService)
    {
        _apiClient = apiClient;
        _locationLookupService = locationLookupService;
    }

    public async Task<GetCalendarEventsQueryResult?> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
    {
        //if no location specified, then do as it currently does
        //otherwise:
        // call the locations api and lookup the specified location
        // if the location was not found, exit straight back with specific bool set on response, eg. "IsLocationNotFound"
        // otherwise:
        //  pass the co-ordinates of the location into the inner api with the radius

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            var locationData = await _locationLookupService.GetLocationInformation(request.Location, 0, 0, false);
            return new GetCalendarEventsQueryResult{LocationItem = locationData };
        }
        

        request.PageSize ??= PageSize;

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request);
        return await _apiClient.GetCalendarEvents(request.RequestedByMemberId!, parameters, cancellationToken);

    }
}
