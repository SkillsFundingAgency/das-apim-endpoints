using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryHandler(IAanHubRestApiClient apiClient) : IRequestHandler<GetMemberNotificationSettingsQuery, GetMemberNotificationSettingsQueryResult>
{
    public async Task<GetMemberNotificationSettingsQueryResult> Handle(GetMemberNotificationSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await apiClient.GetMemberNotificationSettings(request.MemberId, cancellationToken);

        var result = new GetMemberNotificationSettingsQueryResult
        {
            ReceiveMonthlyNotifications = settings.ReceiveNotifications,
            MemberNotificationEventFormats = settings.EventTypes.Any()
                ? settings.EventTypes.Select(x => new GetMemberNotificationSettingsQueryResult.EventType
                {
                    EventFormat = x.EventType,
                    Ordering = x.Ordering,
                    ReceiveNotifications = x.ReceiveNotifications
                }).ToList()
                : [],
            MemberNotificationLocations = settings.Locations.Any()
                ? settings.Locations.Select(x => new GetMemberNotificationSettingsQueryResult.Location
                {
                    Name = x.Name,
                    Radius = x.Radius,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                }).ToList()
                : []
        };

        return result;
    }
}
