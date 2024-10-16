using MediatR;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.AdminAan.Services;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandler(IAanHubRestApiClient apiClient, ILocationLookupService locationLookupService)
    : IRequestHandler<GetCalendarEventsQuery, GetCalendarEventsQueryResult?>
{
    public const int DefaultPageSize = 10;

    public async Task<GetCalendarEventsQueryResult?> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
    {
        //if no location specified, then do as it currently does
        //otherwise:
        // call the locations api and lookup the specified location
        // if the location was not found, exit straight back with specific bool set on response, eg. "IsLocationNotFound"
        // otherwise:
        //  pass the co-ordinates of the location into the inner api with the radius

        double? latitude = null;
        double? longitude = null;
        int? radius = null;
        var orderBy = string.Empty;
        
        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            var locationData = await locationLookupService.GetLocationInformation(request.Location, 0, 0, false);
            if (locationData == null)
            {
                return new GetCalendarEventsQueryResult
                {
                    IsInvalidLocation = true
                };
            }

            latitude = locationData.GeoPoint[0];
            longitude = locationData.GeoPoint[1];
            radius = request.Radius;
            orderBy = request.OrderBy;
        }
        
        request.PageSize ??= DefaultPageSize;

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request, latitude, longitude, radius, orderBy);
        return await apiClient.GetCalendarEvents(request.RequestedByMemberId!, parameters, cancellationToken);
    }
}
